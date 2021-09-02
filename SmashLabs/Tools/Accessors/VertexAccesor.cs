using SmashLabs.IO.Parsables.Mesh;
using SmashLabs.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Tools.Accessors
{
    public class VertexAccesor
    {
        public HSEM file { get;private set; }
        public VertexAccesor(HSEM file)
        {
            this.file = file;
        }

        public ushort[] ReadIndiciesShort(MeshObject Object)
        {
            file.reader.Seek(file.Polygonbuffer.offset + Object.MeshData.ElementOffset);

            return file.reader.ReadObject<ushort>(Object.MeshData.IndexCount);
        }

        public uint[] ReadIndicies32(MeshObject Object)
        {
            file.reader.Seek(file.Polygonbuffer.offset + Object.MeshData.ElementOffset);

            return file.reader.ReadObject<uint>(Object.MeshData.IndexCount);
        }

        public uint[] ReadIndicies(MeshObject Object)
        {
            switch (Object.MeshData.ElementType)
            {
                case DrawElementType.UnsignedShort: ushort[] temp = ReadIndiciesShort(Object); uint[] Out = new uint[temp.Length];  for (int i = 0; i < temp.Length; i++) Out[i] = temp[i]; return Out;
                case DrawElementType.UnsignedInt: return ReadIndicies32(Object);
            }

            return null;
        }

        public SmashVertex[] ReadVertexData(MeshObject Object)
        {
            SmashVertex[] Out = new SmashVertex[Object.MeshData.VertexCount];

            int map = 0;

            foreach (MeshAttribute attr in Object.MeshAttributes)
            {
                Vector4[] Data = ReadVertexData(attr,Object);

                for (int i = 0; i < Object.MeshData.VertexCount; i++)
                {
                    switch (attr.Name)
                    {
                        case "Position0": Out[i].VertexPosition = new Vector3(Data[i].X, Data[i].Y, Data[i].Z); break;
                        case "Normal0": Out[i].VertexNormal = new Vector3(Data[i].X, Data[i].Y, Data[i].Z); break;
                        case "map1": if (map == 0) Out[i].VertexMap0 = new Vector2(Data[i].X, 1-Data[i].Y); else Out[i].VertexMap1 = new Vector2(Data[i].X, Data[i].Y); break;
                        case "colorSet1": Out[i].VertexColor = new Vector4(Data[i].X/128.0f, Data[i].Y / 128.0f, Data[i].Z / 128.0f, Data[i].W / 128.0f); break; //Deviding by 128 is not important.
                    }
                }

                if (attr.Name == "map1")
                {
                    map++;
                }
            }

            return Out;
        }

        public Vector4[] ReadVertexData(MeshAttribute attr, MeshObject Object)
        {
            string attributeName = attr.Names[0];

            int offset = Object.MeshData.VertexOffset;
            int stride = Object.MeshData.Stride;
            if (attr.BufferIndex == 1)
            {
                offset = Object.MeshData.VertexOffset2;
                stride = Object.MeshData.Stride2;
            }

            int count = Object.MeshData.VertexCount;

            int attributeLength = GetAttributeSize(attributeName);

            return ReadVertexBuffer(file.VertexBuffers[attr.BufferIndex].offset,Object.MeshData.VertexCount, offset, attr.BufferOffset, stride, 0, attr.Type, attributeLength);
        }

        unsafe Vector4[] ReadVertexBuffer(long bufferindex,int count, int offset, int bufferoffset, int stride, int position, AttributeDataType type, int size)
        {
            Vector4[] Out = new Vector4[count];

            fixed (Vector4* temp = Out)
            {
                for (int i = 0; i < count; i++)
                {
                    file.reader.Seek(bufferindex + (offset + bufferoffset + stride * (position + i)));

                    float* v4 = (float*)&temp[i];

                    for (int v = 0; v < size; v++)
                    {
                        switch (type)
                        {
                            case AttributeDataType.Byte: v4[v] = file.reader.ReadByte(); break;
                            case AttributeDataType.HalfFloat: v4[v] = file.reader.ReadHalfFloat(); break;
                            case AttributeDataType.HalfFloat2: v4[v] = file.reader.ReadHalfFloat(); break;
                            case AttributeDataType.Float: v4[v] = file.reader.ReadSingle(); break;
                        }
                    }
                }
            }

            return Out;
        }

        public static int GetAttributeSize(string attributeName) //Maybe there is a check in the file?
        {
            int attributeSize = 3;
            if (attributeName.Contains("colorSet") || attributeName.Equals("Normal0") || attributeName.Equals("Tangent0"))
                attributeSize = 4;
            if (attributeName.Equals("map1") || attributeName.Equals("bake1") || attributeName.Contains("uvSet"))
                attributeSize = 2;
            return attributeSize;
        }
    }
}
