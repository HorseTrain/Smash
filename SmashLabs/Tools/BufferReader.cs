using SmashLabs.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Tools
{
    public unsafe class BufferReader
    {
        public byte[] Buffer { get; private set; }

        public long BufferLocation { get; private set; }
        public long BufferEnd => Buffer.Length;
        public void Seek(long location)
        {
            BufferLocation = location;
        }

        public void Advance(long size)
        {
            BufferLocation += size;
        }

        public BufferReader(byte[] buffer)
        {
            Buffer = buffer;
        }

        public T ReadObject<T>() where T: unmanaged
        {
            fixed (byte* temp = Buffer)
            {
                T Out = *(T*)&temp[BufferLocation];

                BufferLocation += sizeof(T);

                return Out;
            }
        }

        public T[] ReadObject<T>(int size) where T: unmanaged
        {
            T[] Out = new T[size];

            for (int i = 0; i < size; i++)
            {
                Out[i] = ReadObject<T>();
            }

            return Out;
        }

        public T ReadObjectStatic<T>(int offset) where T: unmanaged
        {
            long LastLocation = BufferLocation;

            Seek(offset);

            T Out = ReadObject<T>();

            Seek(LastLocation);

            return Out;
        }

        public byte ReadByte()
        {
            return ReadObject<byte>();
        }

        public short ReadShort()
        {
            return ReadObject<short>();
        }

        public int ReadInt()
        {
            return ReadObject<int>();
        }

        public long ReadLong()
        {
            return ReadObject<long>();
        }

        public float ReadSingle()
        {
            return ReadObject<float>();
        }

        public long ReadOffset()
        {
            return BufferLocation + ReadLong();
        }

        public string ReadStringOffset()
        {
            string Out = "";

            long location = ReadOffset();

            long chec = BufferLocation;

            Seek(location);

            Out = ReadString();

            Seek(chec);

            return Out;
        }

        public string ReadString()
        {
            string Out = "";

            while (true)
            {
                byte read = ReadByte();

                if (read == 0)
                    break;
                else
                    Out += (char)read;
            }

            return Out;
        }

        public BufferArrayPointer ReadArrayPointer()
        {
            BufferArrayPointer Out = new BufferArrayPointer();

            Out.AbsoluteOffset = ReadOffset();
            Out.ElementCount = ReadLong();
            Out.LastLocation = BufferLocation;

            Out.reader = this;

            Seek(Out.AbsoluteOffset);

            return Out;
        }

        public string[] ReadStringArray()
        {
            BufferArrayPointer pointer = ReadArrayPointer();

            string[] Out = new string[pointer.ElementCount];

            for (int i = 0; i < Out.Length; i++)
            {
                Out[i] = ReadStringOffset();
            }

            pointer.End();

            return Out;
        }

        //Thanks ploaj :)
        public float ReadHalfFloat()
        {
            int hbits = ReadShort();

            int mant = hbits & 0x03ff;            // 10 bits mantissa
            int exp = hbits & 0x7c00;            // 5 bits exponent
            if (exp == 0x7c00)                   // NaN/Inf
                exp = 0x3fc00;                    // -> NaN/Inf
            else if (exp != 0)                   // normalized value
            {
                exp += 0x1c000;                   // exp - 15 + 127
                if (mant == 0 && exp > 0x1c400)  // smooth transition
                    return BitConverter.ToSingle(BitConverter.GetBytes((hbits & 0x8000) << 16
                        | exp << 13 | 0x3ff), 0);
            }
            else if (mant != 0)                  // && exp==0 -> subnormal
            {
                exp = 0x1c400;                    // make it normal
                do
                {
                    mant <<= 1;                   // mantissa * 2
                    exp -= 0x400;                 // decrease exp by 1
                } while ((mant & 0x400) == 0); // while not normal
                mant &= 0x3ff;                    // discard subnormal bit
            }                                     // else +/-0 -> +/-0
            return BitConverter.ToSingle(BitConverter.GetBytes(          // combine all parts
                (hbits & 0x8000) << 16          // sign  << ( 31 - 15 )
                | (exp | mant) << 13), 0);         // value << ( 23 - 10 )
        }

                int bitPosition;
        public byte Peek()
        {
            byte b = ReadByte();
            BufferLocation--; //?
            return b;
        }

        public int ReadBits(int bitCount)
        {
            byte b = Peek();
            int value = 0;
            int le = 0;
            int bitIndex = 0;
            for (int i = 0; i < bitCount; i++)
            {
                byte bit = (byte)((b & (0x1 << (bitPosition))) >> (bitPosition));
                value |= (bit << (le + bitIndex));
                bitPosition++;
                bitIndex++;
                if (bitPosition >= 8)
                {
                    bitPosition = 0;
                    b = ReadByte();
                    b = Peek();
                }
                if (bitIndex >= 8)
                {
                    bitIndex = 0;
                    if (le + 8 > bitCount)
                    {
                        le = bitCount - 1;
                    }
                    else
                        le += 8;
                }
            }

            return value;
        }
    }
}
