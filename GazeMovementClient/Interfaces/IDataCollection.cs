using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazeMovementClient
{
    public interface IDataCollection
    {
        void SetSessionName(string name);
        void SetImagesPack(List<int> imagesIndices);
        void StartDataCollection(int imageIndex);
        void DisableDataCollection();
        void SaveData();
    }
}
