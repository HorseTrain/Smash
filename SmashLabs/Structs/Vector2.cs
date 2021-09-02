using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Structs
{
    public struct Vector2
    {
        public float X, Y;

        public Vector2(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }
    }
}
