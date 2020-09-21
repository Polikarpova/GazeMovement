using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = System.Drawing.Point;

namespace GazeMovementClient
{
    public class ObjectClassInfo
    {
        public string ObjectClassName { get; set; }
        public string CommonPattern { get; set; }
        public Point GridSize { get; set; }

        public ObjectClassInfo(string objectClassName, string commonPattern, Point gridSize)
        {
            this.ObjectClassName = objectClassName;
            this.CommonPattern = commonPattern;
            this.GridSize = new Point(gridSize.X, gridSize.Y);
        }
    }
}
