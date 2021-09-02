using SmashLabs.IO.Parsables.Mesh.Rigging;
using SmashLabs.Structs;
using SmashLabs.Tools.Exporter;
using SmashLabs.Tools.Exporters;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SmashLabs.IO.Parsables.Mesh
{
    public unsafe class HSEM : IParsable
    {
        public string Name { get; set; } = "";
        MeshMetaData MeshData { get; set; }
        public int[] BufferSizes { get; set; } = new int[0];
        public long PolygonIndexSize { get; set; }
        public MeshBuffer[] VertexBuffers { get; set; }
        public MeshObject[] Objects { get; set; }
        public PolygonBuffer Polygonbuffer { get; set; }
        public MeshRiggingGroup[] RigBuffers { get; set; }
        public override void LoadData()
        {
            Name = reader.ReadStringOffset();

            MeshData = reader.ReadObject<MeshMetaData>();

            Objects = MeshObject.ReadObjects(reader);

            ReadBufferSizes();

            PolygonIndexSize = reader.ReadLong();

            VertexBuffers = MeshBuffer.ParseMeshBuffers(reader);

            Polygonbuffer = PolygonBuffer.ParsePolygonBuffer(reader);

            RigBuffers = MeshRiggingGroup.ParseMeshRiggingGroups(reader);
        }

        void ReadBufferSizes()
        {
            BufferArrayPointer pointer = reader.ReadArrayPointer();

            BufferSizes = reader.ReadObject<int>((int)pointer.ElementCount);

            pointer.End();
        }

        public override unsafe byte[] GetData()
        {
            ExportableBufferCollection Out = new ExportableBufferCollection();

            ByteBuffer Header = Out.AddBuffer();

            ByteBuffer MeshObjectBuffer = Out.AddBuffer();

            ByteBuffer AttributeArray = Out.AddBuffer();

            ByteBuffer MeshAttributeArray = Out.AddBuffer();

            ByteBuffer BufferSizes = Out.AddBuffer();

            ByteBuffer vertexbufferpointers = Out.AddBuffer();
            
            ByteBuffer PolygonData = Out.AddBuffer();

            ByteBuffer RigData = Out.AddBuffer();

            ByteBuffer RiggingData = Out.AddBuffer();

            ByteBuffer StringBuffer = Out.AddBuffer();

            Header.AddObject(this.Header);
            Header.AddObject(this.Magic);

            Out.AddStringWithPointer(Name,Header,StringBuffer);

            Header.AddObject(MeshData);

            Out.AddArrayPointer(Header, MeshObjectBuffer,Objects.Length);

            foreach (MeshObject Object in Objects)
            {
                Out.AddStringWithPointer(Object.Name,MeshObjectBuffer,StringBuffer);
                MeshObjectBuffer.AddObject(Object.SubIndex);
                Out.AddStringWithPointer(Object.ParentBoneName, MeshObjectBuffer, StringBuffer);
                MeshObjectBuffer.AddObject(Object.MeshData);

                Out.AddArrayPointer(MeshObjectBuffer,AttributeArray,Object.MeshAttributes.Length);

                foreach (MeshAttribute attribute in Object.MeshAttributes)
                {
                    AttributeArray.AddObject(attribute.Index);
                    AttributeArray.AddObject(attribute.Type);
                    AttributeArray.AddObject(attribute.BufferIndex);
                    AttributeArray.AddObject(attribute.BufferOffset);
                    AttributeArray.AddObject(attribute.Unk0);

                    Out.AddStringWithPointer(attribute.Name,AttributeArray,StringBuffer);

                    Out.AddArrayPointer(AttributeArray, MeshAttributeArray,attribute.Names.Length);

                    foreach (string name in attribute.Names)
                    {
                        Out.AddStringWithPointer(name,MeshAttributeArray,StringBuffer);
                    }
                }
            }

            {
                Out.AddArrayPointer(Header, BufferSizes, this.BufferSizes.Length);

                foreach (int i in this.BufferSizes)
                {
                    BufferSizes.AddObject(i);
                }
            }

            Header.AddObject(PolygonIndexSize);

            {
                Out.AddArrayPointer(Header,vertexbufferpointers,VertexBuffers.Length);

                foreach (MeshBuffer Buffer in VertexBuffers)
                {
                    ByteBuffer Meshbuffer = Out.AddBuffer();

                    Out.AddArrayPointer(vertexbufferpointers,Meshbuffer,(int)Buffer.size);

                    for (int i = 0; i < Buffer.size; i++)
                    {
                        Meshbuffer.Add(reader.Buffer[i + Buffer.offset]);
                    }
                }
            }

            {
                Out.AddArrayPointer(Header, PolygonData, (int)Polygonbuffer.size);

                for (int i = 0; i < Polygonbuffer.size; i++)
                {
                    PolygonData.Add(reader.Buffer[i + Polygonbuffer.offset]);
                }
            }

            Out.AddArrayPointer(Header,RigData,RigBuffers.Length);

            foreach (MeshRiggingGroup group in RigBuffers)
            {
                Out.AddStringWithPointer(group.Name,RigData,StringBuffer);
                RigData.AddObject(group.SubIndex);
                RigData.AddObject(group.Flags);

                ByteBuffer bonebuffers = Out.AddBuffer();

                Out.AddArrayPointer(RigData, bonebuffers,group.Buffers.Length);

                foreach (MeshBoneBuffer buffer in group.Buffers)
                {
                    Out.AddStringWithPointer(buffer.name,bonebuffers,StringBuffer);

                    Out.AddArrayPointer(bonebuffers, RiggingData, (int)buffer.Size);

                    for (int i = 0; i < buffer.Size; i++)
                    {
                        RiggingData.Add(reader.Buffer[i + buffer.offset]);
                    }
                }
            }

            return Out.FinalBuffer();
        }
    }
}
