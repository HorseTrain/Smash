using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Material.Enums
{
    public enum ParamDataType : ulong
    {
        Float = 0x1,
        Boolean = 0x2,
        Vector4 = 0x5,
        String = 0xB,
        Sampler = 0xE,
        UvTransform = 0x10,
        BlendState = 0x11,
        RasterizerState = 0x12,
    }
}
