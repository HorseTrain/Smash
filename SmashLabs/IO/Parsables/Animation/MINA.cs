using SmashLabs.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Animation
{
    public class MINA : IParsable
    {
        public string AnimationName { get; set; }
        public int FrameCount { get; set; }
        public int Unk1 { get; set; }
        public AnimationGroup[] Animations { get; set; }
        public byte[] AnimationDataBuffer { get; set; }

        public override void LoadData()
        {
            reader.Seek(0x18);

            FrameCount = (int)reader.ReadSingle();

            Unk1 = reader.ReadInt();

            AnimationName = reader.ReadStringOffset();
            
            Animations = AnimationGroup.ParseAnimationGroups(reader);

            BufferArrayPointer pointer = reader.ReadArrayPointer();

            reader.Seek(pointer.AbsoluteOffset);

            AnimationDataBuffer = reader.ReadObject<byte>((int)pointer.ElementCount);
        }
    }
}
