using SmashLabs.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Structs
{
    public class BufferArrayPointer
    {
        public long AbsoluteOffset { get; set; }
        public long ElementCount { get; set; }
        public long LastLocation { get; set; }
        public BufferReader reader{ get; set; }
        public void End()
        {
            reader.Seek(LastLocation);
        }
    }
}
