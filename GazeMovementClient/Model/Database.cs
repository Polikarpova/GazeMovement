using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using File = System.IO.File;
using Directory = System.IO.Directory;
using Path = System.IO.Path;
using SysImage = System.Drawing.Image;
using FileInfo = System.IO.FileInfo;
using Rectangle = System.Drawing.Rectangle;
using GazeMovementClient.Exceptions;
using System.Drawing;

namespace GazeMovementClient.Model
{
    class Database : IDatabase
    {
        private List<Image> Images { get; set; }
        private List<ObjectClass> ObjectClasses { get; set; }
        private List<Session> Sessions { get; set; }

        private static readonly string dataPath = Path.Combine(Environment.CurrentDirectory, "data");
        private static readonly string imagesPath = Path.Combine(dataPath, "images");
        private static readonly string imagesFile = Path.Combine(dataPath, "imagesData.json");
        private static readonly string objectClassesFile = Path.Combine(dataPath, "objectClassesData.json");
        private static readonly string sessionsFile = Path.Combine(dataPath, "sessionsData.json");

        public int ImagesCount => Images.Count;

        public Database()
        {
            //проверка наличия папки с БД (data и data/images)
            CheckFolder();

            //если при проверке JSON-файлов не было ошибок
            CheckJSONFiles();
        }

        /// <summary>
        /// Проверяет наличие папок data и data/images, в которых лежит БД и изображение
        /// </summary>
        private void CheckFolder()
        {
            if ((!Directory.Exists(dataPath)) || (Directory.Exists(dataPath) && !Directory.Exists(imagesPath)))
            {
                Directory.CreateDirectory(imagesPath);
            }
        }

        /// <summary>
        /// Проверяет наличие файлов с данными БД
        /// </summary>
        /// <param name="dataPath">
        /// Путь до папки, где должеы лежать искомые файлы
        /// </param>
        /// <param name="isFilesEmpty">
        /// true - если оба файла пустые, иначе false
        /// </param>
        private void CheckJSONFiles()
        {
            if (File.Exists(imagesFile) && File.Exists(objectClassesFile) && File.Exists(sessionsFile))
            {
                //считываем из файликов данные
                Images = JsonConvert.DeserializeObject<List<Image>>(File.ReadAllText(imagesFile));
                ObjectClasses = JsonConvert.DeserializeObject<List<ObjectClass>>(File.ReadAllText(objectClassesFile));
                Sessions = JsonConvert.DeserializeObject<List<Session>>(File.ReadAllText(sessionsFile));

                if (Images == null && (Sessions != null))
                {
                    throw new DatabaseException("Файл imagesData.json пуст!");
                }
                else if (Images == null && ObjectClasses == null)
                {
                    Images = new List<Image>();
                    ObjectClasses = new List<ObjectClass>();
                }
                else if (Images != null && ObjectClasses == null)
                {
                    ObjectClasses = new List<ObjectClass>();
                }

                if (Sessions == null)
                {
                    Sessions = new List<Session>();
                }
            }
            else
            {
                //создаем недостающие файлы, оставляя их пустыми (!)

                if (!File.Exists(imagesFile))
                {
                    using (File.Create(imagesFile))
                    { }

                    Images = new List<Image>();
                }

                if (!File.Exists(objectClassesFile))
                {
                    using (File.Create(objectClassesFile))
                    { }

                    ObjectClasses = new List<ObjectClass>();
                }

                if (!File.Exists(sessionsFile))
                {
                    using (File.Create(sessionsFile))
                    { }

                    Sessions = new List<Session>();
                }
            }
        }

        private void WriteToJSON()
        {
            File.WriteAllText(imagesFile, JsonConvert.SerializeObject(Images));
            File.WriteAllText(objectClassesFile, JsonConvert.SerializeObject(ObjectClasses));
            File.WriteAllText(sessionsFile, JsonConvert.SerializeObject(Sessions));
        }

        public static void RemakeDatabase()
        {
            //Удалаяем все файлы из папки data/image
            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(imagesPath);
            foreach (FileInfo file in info.GetFiles())
            {
                file.Delete();
            }

            //Удаляем JSON-файлы
            File.Delete(imagesFile);
            File.Delete(objectClassesFile);
            File.Delete(sessionsFile);
        }

        /// <summary>
        /// Проверка изображения на допустимость к загрузке
        /// </summary>
        /// <param name="imagePath"></param>
        public void CheckDownloadingImage(string imagePath)
        {
            //Проверка загружаемого изображения
            SysImage image;
            using (var imgStream = File.OpenRead(imagePath))
            {
                image = SysImage.FromStream(imgStream);
            }

            if (image.Height < 700 || image.Width < 1270 || image.Height > image.Width)
            {
                throw new ImageException("Принимаются изображения с размерами не меньше чем 1270 пикселей в ширину и 700 пикселей в высоту.");
            }
        }

