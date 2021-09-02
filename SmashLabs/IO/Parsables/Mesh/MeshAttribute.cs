using SmashLabs.Structs;
using SmashLabs.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Mesh
{
    public class MeshAttribute
    {
        public int Index { get; set; }
        public AttributeDataType Type { get; set; }
        public int BufferIndex { get; set; }
        public int BufferOffset { get; set; }
        public long Unk0 { get; set; } //2 ints?
        public string Name { get; set; }

        public string[] Names { get; set; }

        public static MeshAttribute[] ParseMeshAttributes(BufferReader reader)
        {
            BufferArrayPointer AttribPointer = reader.ReadArrayPointer();

            MeshAttribute[] Out = new MeshAttribute[AttribPointer.ElementCount];

            for (int i = 0; i < Out.Length; i++)
            {
                Out[i] = ParseMeshAttribute(reader);
            }

            AttribPointer.End();

            return Out;
        }

        public static MeshAttribute ParseMeshAttribute(BufferReader reader)
        {
            MeshAttribute Out = new MeshAttribute();

            Out.Index = reader.ReadInt();
            Out.Type = reader.ReadObject<AttributeDataType>();
            Out.BufferIndex = reader.ReadInt();
            Out.BufferOffset = reader.ReadInt();
            Out.Unk0 = reader.ReadLong();
            Out.Name = reader.ReadStringOffset();

            Out.Names = reader.ReadStringArray();

            //48

            return Out;
        }
    }
}
