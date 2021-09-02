using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Structs
{
    public struct Vector3
    {
        public float X, Y, Z;

        public Vector3(float X, float Y,float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }
    }
}
