using SmashLabs.Tools.Exporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Tools.Exporters
{
    public unsafe class BufferWriter
    {
        public byte[] Data { get; set; }

        public byte* Location
        {
            get
            {
                fixed (byte * temp = Data)
                {
                    return temp;
                }
            }
        }

        public T GetObject<T>(int offset) where T: unmanaged
        {
            return *((T*)(&Location[offset]));
        }

        public void WriteObject<T>(T Data,int offset) where T: unmanaged
        {
            for (int i = 0; i < sizeof(T); i++)
            {
                this.Data[offset + i] = ((byte*)&Data)[i];
            }
        }

        public BufferWriter(byte[] data)
        {
            this.Data = data;
        }
    }

    public class ExportableBufferCollection
    {
        public List<ArrayPointer> Pointers { get; set; } = new List<ArrayPointer>();
        public List<ByteBuffer> Buffers { get; set; } = new List<ByteBuffer>();

        public ByteBuffer AddBuffer()
        {
            Buffers.Add(new ByteBuffer());

            return Buffers[Buffers.Count - 1];
        }

        public List<byte> BuildBuffer()
        {
            List<byte> Out = new List<byte>();

            foreach (List<byte> buffer in Buffers)
            {
                foreach (byte b in buffer)
                {
                    Out.Add(b);
                }
            }

            return Out;
        }

        public List<byte> BuildBuffer(out List<int> offsets)
        {
            List<byte> Out = new List<byte>();

            offsets = new List<int>();

            foreach (List<byte> buffer in Buffers)
            {
                offsets.Add(Out.Count);

                foreach (byte b in buffer)
                {
                    Out.Add(b);
                }
            }

            return Out;
        }

        public byte[] FinalBuffer()
        {
            List<int> offsets;

            byte[] temp = BuildBuffer(out offsets).ToArray();

            BufferWriter writer = new BufferWriter(temp);

            foreach (ArrayPointer pointer in Pointers)
            {
                int absoluteoffset = offsets[pointer.PointerDataBufferIndex] + pointer.DataOffset;
                int absolutepointeroffset = offsets[pointer.PointerLocationBufferIndex] + pointer.PointerLocation;

                int offset = absoluteoffset - absolutepointeroffset;

                writer.WriteObject((long)offset, absolutepointeroffset);
            }

            return temp;
        }

        public void AddPointer(ByteBuffer pointerlocation, ByteBuffer datalocation)
        {
            ArrayPointer temp = new ArrayPointer()
            {
                PointerLocationBufferIndex = Buffers.IndexOf(pointerlocation),
                PointerDataBufferIndex = Buffers.IndexOf(datalocation),
                PointerLocation = pointerlocation.Count,
                DataOffset = datalocation.Count
            };

            pointerlocation.AddPadding(8);

            Pointers.Add(temp);
        }

        public void AddArrayPointer(ByteBuffer pointerlocation, ByteBuffer datalocation, int count)
        {
            AddPointer(pointerlocation,datalocation);
            pointerlocation.AddObject((long)count);
        }

        public Dictionary<ByteBuffer, List<string>> ExistingStrings = new Dictionary<ByteBuffer, List<string>>();

        public Dictionary<ByteBuffer, List<int>> ExistingStringOffsets = new Dictionary<ByteBuffer, List<int>>();

        public void AddStringWithPointer(string str, ByteBuffer pointerlocation, ByteBuffer datalocation,bool pad = true)
        {
            if (pointerlocation == datalocation)
                throw new Exception("Pointer location can not be datta location!!");

            AddPointer(pointerlocation, datalocation);
            datalocation.AddString(str, pad);
        }
    }
}
