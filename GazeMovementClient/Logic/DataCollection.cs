using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobii.Interaction;
using System.Drawing;

namespace GazeMovementClient.Logic
{
    class DataCollection : IDataCollection
    {
        //База данных
        private IDatabase DB;

        private Host host;
        private GazePointDataStream gazePointDataStream;

        public bool IsRecording { get; private set; }

        private string sessionName;
        private Dictionary<int, List<PointF>> Points;
        private int currentIndex = -1;

        public DataCollection(IDatabase database)
        {
            DB = database;
            host = new Host();
            Points = new Dictionary<int, List<PointF>>();
            IsRecording = false;
        }

        public void SetSessionName(string name)
        {
            this.sessionName = name;
        }

        public void SetImagesPack(List<int> imagesIndices)
        {
            currentIndex = -1;

            Points.Clear();

            foreach(int index in imagesIndices)
            {
                Points.Add(index, new List<PointF>());
            }
        }

        public void StartDataCollection(int imageIndex)
        {
            IsRecording = true;
            currentIndex = imageIndex;

            gazePointDataStream = host.Streams.CreateGazePointDataStream();
            host.EnableConnection();
            gazePointDataStream.GazePoint(RecordGazePointToList);
        }

        private void RecordGazePointToList(double x, double y, double ts)
        {
            PointF point = new PointF((float)x, (float)y);
            Points[currentIndex].Add(point);
        }

        /// <summary>
        /// Отключить сбор данных с айтрекера
        /// </summary>
        public void DisableDataCollection()
        {
            if (IsRecording)
            {
                host.DisableConnection();
                IsRecording = false;
            }
        }

        /// <summary>
        /// Сохранить собранные данные
        /// </summary>
        /// <param name="imageIndex"></param>
        public void SaveData()
        {
            //Сохранить
            DB.SaveSession(Points, sessionName);
        }
    }
}
