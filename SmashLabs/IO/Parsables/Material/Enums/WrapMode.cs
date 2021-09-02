using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Material.Enums
{
    public enum WrapMode : int
    {
        Repeat = 0,
        ClampToEdge = 1,
        MirroredRepeat = 2,
        ClampToBorder = 3
    }
}
