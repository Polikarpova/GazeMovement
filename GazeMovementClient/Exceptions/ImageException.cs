using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazeMovementClient.Exceptions
{
    class ImageException : Exception
    {
        public ImageException(string message) : base(message)
        { }
    }
}
