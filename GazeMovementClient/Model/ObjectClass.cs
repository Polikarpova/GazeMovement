using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;

namespace GazeMovementClient.Model
{
    class ObjectClass
    {
        public string Name { get; private set; }
        public string CommonPattern { get; private set; }
        public Point GridSize { get; private set; }

        public ObjectClass(string name)
        {
            this.Name = name;
            this.CommonPattern = "";
        }

        [JsonConstructor]
        public ObjectClass(string name, string commonPattern, Point gridSize)
        {
            this.Name = name;
            this.CommonPattern = commonPattern;
            this.GridSize = new Point(gridSize.X, gridSize.Y);
        }

        public void SetFields(string commonPattern, Point gridSize)
        {
            this.CommonPattern = commonPattern;
            this.GridSize = new Point(gridSize.X, gridSize.Y);
        }
    }
}
