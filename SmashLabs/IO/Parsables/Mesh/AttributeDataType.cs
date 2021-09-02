using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Mesh
{
    public enum AttributeDataType : uint
    {
        Float = 0u,
        Byte = 2u,
        HalfFloat = 5u,
        HalfFloat2 = 8u,
    }
}
