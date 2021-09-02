using SmashLabs.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Mesh
{
    public struct MeshMetaData
    {
        public Vector3 BoundingSphereLocation;
        public float BoundingSphereRadius;
        public Vector3 MinBoundingBox;
        public Vector3 MaxBoundingBox;
        public Vector3 OBBCenter;
        public Matrix3 Matrix;
        public Vector3 OBBSize;
        public float Unk0; //?
    }
}
