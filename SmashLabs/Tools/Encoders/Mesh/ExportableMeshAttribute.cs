using SmashLabs.IO.Parsables.Mesh;
using SmashLabs.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Tools.Encoders.Mesh
{
    public class ExportableMeshAttribute 
    {
        public int Index { get; set; }
        public AttributeDataType Type { get; set; } = AttributeDataType.HalfFloat;
        public int BufferIndex { get; set; }
        public int BufferOffset { get; set; }
        public long Unk0 { get; set; }
        public string Name { get; set; }
        public Vector4[] Data { get; set; }
        public string[] Names { get; set; }
    }
}