        public void CheckNewImageName(string imageName)
        {
            //Проверка на имя
            if (IsImageNameExist(imageName))
            {
                throw new ImageException("Изображение с таким именем уже существует!");
            }
        }

        private bool IsImageNameExist(string imageName)
        {
            string[] images = Directory.GetFiles(imagesPath);

            foreach (string name in images)
            {
                string currentImage = Path.GetFileNameWithoutExtension(name);
                if (currentImage == imageName)
                {
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// Добавление изображения в БД
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageName"></param>
        public void DownloadImage(string image, string imageName)
        {
            string name = imageName;

            //Если нет расширения, то добавить
            FileInfo info = new FileInfo(image);
            if (!name.Contains(info.Extension))
            {
                name = imageName + info.Extension;
            }

            //Копируем изображение в папку images
            string imagePath = Path.Combine(imagesPath, name);
            File.Copy(image, imagePath);

            //Добавляем изображение в БД
            AddNewImage(new FileInfo(imagePath));
        }

        private void AddNewImage(FileInfo imageInfo)
        {
            //добавляем файл
            this.Images.Add(new Image(imageInfo));

            //записываем изменения в файл
            WriteToJSON();
        }

        public void DownloadImages(string imagesFolderPath)
        {
            string[] files = Directory.GetFiles(imagesFolderPath);
            List<string> imageExtensions = new List<string> { ".jpeg", ".jpg", ".png" };

            foreach (string fileName in files)
            {
                FileInfo imageInfo = new FileInfo(fileName);

                //если это изображение и если такое имя ещё не занято
                if ( imageExtensions.Contains(Path.GetExtension(fileName).ToLower()) && !IsImageNameExist(Path.GetFileNameWithoutExtension(imageInfo.Name)) )
                {
                    DownloadImage(fileName, Path.GetFileNameWithoutExtension(imageInfo.Name));
                }
            }
        }

        public List<string> GetImageNames()
        {
            List<string> result = new List<string>();

            foreach (Image image in Images)
            {
                result.Add(image.Name);
            }

            return result;
        }

        public SysImage GetImage(int index)
        {
            return Images[index].GetImage();
        }

        public void DeleteImage(int index)
        {
            //Удаляем само изображение
            File.Delete(Images[index].Path);

            //Удаляем пути взгляда этого изображения
            for (int i = 0; i < Images[index].EyePaths.Count; i++)
            {
                DeleteEyePath(index, i);
            }

            //Удаляем запись об изображении
            Images.Remove(Images[index]);
            
            //записываем изменения в файл
            WriteToJSON();
        }

        public bool IsObjectClassesExist()
        {
            return this.ObjectClasses.Count > 0 ? true : false;
        }

        public List<string> GetObjectClassNames()
        {
            List<string> result = new List<string>();

            foreach (ObjectClass objectClass in ObjectClasses)
            {
                result.Add(objectClass.Name);
            }

            return result;
        }

        public List<ObjectClassInfo> GetObjectClassesInfo()
        {
            List<ObjectClassInfo> result = new List<ObjectClassInfo>();

            foreach (ObjectClass objectClass in ObjectClasses)
            {
                result.Add(new ObjectClassInfo(objectClass.Name, objectClass.CommonPattern, objectClass.GridSize));
            }

            return result;
        }

        public void RewriteObjectClasses(List<ObjectClassInfo> newInformation)
        {
            foreach (ObjectClassInfo info in newInformation)
            {
                GetObjectClass(info.ObjectClassName).SetFields(info.CommonPattern, info.GridSize);
            }

            WriteToJSON();
        }

        public int IsHighlightedObjectsExist(int imageIndex)
        {
            return Images[imageIndex].HighlightedObjects.Count;
        }

        public List<Rectangle> GetAllHighlightedObjectsAsRectangle(int imageIndex)
        {
            return Images[imageIndex].GetRectangles();
        }

        public List<RectangleF> GetAllHighlightedObjectsByObjectClassName(int imageIndex, string objectClassName)
        {
            List<RectangleF> result = new List<RectangleF>();

            foreach(HighlightedObject highlightedObject in Images[imageIndex].HighlightedObjects)
            {
                if ( highlightedObject.ObjectClass.Name == objectClassName)
                {
                    result.Add( new RectangleF(highlightedObject.Rectangle.Location, highlightedObject.Rectangle.Size) );
                }
            }

            return result;
        }

        public string GetHighlightedObjectClassObjectName(int imageIndex, int index)
        {
            return Images[imageIndex].HighlightedObjects[index].ObjectClass.Name;
        }

        public void SaveHighlightedObject(int imageIndex, string objectClassName, Rectangle rectangle)
        {
            Images[imageIndex].AddHighlightedObject(GetObjectClass(objectClassName), rectangle);
            WriteToJSON();
        }

        private ObjectClass GetObjectClass(string objectClassName)
        {
            //если такой есть, то возвращаем его
            foreach(ObjectClass oc in ObjectClasses)
            {
                if (oc.Name == objectClassName)
                {
                    return oc;
                }
            }

            //иначе, добавляем и возвращаем добавленный
            ObjectClasses.Add(new ObjectClass(objectClassName));
            return ObjectClasses[ObjectClasses.Count - 1];
        }

        public void DeleteHighlightedObjects(int indexImage)
        {
            //удаляем
            Images[indexImage].HighlightedObjects.Clear();

            //записываем изменения в файл
            WriteToJSON();
        }

        public List<string> GetEyePathNames(int imageIndex)
        {
            return Images[imageIndex].GetEyePathNames();
        }

        public void DeleteEyePath(int imageIndex, int index)
        {
            //узнаем к какой сессии он относится
            string sessionName =  Images[imageIndex].GetEyePathNameByIndex(index).Split('_').ElementAt(1);

            //если у этой сессии остался один путь взгляда, то удалить и её
            int sessionIndex = GetSessionIndexByName(sessionName);
            if (Sessions[sessionIndex].EyePathNames.Count == 1)
            {
                DeleteSession(sessionIndex);
            }
            else
            {
                string name = Images[imageIndex].EyePaths[index].Name;

                //просто удаляем путь
                Images[imageIndex].DeleteEyePath(index);

                //удалить этот путь из списка сессии
                Sessions[sessionIndex].EyePathNames.Remove(name);
            }

            //записываем изменения в файл
            WriteToJSON();
        }

        private int GetSessionIndexByName(string name)
        {
            int i = 0;
            foreach (Session session in Sessions)
            {
                if (session.Name == name)
                {
                    break;
                }

                i++;
            }

            return i;
        }

        public List<PointF> GetEyePath(int imageIndex, int index)
        {
            return Images[imageIndex].EyePaths[index].Points;
        }

        public int GetEyePathsCount(int imageIndex)
        {
            return Images[imageIndex].EyePaths.Count;
        }

        public List<string> GetSessionNames()
        {
            List<string> result = new List<string>();

            foreach (Session session in Sessions)
            {
                result.Add(session.Name);
            }

            return result;
        }

        public List<int> GetImageIndices(int index)
        {
            List<int> result = new List<int>();

            foreach (string eyePathName in Sessions[index].EyePathNames)
            {
                result.Add(GetImageIndexByName(eyePathName.Split('_').First()));
            }

            return result;
        }

        public void SaveSession(Dictionary<int, List<PointF>> eyePaths, string sessionName)
        {
            List<string> eyePathNames = new List<string>();

            foreach (KeyValuePair<int, List<PointF>> pair in eyePaths)
            {
                eyePathNames.Add(Images[pair.Key].AddEyePath(pair.Value, sessionName));
            }

            Sessions.Add(new Session(sessionName, eyePathNames));

            //записываем в файлик
            WriteToJSON();
        }

        public bool IsSessionExist(string name)
        {
            bool isExist = false;

            foreach(Session session in Sessions)
            {
                if (session.Name == name)
                {
                    isExist = true;
                    break;
                }
            }

            return isExist;
        }

        public void DeleteSession(int index)
        {
            //удалить все пути взгляда этой сессии
            foreach(string name in Sessions[index].EyePathNames)
            {
                //получить имя изображения
                string imageName = name.Split('_').First();

                //удалить путь у изображения
                Images[GetImageIndexByName(imageName)].DeleteEyePathByName(name);
            }

            //удалить сессию
            Sessions.RemoveAt(index);

            //записать в файлик
            WriteToJSON();
        }

        private int GetImageIndexByName(string name)
        {
            int i = 0;
            foreach (Image image in Images)
            {
                if (Path.GetFileNameWithoutExtension(image.Name) == name)
                {
                    break;
                }

                i++;
            }

            return i;
        }

        public int GetEyePathIndexByImageIndex(int imageIndex, int sessionIndex)
        {
            //получить имя сессии по индексу
            string sessionName = "_" + Sessions[sessionIndex].Name + "_";

            int index = -1;
            foreach (EyePath eyePath in Images[imageIndex].EyePaths)
            {
                index++;

                if (eyePath.Name.Contains(sessionName))
                {
                    break;
                }
            }

            return index;
        }
    }
}
