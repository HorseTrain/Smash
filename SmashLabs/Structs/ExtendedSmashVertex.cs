using SmashLabs.IO.Parsables.Mesh;
using SmashLabs.IO.Parsables.Skeleton;
using SmashLabs.Tools.Accessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Structs
{
    public struct ExtendedSmashVertex
    {
        public SmashVertex VertexData;
        public VertexRig RigData;

        public override string ToString()
        {
            return VertexData.ToString() + RigData.ToString();
        }

        public static ExtendedSmashVertex[] ReadFullVerticies(VertexAccesor accesor,RigAccessor raccessor,LEKS Skeleton,MeshObject Object,out bool Rigged)
        {
            ExtendedSmashVertex[] Out = new ExtendedSmashVertex[Object.MeshData.VertexCount];

            SmashVertex[] verticies = accesor.ReadVertexData(Object);
            VertexRig[] rigdata = raccessor.ReadVertexWeightData(Object,Skeleton.BoneDic,out Rigged);

            for (int i = 0; i < Out.Length; i++)
            {
                Out[i].VertexData = verticies[i];
                Out[i].RigData = rigdata[i];
            }

            return Out;
        }
    }
}
