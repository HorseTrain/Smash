using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Game.Asset
{
    public static class BinaryTools
    {
        [DllImport("msvcrt.dll")]
        public unsafe static extern IntPtr memcpy(void* dest, void* src, int count);

        public static unsafe T ConvertStructs<T, F>(F source) where T: unmanaged where F: unmanaged
        {
            Debug.Assert(sizeof(T) == sizeof(F));

            T Out = new T();

            byte* des = (byte*)&Out;
            byte* src = (byte*)&source;

            memcpy(des,src, sizeof(T));

            return Out;
        }
    }
}
