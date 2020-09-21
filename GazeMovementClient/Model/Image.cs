using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileInfo = System.IO.FileInfo;
using SysImage = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;
using System.Drawing;

namespace GazeMovementClient.Model
{
    class Image
    {
        
        public FileInfo ImageInfo { get; private set; }
        public string Name => ImageInfo.Name;
        public string Path => ImageInfo.FullName;

        public List<HighlightedObject> HighlightedObjects { get; private set; }
        public List<EyePath> EyePaths { get; private set; }

        public Image(FileInfo imageInfo)
        {
            this.ImageInfo = imageInfo;
            this.HighlightedObjects = new List<HighlightedObject>();
            this.EyePaths = new List<EyePath>();
        }

        public SysImage GetImage()
        {
            SysImage result;
            using (var imgStream = System.IO.File.OpenRead(ImageInfo.FullName))
            {
                result = SysImage.FromStream(imgStream);
            }
            return result;
        }

        public List<Rectangle> GetRectangles()
        {
            List<Rectangle> rectangles = new List<Rectangle>();

            foreach (HighlightedObject highlightedObject in HighlightedObjects)
            {
                rectangles.Add(highlightedObject.Rectangle);
            }

            return rectangles;
        }

        public void AddHighlightedObject(ObjectClass objectClass, Rectangle rectangle)
        {
            this.HighlightedObjects.Add(new HighlightedObject(objectClass, rectangle));
        }

        public string AddEyePath(List<PointF> points, string sessionName)
        {
            //формируем имя
            string name = System.IO.Path.GetFileNameWithoutExtension(Name) +
                          "_" + sessionName + "_" + DateTime.Now.ToString("dd-MM-yyyy::HH-mm-ss");

            //добавляем новый EyePath
            this.EyePaths.Add(new EyePath(name, points));

            return name;
        }

        public List<string> GetEyePathNames()
        {
            List<string> result = new List<string>();

            foreach (EyePath eyePath in EyePaths)
            {
                result.Add(eyePath.Name);
            }

            return result;
        }

        public string GetEyePathNameByIndex(int index)
        {
            return EyePaths[index].Name;
        }

        public void DeleteEyePath(int index)
        {
            EyePaths.RemoveAt(index);
        }

        public void DeleteEyePathByName(string name)
        {
            int i = -1;
            foreach(EyePath eyePath in EyePaths)
            {
                i++;
                if (eyePath.Name == name)
                {
                    break;
                }
            }

            DeleteEyePath(i);
        }
    }
}
