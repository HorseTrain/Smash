using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Tools.Exporter
{
    public unsafe class ByteBuffer : List<byte>
    {
        public void AddObject<T>(T Data)where T: unmanaged
        {
            T* temp = &Data;

            for (int i = 0; i < sizeof(T); i++)
            {
                Add(((byte*)temp)[i]);
            }
        }

        public void AddString(string str,bool pad = true)
        {
            int count = 0;

            foreach (char c in str)
            {
                Add((byte)c);

                count++;
            }

            if (pad)
            {
                AddPadding(4 - (count % 4));
            }
            else
            {
                AddPadding(1);
            }
        }

        public void AddPadding(int count,byte value = 0)
        {
            for (int i = 0; i < count; i++)
            {
                Add(value);
            }
        }
    }
}
