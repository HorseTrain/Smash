using SmashLabs.Structs;
using SmashLabs.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Mesh
{
    public class MeshBuffer
    {
        public long size { get; set; }
        public long offset { get; set; }

        public static MeshBuffer[] ParseMeshBuffers(BufferReader reader)
        {
            BufferArrayPointer pointer = reader.ReadArrayPointer();

            MeshBuffer[] Out = new MeshBuffer[pointer.ElementCount];

            for (int i = 0; i < Out.Length; i++)
            {
                Out[i] = ParseMeshBuffer(reader);
            }

            pointer.End();

            return Out;
        }

        public static MeshBuffer ParseMeshBuffer(BufferReader reader)
        {
            MeshBuffer Out = new MeshBuffer();

            BufferArrayPointer pointer = reader.ReadArrayPointer();

            Out.size = pointer.ElementCount;
            Out.offset = pointer.AbsoluteOffset;

            pointer.End();

            return Out;
        }
    }
}
