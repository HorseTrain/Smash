using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Structs
{
    public struct Matrix3
    {
        public Vector3 Row0;
        public Vector3 Row1;
        public Vector3 Row2;

        public override string ToString()
        {
            return
                Row0.ToString() + "\n" +
                Row1.ToString() + "\n" +
                Row2.ToString();
        }
    }
}
