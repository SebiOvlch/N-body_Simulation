using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_bodyPhysics.Core
{
    public class Body
    {
        // State Variables
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration {  get; set; }

        // Properties
        public float Mass { get; set; }
        public float Radius { get; set; }
    }
}
