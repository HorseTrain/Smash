using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Tools
{
    public unsafe class Memory
    {
        public static void MemCoppy(void* home,void* location, long size)
        {
            for (int i = 0; i < size; i++)
            {
                ((byte*)location)[i] = ((byte*)home)[i];
            }
        }
    }
}
