using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Structs
{
    public struct SmashVertex
    {
        public Vector3 VertexPosition;
        public Vector3 VertexNormal;
        public Vector2 VertexMap0;
        public Vector2 VertexMap1;
        public Vector4 VertexColor;

        public override string ToString()
        {
            return VertexPosition.ToString() + "\n" + VertexNormal.ToString() + "\n" + VertexMap0.ToString() + "\n" + VertexMap1.ToString() + "\n" + VertexColor.ToString() + "\n";
        }
    }
}
