using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = System.Drawing.Rectangle;
using PointF = System.Drawing.PointF;

namespace GazeMovementClient
{
    public interface IDataAnalysis
    {
        List<ObjectClassInfo> FindCommonPatternsInSessions(List<int> sessionIndices);
        List<List<PointF>> GetPointsFromHighlightedObjects(int imageIndex, int eyePathIndex);
    }
}
