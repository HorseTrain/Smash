using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Animation.Enums
{
    public enum AnimTrackFlags
    {
        Transform = 0x0001,
        Texture = 0x0002,
        Float = 0x0003,
        PatternIndex = 0x0005,
        Boolean = 0x0008,
        Vector4 = 0x0009,

        Direct = 0x0100,
        ConstTransform = 0x0200,
        Compressed = 0x0400,
        Constant = 0x0500,
    }
}
