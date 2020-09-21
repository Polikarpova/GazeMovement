using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Rectangle = System.Drawing.Rectangle;

namespace GazeMovementClient.Model
{
    class HighlightedObject
    {
        public ObjectClass ObjectClass { get; private set; }
        public Rectangle Rectangle { get; private set; }

        public HighlightedObject(ObjectClass objectClass, Rectangle rectangle)
        {
            this.ObjectClass = objectClass;
            this.Rectangle = rectangle;
        }
    }
}
