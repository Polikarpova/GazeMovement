using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Rectangle = System.Drawing.Rectangle;
using Point = System.Drawing.Point;
using GazeMovementClient.Logic;
using GazeMovementClient.Model;
using GazeMovementClient.Exceptions;

namespace GazeMovementClient.View
{
    public partial class MainForm : Form
    {
        //База данных
        private IDatabase DB;

        //Сбор данных
        private IDataCollection DC;

        //Анализ данных
        private IDataAnalysis DA;

        //Режим рисования
        private bool isDrawMode = false;
        private TransparentPanel tp;
        private Point startXY = new Point(-1, -1);
        private Point endXY = new Point(-1, -1);
        private bool isPenMoving = false;
        private float penWidth = 2;
        private Color penColor = Color.Wheat;
        private string selectedObjectClass = "";

        //Выбор изображений
        private List<int> selectedImages = new List<int>();

        //Инициализация формы
        public MainForm()
        {
            InitializeComponent();

            this.upperTab.SelectedIndex = 0;

            //пытаемся проинициализировать БД
            try
            {
                DB = new Database();
                UpdateImageNameList();
                UpdateSessionsList();
            }
            catch (DatabaseException dEx)
            {
                DialogResult result = MessageBox.Show(dEx.Message + Environment.NewLine + "Если вы нажмете 'Ок' база данных будет стерта." + Environment.NewLine +
                                                      "Если вы нажмете 'Отмена', то данные не будут стерты и программа закроется.", "Упс!"
                                                      , MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation,
                                                      MessageBoxDefaultButton.Button1);
                if (result == DialogResult.OK)
                {
                    //Пересоздаем БД - папка с изображениями будет очищена!
                    Database.RemakeDatabase();

                    //Снова инициализируем БД
                    DB = new Database();
                }
                else if (result == DialogResult.Cancel)
                {
                    //Выключаем программу
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "Сейчас программа закроется", "Упс!");

                //Выключаем программу
                Environment.Exit(0);
            }


            DC = new DataCollection(DB);
            DA = new DataAnalysis(DB);

            InitTransparentPanel();
        }

