using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Structs
{
    public struct Vector4I
    {
        public int X, Y, Z, W;

        public Vector4I(int X = 0, int Y = 0, int Z = 0, int W = 0)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }

        public override string ToString()
        {
            return "(" + X + "," + Y + "," + Z + "," + W + ")";
        }
    }
}
