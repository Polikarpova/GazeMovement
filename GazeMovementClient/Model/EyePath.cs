using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GazeMovementClient.Model
{
    class EyePath
    {
        public string Name { get; private set; }
        public List<PointF> Points { get; private set; }
        public string ImageName => Name.Split('_').First();

        public EyePath(string name, List<PointF> points)
        {
            Name = name;
            Points = new List<PointF>(points);
        }
    }
}
