using SmashLabs.Structs;
using SmashLabs.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Mesh.Rigging
{
    public class MeshRiggingGroup
    {
        public string Name { get; set; }
        public long SubIndex { get; set; }
        public long Flags { get; set; }
        public MeshBoneBuffer[] Buffers { get; set; }

        public static MeshRiggingGroup[] ParseMeshRiggingGroups(BufferReader reader)
        {
            BufferArrayPointer pointer = reader.ReadArrayPointer();

            MeshRiggingGroup[] Out = new MeshRiggingGroup[pointer.ElementCount];

            for (int i = 0; i < Out.Length; i++)
            {
                Out[i] = ParseMeshRiggingGroup(reader);
            }

            pointer.End();

            return Out;
        }

        public static MeshRiggingGroup ParseMeshRiggingGroup(BufferReader reader)
        {
            MeshRiggingGroup Out = new MeshRiggingGroup();

            Out.Name = reader.ReadStringOffset();
            Out.SubIndex = reader.ReadLong();
            Out.Flags = reader.ReadLong();
            Out.Buffers = MeshBoneBuffer.ReadMeshBoneBuffers(reader);

            return Out;
        }
    }
}
