using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Tools.Exporter
{
    public class ArrayPointer
    {
        public int PointerLocationBufferIndex { get; set; } 
        public int PointerDataBufferIndex { get; set; }
        public int PointerLocation { get; set; }
        public int DataOffset { get; set; }
    }
}
