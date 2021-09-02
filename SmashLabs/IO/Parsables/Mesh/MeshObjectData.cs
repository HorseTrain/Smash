using SmashLabs.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Mesh
{
    public struct MeshObjectData
    {
        public int VertexCount;
        public int IndexCount;
        public uint Unk2;
        public int VertexOffset;
        public int VertexOffset2;
        public int FinalBufferOffset;
        public int BufferIndex;
        public int Stride;
        public int Stride2;
        public int Unk6;
        public int Unk7;
        public int ElementOffset;
        public int Unk8;
        public DrawElementType ElementType;
        public int HasRigging;
        public int Unk11;
        public int Unk12;
        public Vector3 BoundingSphereCenter;
        public float BoundingSphereRadius;
        public Vector3 BoundingBoxMin;
        public Vector3 BoundingBoxMax;
        public Vector3 OrientedBoundingBoxCenter;
        public Matrix3 OrientedBoundingBoxTransform;
        public Vector3 OrientedBoundingBoxSize;
    }
}
