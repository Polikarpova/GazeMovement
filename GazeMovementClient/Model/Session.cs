using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazeMovementClient.Model
{
    class Session
    {
        public string Name { get; private set; }
        public List<string> EyePathNames { get; private set; }

        public Session (string name, List<string> eyePathNames)
        {
            this.Name = name;
            this.EyePathNames = new List<string>(eyePathNames);
        }
    }
}
