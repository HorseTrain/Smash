using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Animation.Enums
{
    public enum AnimType : ulong
    {
        Transform = 1Lu,
        Visibility = 2Lu,
        Material = 4Lu,
        Camera = 5Lu
    }
}