        //--------------Библиотека изображений--------------
        private void DownloadImageButton_Click(object sender, EventArgs e)
        {
            //Настраиваем форму для выбора изображения
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите изображение...",
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Image files (*.jpeg;*.jpg;*.png)|*.jpeg;*.jpg;*.png"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //заблокировать окно
                this.Enabled = false;

                try
                {
                    //проверка изображения
                    DB.CheckDownloadingImage(openFileDialog.FileName);
                    System.IO.FileInfo imageInfo = new System.IO.FileInfo(openFileDialog.FileName);

                    //запрос имени
                    NewImageNameDialog dialog = new NewImageNameDialog(System.IO.Path.GetFileNameWithoutExtension(imageInfo.Name));
                    dialog.ShowDialog();
                    string imageName = dialog.FileName;

                    //проверка имени
                    DB.CheckNewImageName(imageName);

                    //загрузка
                    DB.DownloadImage(imageInfo.FullName, imageName);

                    //обновление imageNameList
                    UpdateImageNameList();
                }
                catch ( ImageException ex )
                {
                    MessageBox.Show(ex.Message, "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch ( Exception ex )
                {
                    MessageBox.Show(ex.Message, "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //разблокировать окно
                this.Enabled = true;
            }
        }

        private void UpdateImageNameList()
        {
            this.imageNameList.Items.Clear();

            List<string> names = DB.GetImageNames();

            foreach (string name in names)
            {
                this.imageNameList.Items.Add(name);
            }

            this.imageNameList.Refresh();
            this.imageNameList.SelectedIndex = -1;
            ImageNameList_SelectedIndexChanged(this, new EventArgs());
        }

        private void ImageNameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (imageNameList.SelectedIndex != -1)
            {
                //Показать изображение в image
                try
                {
                    this.image.BackgroundImage = DB.GetImage(imageNameList.SelectedIndex);
                    this.addNewObjectButton.Enabled = true;
                    this.deleteAllObjectsButton.Enabled = true;
                    this.showObjectsButton.Enabled = true;
                }
                catch
                { }
            }
            else
            {
                this.addNewObjectButton.Enabled = false;
                this.deleteAllObjectsButton.Enabled = false;
                this.showObjectsButton.Enabled = false;
            }

            UpdateEyePathDataList();
        }

        private void ShowObjectsButton_Click(object sender, EventArgs e)
        {
            //отобразить все ранее выделенные объекты
            List<Rectangle> list = DB.GetAllHighlightedObjectsAsRectangle(imageNameList.SelectedIndex);

            if (list.Count > 0)
            {
                //спрашиваем цвет
                ColorDialog colorDialog = new ColorDialog();

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    this.penColor = colorDialog.Color;
                }

                foreach (Rectangle rectangle in list)
                {
                    using (Graphics g = tp.CreateGraphics())
                    {
                        //масштабируем прямоугольник к размеру pictureBox
                        float xScale = (float)image.Width / (float)SystemInformation.VirtualScreen.Width;
                        float yScale = (float)image.Height / (float)SystemInformation.VirtualScreen.Height;
                        Rectangle rec = ScaleRectangle(rectangle, xScale, yScale);

                        //рисуем
                        g.DrawRectangle(new Pen(penColor, penWidth), rec);
                    }
                }
            }
            else
            {
                MessageBox.Show("У этого изображения нет выделенных объектов.", "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /// <summary>
        /// Масштабирование прямоугольника от одних рамеров к другим
        /// </summary>
        /// <param name="rec">масштабируемый прямоугольник</param>
        /// <param name="xScale">коэффициент по X</param>
        /// <param name="yScale">коэффициент по Y</param>
        /// <returns>новый прямоугольник</returns>
        private Rectangle ScaleRectangle(Rectangle rec, float xScale, float yScale)
        {
            int X = (int)((float)rec.X * xScale);
            int Y = (int)((float)rec.Y * yScale);
            int Width = (int)((float)rec.Width * xScale);
            int Height = (int)((float)rec.Height * yScale);

            return new Rectangle(X, Y, Width, Height);
        }

        private void DeleteImageButton_Click(object sender, EventArgs e)
        {
            if (imageNameList.SelectedIndex == -1)
            {
                MessageBox.Show("Вы должны выбрать изображение из списка!", "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult result = MessageBox.Show("Вместе с этим изображением из системы будет удалена следующая информация:" + Environment.NewLine +
                                                  "- все выделенные на этом изображении объекты;" + Environment.NewLine +
                                                  "- все сохраненные для этого изображения траектории пути взгляда;" + Environment.NewLine +
                                                  "- все сессии, если они содержат записанный путь взгляда только для этого изображения.", "Удалить это иозображение?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);


                if (result == DialogResult.Yes)
                {
                    //заблокировать окно
                    this.Enabled = false;

                    //убираем изображение с image
                    image.BackgroundImage = null;
                    image.Refresh();

                    //удалить
                    DB.DeleteImage(imageNameList.SelectedIndex);

                    //Удаляем выбранное изображение из списка выбранных, если оно там есть, а идущие после него уменьшаем на 1
                    if (this.selectedImages.Contains(imageNameList.SelectedIndex))
                    {
                        int deletedIndex = this.selectedImages.IndexOf(imageNameList.SelectedIndex);
                        this.selectedImages.Remove(imageNameList.SelectedIndex);
                        for (int i = deletedIndex; i < selectedImages.Count; i++)
                        {
                            this.selectedImages[i]--;
                        }
                    }

                    //обновить imageNameList
                    UpdateImageNameList();

                    //обновить лист сессий
                    UpdateSessionsList();

                    //разблокировать окно
                    this.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Инициализация прозрачной панели, которая накладывается на изображение, чтобы рисовать на ней
        /// </summary>
        private void InitTransparentPanel()
        {
            tp = new TransparentPanel
            {
                //настраиваем размеры и добавляем на картинку
                Size = image.Size,
                Location = image.Location
            };
            this.Controls.Add(tp);
            tp.BringToFront();

            //настраиваем её как холст для рисования
            tp.MouseDown += new MouseEventHandler(this.Tp_MouseDown);
            tp.MouseUp += new MouseEventHandler(this.Tp_MouseUp);
            tp.MouseMove += new MouseEventHandler(this.Tp_MouseMove);
            
        }

        //--------------Добавление новых объектов
        /// <summary>
        /// В этой функции запрашивает класс выделяемого объекта, включается режим рисования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewObjectButton_Click(object sender, EventArgs e)
        {
            //Спрашиваем о классе
            ChooseObjectClassDialog dialog = new ChooseObjectClassDialog(DB.GetObjectClassNames());

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //сохраняем класс объекта
                this.selectedObjectClass = dialog.SelectedItemText;

                //отключаем все остальные функции
                EnabledFunctions(false); //вернуть всё на место в Tp_MouseUp

                //даем возможность порисовать
                isDrawMode = true; //выключается в Tp_MouseUp

                //спрашиваем цвет
                ColorDialog colorDialog = new ColorDialog();

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    this.penColor = colorDialog.Color;
                }
            }
        }

        /// <summary>
        /// Включает/выключает все функции, кроме кнопки для удаления всех объектов на изображении
        /// </summary>
        /// <param name="flag">Флаг видимости</param>
        private void EnabledFunctions(bool flag)
        {
            this.upperTab.Enabled = this.imageNameList.Enabled = this.downloadImageButton.Enabled = this.deleteImageButton.Enabled = this.addNewObjectButton.Enabled = this.showObjectsButton.Enabled = flag;
        }

        private void Tp_MouseDown(object sender, MouseEventArgs e)
        {
            if (isDrawMode)
            {
                isPenMoving = true;
                startXY = e.Location;
            }
        }

        private void Tp_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawMode)
            {
                isPenMoving = false;

                //проверяем, что координаты не выходят за рамки изображения, редактируем их, если это происходит
                ProcessCoordinates();

                //создаем полученный прямоугольник
                Rectangle rectangle = new Rectangle
                {
                    X = Math.Min(startXY.X, endXY.X),
                    Y = Math.Min(startXY.Y, endXY.Y),
                    Width = Math.Abs(startXY.X - endXY.X),
                    Height = Math.Abs(startXY.Y - endXY.Y)
                };

                //проверяем, что область не пересекается с другими
                bool isAllright = CheckRectangle(rectangle);

                if (isAllright)
                {
                    //отображаем полученную область
                    using (Graphics g = tp.CreateGraphics())
                    {
                        g.DrawRectangle(new Pen(penColor, penWidth), rectangle);
                    }

                    //масштабируем rectangle к реальным размерам image.BackgroundImage
                    float xScale = (float)SystemInformation.VirtualScreen.Width / (float)image.Width;
                    float yScale = (float)SystemInformation.VirtualScreen.Height / (float)image.Height;
                    Rectangle rec = ScaleRectangle(rectangle, xScale, yScale);

                    //сохраняем область и класс в БД
                    DB.SaveHighlightedObject(imageNameList.SelectedIndex, selectedObjectClass, rec);
                }
                else
                {
                    MessageBox.Show("Новая область пересекается с другими!", "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                //выключаем режим рисования
                isDrawMode = false;

                //активируем заблокированные функции
                EnabledFunctions(true);
            }
        }

        private void ProcessCoordinates()
        {
            //координаты относительно левого верхнего угла изображения
            if (endXY.X < 0) endXY.X = 0;
            if (endXY.Y < 0) endXY.Y = 0;

            if (endXY.X > image.Width) endXY.X = image.Width;
            if (endXY.Y > image.Height) endXY.Y = image.Height;
        }

        private bool CheckRectangle(Rectangle rectangle)
        {
            bool isCoordinatesNotIntersect = true;

            //получить координаты всех областей для текущего изображения
            List<Rectangle> rectangles = DB.GetAllHighlightedObjectsAsRectangle(this.imageNameList.SelectedIndex);

            //проверить координаты в цикле, пока не найдем пересечение
            if (rectangles.Count != 0)
            {
                //масштабируем rectangle к реальным размерам image.BackgroundImage
                float xScale = (float)SystemInformation.VirtualScreen.Width / (float)image.Width;
                float yScale = (float)SystemInformation.VirtualScreen.Height / (float)image.Height;
                Rectangle rec = ScaleRectangle(rectangle, xScale, yScale);

                int i = 0;
                while (isCoordinatesNotIntersect && i < rectangles.Count)
                {
                    isCoordinatesNotIntersect = !rec.IntersectsWith(rectangles[i]);

                    i++;
                }
            }

            return isCoordinatesNotIntersect;
        }

        private void Tp_MouseMove(object sender, MouseEventArgs e)
        {
            if ( isDrawMode && isPenMoving )
            {
                endXY = e.Location;
            }
        }

        //--------------Удаление объектов
        private void DeleteAllObjectsButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалит все выделенные на этом изображении объекты?", "Удалить все выделенные объекты?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                //удалить
                DB.DeleteHighlightedObjects(imageNameList.SelectedIndex);

                //Переинициализируем панель для рисования
                this.Controls.Remove(tp);
                InitTransparentPanel();
            }
        }

        //--------------Чтение движения взгляда--------------
        private void SelectImagesButton_Click(object sender, EventArgs e)
        {
            if (DB.ImagesCount == 0)
            {
                MessageBox.Show("Ни одно изображение не было добавлено!", "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                SelectImagesDialog dialog = new SelectImagesDialog(this.DB, this.selectedImages);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    //сохраняем выбранные изображения
                    this.selectedImages.Clear();
                    this.selectedImages.AddRange(dialog.SelectedImages);
                }
            }
        }

        private void SessionName_TextChanged(object sender, EventArgs e)
        {
            if (this.sessionName.Text.Contains('_') || this.sessionName.Text.Contains(' '))
            {
                this.sessionName.Text = this.sessionName.Text.Replace('_', '-');
                this.sessionName.Text = this.sessionName.Text.Replace(' ', '-');
                this.sessionName.SelectionStart = this.sessionName.Text.Length;
            }
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            //проверка имени сессии
            if (this.sessionName.Text.Length == 0)
            {
                MessageBox.Show("Необходимо задать имя сессии.", "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if ( DB.IsSessionExist(this.sessionName.Text) )
            {
                MessageBox.Show($"Сессия с именем {this.sessionName.Text} уже существует.", "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (this.selectedImages.Count != 0)
                {
                    //Вызов формы в которой всё происходит
                    DataCollectionDialog dialog = new DataCollectionDialog(DB, DC, this.selectedImages, (int)durationNum.Value, this.sessionName.Text, this.isWithPause.Checked);
                    dialog.ShowDialog();

                    UpdateEyePathDataList();
                    UpdateSessionsList();

                    //Очищаем поле с именем сесии
                    this.sessionName.Text = "";
                }
                else
                {
                    MessageBox.Show("Вы должны выбрать изображения.", "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        //--------------Анализ данных--------------
        private void EyePathDataList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eyePathDataList.SelectedIndex != -1)
            {
                //включить кнопки
                this.blocksButton.Enabled = true;
                this.gazePlotButton.Enabled = true;
                this.deleteEyePathButton.Enabled = true;
            }
            else
            {
                this.blocksButton.Enabled = false;
                this.gazePlotButton.Enabled = false;
                this.deleteEyePathButton.Enabled = false;
            }
        }

        private void UpdateEyePathDataList()
        {
            this.eyePathDataList.Items.Clear();

            if (imageNameList.SelectedIndex != -1)
            {
                List<string> names = DB.GetEyePathNames(imageNameList.SelectedIndex);

                foreach (string name in names)
                {
                    this.eyePathDataList.Items.Add(name);
                }
            }

            this.eyePathDataList.Refresh();
            this.eyePathDataList.SelectedIndex = -1;
            EyePathDataList_SelectedIndexChanged(this, new EventArgs());
        }

        private void DeleteEyePathButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить этот путь взгляда?", "Удалить этот путь взгляда?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

            if (result == DialogResult.Yes)
            {
                //удалить
                DB.DeleteEyePath(imageNameList.SelectedIndex, eyePathDataList.SelectedIndex);

                //обновить imageNameList
                UpdateEyePathDataList();
                UpdateSessionsList();

                //Обновляем панель
                this.Controls.Remove(tp);
                InitTransparentPanel();
            }
        }

        private void GazePlotButton_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            Color color;

            if ( dialog.ShowDialog() == DialogResult.OK)
            {
                color = dialog.Color;
            }
            else
            {
                color = Color.Black;
            }

            //Отобразить все ранее выделенные объекты
            List<PointF> list = DB.GetEyePath(imageNameList.SelectedIndex, eyePathDataList.SelectedIndex);

            if (list.Count > 0)
            {
                using (Graphics g = tp.CreateGraphics())
                {
                    Pen circlePen = new Pen(color, 2);
                    Pen linePen = new Pen(color, 1);
                    int width = image.Width;
                    int height = image.Height;
                    int screenWidth = SystemInformation.VirtualScreen.Width;
                    int screenHeight = SystemInformation.VirtualScreen.Height;

                    g.DrawEllipse(circlePen, (list[0].X*width/ screenWidth), (list[0].Y * height/ screenHeight),2,2);

                    for (int i = 1; i < list.Count; i++)
                    {
                        g.DrawLine(linePen,
                                   (list[i - 1].X * width / screenWidth), (list[i - 1].Y * height / screenHeight),
                                   (list[i].X * width / screenWidth), (list[i].Y * height / screenHeight));
                        g.DrawEllipse(circlePen, (list[i].X * width / screenWidth), (list[i].Y * height / screenHeight), 2, 2);
                    }
                }
            }
        }

        private void BlocksButton_Click(object sender, EventArgs e)
        {
            //если области есть
            if (DB.IsHighlightedObjectsExist(imageNameList.SelectedIndex) != 0)
            {
                ColorDialog dialog = new ColorDialog();
                Color color;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    color = dialog.Color;
                }
                else
                {
                    color = Color.Black;
                }

                //получить лист листов точек
                List<List<PointF>> lists = DA.GetPointsFromHighlightedObjects(imageNameList.SelectedIndex, eyePathDataList.SelectedIndex);

                foreach (List<PointF> list in lists)
                {
                    if (list.Count > 0)
                    {
                        using (Graphics g = tp.CreateGraphics())
                        {
                            Pen circlePen = new Pen(color, 2);
                            Pen linePen = new Pen(color, 1);
                            int width = image.Width;
                            int height = image.Height;
                            int screenWidth = SystemInformation.VirtualScreen.Width;
                            int screenHeight = SystemInformation.VirtualScreen.Height;

                            g.DrawEllipse(circlePen, (list[0].X * width / screenWidth), (list[0].Y * height / screenHeight), 2, 2);

                            for (int i = 1; i < list.Count; i++)
                            {
                                g.DrawLine(linePen,
                                            (list[i - 1].X * width / screenWidth), (list[i - 1].Y * height / screenHeight),
                                            (list[i].X * width / screenWidth), (list[i].Y * height / screenHeight));
                                g.DrawEllipse(circlePen, (list[i].X * width / screenWidth), (list[i].Y * height / screenHeight), 2, 2);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Это изображение не имеет выделенных объектов!", "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        
        private void UpdateSessionsList()
        {
            this.sessionsList.Items.Clear();

            List<string> names = DB.GetSessionNames();

            foreach (string name in names)
            {
                this.sessionsList.Items.Add(name);
            }

            this.sessionsList.Refresh();
            this.sessionsList.SelectedIndex = -1;
            SessionsList_SelectedIndexChanged(this, new EventArgs());
        }

        private void SessionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sessionsList.SelectedIndex != -1)
            {
                this.commonPaternsButton.Enabled = true;
                this.deleteSessionButton.Enabled = sessionsList.SelectedIndices.Count == 1;
            }
            else
            {
                this.commonPaternsButton.Enabled = false;
                this.deleteSessionButton.Enabled = false;
            }
        }

        private void DeleteSessionButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить эту сессию?", "Удалить эту сессию?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

            if (result == DialogResult.Yes)
            {
                //удалить
                DB.DeleteSession(sessionsList.SelectedIndex);

                //обновить
                UpdateSessionsList();
                UpdateEyePathDataList();
            }
        }

        private void CommonPaternsButton_Click(object sender, EventArgs e)
        {
            if ( !DB.IsObjectClassesExist())
            {
                MessageBox.Show("Вы еще не добавили ни один класс объекта!", "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                //заблокировать окно
                this.Enabled = false;

                //получить проанализированные данные
                List<int> sessionIndices = new List<int>();
                foreach (var index in sessionsList.SelectedIndices)
                {
                    sessionIndices.Add((int)index);
                }

                List<ObjectClassInfo> result = DA.FindCommonPatternsInSessions(sessionIndices);

                //разблокировать
                this.Enabled = true;

                //вывести данные на экран
                ProcessedDataDialog dialog = new ProcessedDataDialog(DB, result);
                dialog.ShowDialog();

                //сохранить ли новые данные?
                DialogResult dialogResult = MessageBox.Show("Сохранить эти данные?", "Сохранить?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                if (dialogResult == DialogResult.Yes)
                {
                    DB.RewriteObjectClasses(result);
                }
            }
        }

        private List<Rectangle> ResizeToPictureBox(List<Rectangle> rectangles)
        {
            List<Rectangle> result = new List<Rectangle>();

            foreach (Rectangle rectangle in rectangles)
            {
                float xScale = (float)image.BackgroundImage.Width / (float)SystemInformation.VirtualScreen.Width;
                float yScale = (float)image.BackgroundImage.Height / (float)SystemInformation.VirtualScreen.Height;
                result.Add(ScaleRectangle(rectangle, xScale, yScale));
            }

            return result;
        }

        private void ShowObjClassesAnalisysMenuItem_Click(object sender, EventArgs e)
        {
            //если классы есть
            if (DB.IsObjectClassesExist())
            {
                //включаем форму
                ObjectClassesDialog dialog = new ObjectClassesDialog(this.DB);
                dialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("Вы еще не добавили ни один класс объекта!", "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        //--------------Остальное--------------
        private void ResetImageMenuItem_Click(object sender, EventArgs e)
        {
            if (this.image.BackgroundImage != null)
            {
                this.image.BackgroundImage = null;
                this.imageNameList.SelectedIndex = -1;
                ImageNameList_SelectedIndexChanged(this, new EventArgs());
            }
        }

        private void DownloadImagesMenuItem_Click(object sender, EventArgs e)
        {
            //Настраиваем форму для выбора изображения
            FolderBrowserDialog selectFolderDialog = new FolderBrowserDialog
            {
                Description = "Выберите папку с изображениями, которые вы хотите загрузить. Программа загрузит все изображения, проигнорируя вложенные папки. Используя этот способ вы не можете задавать имена для изображений. Если имя изображения будет совпадать с другим, то оно не будет загружено."
            };

            if (selectFolderDialog.ShowDialog() == DialogResult.OK)
            {
                //заблокировать окно
                this.Enabled = false;

                try
                {
                    //Загрузить все изображения, пропуская те, чьи имена совпадают с именами уже загруженных изображений
                    DB.DownloadImages(selectFolderDialog.SelectedPath);

                    //обновление imageNameList
                    UpdateImageNameList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Упс!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //разблокировать окно
                this.Enabled = true;
            }
        }
    }
}
