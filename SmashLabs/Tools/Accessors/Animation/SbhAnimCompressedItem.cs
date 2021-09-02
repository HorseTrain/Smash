using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SmashLabs.Tools.Accessors.Animation
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SbhAnimCompressedItem
    {
        public float Start;
        public float End;
        public ulong Count;
    }
}
