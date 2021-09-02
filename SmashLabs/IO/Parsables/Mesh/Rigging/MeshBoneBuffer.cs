using SmashLabs.Structs;
using SmashLabs.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Mesh.Rigging
{
    public unsafe class MeshBoneBuffer
    {
        public string name { get; set; }

        public long offset { get; set; }
        public long Size { get; set; }

        public static MeshBoneBuffer[] ReadMeshBoneBuffers(BufferReader reader)
        {
            BufferArrayPointer pointer = reader.ReadArrayPointer();

            MeshBoneBuffer[] Out = new MeshBoneBuffer[pointer.ElementCount];

            for (int i = 0; i < Out.Length; i++)
            {
                Out[i] = ReadMeshBoneBuffer(reader);
            }

            pointer.End();

            return Out;
        }

        public static MeshBoneBuffer ReadMeshBoneBuffer(BufferReader reader)
        {
            MeshBoneBuffer Out = new MeshBoneBuffer();

            Out.name = reader.ReadStringOffset();

            BufferArrayPointer pointer = reader.ReadArrayPointer();

            Out.offset = pointer.AbsoluteOffset;
            Out.Size = pointer.ElementCount;

            //Console.WriteLine(Out.name);

            pointer.End();

            return Out;
        }
    }
}
