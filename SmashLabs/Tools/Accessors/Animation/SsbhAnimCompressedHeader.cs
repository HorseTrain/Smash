using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SmashLabs.Tools.Accessors.Animation
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SsbhAnimCompressedHeader
    {
        public ushort Unk_4; // always 4?
        public ushort Flags;
        public ushort DefaultDataOffset;
        public ushort BitsPerEntry;
        public int CompressedDataOffset;
        public int FrameCount;
    }

}
