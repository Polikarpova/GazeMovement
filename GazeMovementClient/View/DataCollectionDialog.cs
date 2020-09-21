using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SysImage = System.Drawing.Image;

namespace GazeMovementClient.View
{
    public partial class DataCollectionDialog : Form
    {
        private IDatabase DB;

        //Сбор данных
        private IDataCollection DC;
        private int duration;
        private string sessionName;
        private List<int> imageIndices;

        //Для показа изображений
        int index = -1; //индекс индекса изображения в списке всех изображений, которые надо показать
        bool isWithPause;

        public DataCollectionDialog(IDatabase database, IDataCollection dataCollection, List<int> imageIndices, int durationTime, string sessionName, bool isWithPause)
        {
            //Инициализация компонентов
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;    //убрать рамку окна
            this.WindowState = FormWindowState.Maximized;   //на весь экран

            //Поля
            this.DB = database;
            this.DC = dataCollection;
            this.duration = durationTime;
            this.sessionName = sessionName;
            this.imageIndices = new List<int>(imageIndices);
            this.isWithPause = isWithPause;

            timer.Interval = durationTime * 1000;

            //скрываем курсор
            Cursor.Hide();

            //Настройка молуля сбора данных
            DC.SetSessionName(sessionName);
            DC.SetImagesPack(this.imageIndices);
        }

        private void DataCollectionDialog_Shown(object sender, EventArgs e)
        {
            //запускаем таймер
            timer.Start();

            //делаем один программный тик
            Timer_Tick(this, new EventArgs());
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            index++;

            //убираем изображение
            this.image.BackgroundImage = Properties.Resources.Black;

            //показываем курсор, так как будут появляться диалоги
            Cursor.Show();

            //останавливаем запись
            StopRecord();

            //если были показаны все изображения
            if (index == this.imageIndices.Count)
            {
                //спрашиваем про сохранение
                DialogResult result = MessageBox.Show("Время вышло!" + Environment.NewLine + "Хотите сохранить данные?", "Сохранить?", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);

                if (result == DialogResult.OK)
                {
                    DC.SaveData();
                    MessageBox.Show("Данные сохранены!");
                }

                this.Close();
            }
            else
            {
                //если это первое изображение или если выбрана функция показа изображений с паузой
                if (index == 0 || isWithPause)
                {
                    //Вывод информации
                    MessageBox.Show($"Когда вы нажмете на 'Ок', то начнут появляться изображения в течении {duration} секунд каждое.\nЕсли вы хотите прервать запись нажмите ESC после этого сообщения", "Hey!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }

                //Скрыть курсор
                Cursor.Hide();

                //Инициализация изображения
                InitImage(imageIndices[index]);

                //Начать запись
                StartRecord();
            }
        }

        private void InitImage(int imageIndex)
        {
            var imageBitmap = new Bitmap(DB.GetImage(imageIndex));
            this.image.BackgroundImage = imageBitmap;
        }

        private void StartRecord()
        {
            //включаем таймер
            timer.Start();

            //запускаем сбор данных
            DC.StartDataCollection(imageIndices[index]);
        }

        /// <summary>
        /// Обработка событий нажатия клавиш
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataCollectionDialog_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;
                StopRecord();
                Cursor.Show();
                this.Close();
            }
        }

        private void StopRecord()
        {
            timer.Stop();
            DC.DisableDataCollection();
        }
    }
}
