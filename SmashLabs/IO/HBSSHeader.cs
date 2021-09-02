using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO
{
    public struct HBSSHeader
    {
        public byte Magic0, Magic1, Magic2, Magic3; //Always "HBSS"?
        public int Unk0;

        public long Unk1;

        public override string ToString()
        {
            string Out = "";

            Out += (char)Magic0;
            Out += (char)Magic1;
            Out += (char)Magic2;
            Out += (char)Magic3;

            return Out;
        }
    }
}
