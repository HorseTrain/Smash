using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Structs
{
    public struct Vector4
    {
        public float X, Y, Z, W;

        public Vector4(float X = 0, float Y = 0, float Z = 0, float W = 0)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }

        public override string ToString()
        {
            return "V: (" + X + ", " + Y + ", " + Z + "), W: " + W;
        }
    }
}
