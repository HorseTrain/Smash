using SmashLabs.Structs;
using SmashLabs.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Mesh
{
    public class PolygonBuffer
    {
        public long size { get; set; }
        public long offset { get; private set; }

        public static PolygonBuffer ParsePolygonBuffer(BufferReader reader)
        {
            BufferArrayPointer pointer = reader.ReadArrayPointer();

            PolygonBuffer Out = new PolygonBuffer();

            Out.size = pointer.ElementCount;
            Out.offset = pointer.AbsoluteOffset;

            pointer.End();

            return Out;
        }
    }
}
