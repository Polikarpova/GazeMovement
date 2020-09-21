using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazeMovementClient.Exceptions
{
    class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message)
        { }
    }
}
