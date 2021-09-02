using SmashLabs.Structs;
using SmashLabs.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Mesh
{
    public unsafe class MeshObject
    {  
        public string Name { get; set; }
        public long SubIndex { get; set; }
        public string ParentBoneName { get; set; }
        public MeshObjectData MeshData;
        public MeshAttribute[] MeshAttributes { get; set; }
        public static MeshObject[] ReadObjects(BufferReader reader)
        {
            BufferArrayPointer MeshObjects = reader.ReadArrayPointer();

            MeshObject[] Out = new MeshObject[MeshObjects.ElementCount];

            for (int i = 0; i < Out.Length; i++)
            {
                Out[i] = ReadObject(reader);
            }

            MeshObjects.End();

            return Out;
        }

        public static MeshObject ReadObject(BufferReader reader)
        {
            MeshObject Out = new MeshObject();

            Out.Name = reader.ReadStringOffset();
            Out.SubIndex = reader.ReadLong();
            Out.ParentBoneName = reader.ReadStringOffset();
            Out.MeshData = reader.ReadObject<MeshObjectData>();
            Out.MeshAttributes = MeshAttribute.ParseMeshAttributes(reader);

            //208

            return Out;
        }
    }
}
