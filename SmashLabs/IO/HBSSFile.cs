using SmashLabs.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO
{
    public unsafe class HBSSFile
    {
        public byte[] Buffer { get; set; }

        private byte* datapointer
        {
            get
            {
                fixed (byte* temp = Buffer)
                {
                    return temp;
                }
            }
        }

        public HBSSHeader Header => *(HBSSHeader*)datapointer; //Maybe implament with buffer reader for consistency? 

        private HBSSFile()
        {

        }

        public static HBSSFile TryParseFile(byte[] Data)
        {
            HBSSFile Out = new HBSSFile();

            Out.Buffer = Data;

            if (Out.Header.ToString() != "HBSS")
                ErrorHandling.ThrowError("Input file not valid HBSS");

            return Out;
        }

        public static HBSSFile TryParseFile(string path)
        {
            return TryParseFile(System.IO.File.ReadAllBytes(path));
        }
    }
}
