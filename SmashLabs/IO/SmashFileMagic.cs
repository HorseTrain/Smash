using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO
{
    public struct SmashFileMagic
    {
        public byte Magic0, Magic1, Magic2, Magic3;
        public short MajorVersion; //?
        public short MinorVersion; //?

        public override string ToString()
        {
            string Out = "";

            Out += (char)Magic0;
            Out += (char)Magic1;
            Out += (char)Magic2;
            Out += (char)Magic3;

            return Out;
        }

        public SmashFileMagic(string name,short major = 1, short minor = 0)
        {
            Magic0 = (byte)name[0];
            Magic1 = (byte)name[1];
            Magic2 = (byte)name[2];
            Magic3 = (byte)name[3];

            MajorVersion = major;
            MinorVersion = minor;
        }
    }
}
