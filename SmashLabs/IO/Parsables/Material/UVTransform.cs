using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Material
{
    public struct UVTransform
    {
        public float X ;
        public float Y ;
        public float Z ;
        public float W ;
        public float V;
        public override string ToString()
        {
            return $"({X}, {Y}, {Z}, {W}, {V})";
        }
    }
}
