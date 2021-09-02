using SmashLabs.IO.Parsables.Material.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Material
{
    public struct RasterizerState
    {
        public FillMode FillMode ;

        public CullMode CullMode ;

        public float DepthBias ;

        public float Unk4 ;

        public float Unk5 ;

        public int Unk6 ;

        public int Unk7 ;

        public float Unk8 ;
    }
}
