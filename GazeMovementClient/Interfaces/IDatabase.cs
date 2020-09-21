using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = System.Drawing.Rectangle;
using SysImage = System.Drawing.Image;
using GazeMovementClient.Model;
using System.Drawing;

namespace GazeMovementClient
{
    /// <summary>
    /// Интерфейс для взаимодействия с модулем базы данных
    /// </summary>
    public interface IDatabase
    {
        //изображения
        void CheckDownloadingImage(string imagePath);
        void CheckNewImageName(string imageName);
        void DownloadImage(string image, string imageName);
        void DownloadImages(string imagesFolderPath);
        List<string> GetImageNames();
        SysImage GetImage(int index);
        void DeleteImage(int index);
        int ImagesCount { get; }

        //выделенные области
        int IsHighlightedObjectsExist(int imageIndex);
        List<Rectangle> GetAllHighlightedObjectsAsRectangle(int imageIndex);
        List<RectangleF> GetAllHighlightedObjectsByObjectClassName(int imageIndex, string objectClassName);
        string GetHighlightedObjectClassObjectName(int imageIndex, int index);
        void SaveHighlightedObject(int imageIndex, string objectClassName, Rectangle rectangle);
        void DeleteHighlightedObjects(int indexImage);

        //классы объектов
        bool IsObjectClassesExist();
        List<string> GetObjectClassNames();
        List<ObjectClassInfo> GetObjectClassesInfo();
        void RewriteObjectClasses(List<ObjectClassInfo> newInformation);

        //данные о передвижении взгляда
        List<string> GetEyePathNames(int imageIndex);
        void DeleteEyePath(int imageIndex, int index);
        List<PointF> GetEyePath(int imageIndex, int index);
        int GetEyePathsCount(int imageIndex);
        int GetEyePathIndexByImageIndex(int imageIndex, int sessionIndex);

        //сессии
        List<string> GetSessionNames();
        List<int> GetImageIndices(int index);
        void SaveSession(Dictionary<int, List<PointF>> eyePaths, string sessionName);
        bool IsSessionExist(string name);
        void DeleteSession(int index);
    }
}
