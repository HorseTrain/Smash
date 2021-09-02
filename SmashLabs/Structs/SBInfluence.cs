using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Structs
{
    public class SBInfluence
    {
        public string BoneName;
        public int VertexIndex;
        public float VertexWeight;

        public override string ToString()
        {
            return "{" + BoneName + "," + VertexIndex + "," + VertexWeight + "}";
        }
    }
}
