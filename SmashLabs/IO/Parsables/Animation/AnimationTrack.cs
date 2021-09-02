using System;
using SmashLabs.Structs;
using SmashLabs.Tools;

namespace SmashLabs.IO.Parsables.Animation
{
    public class AnimationTrack
    {
        public string Name { get; set; }
        public uint Flags { get; set; }
        public uint FrameCount { get; set; }
        public uint Unk3 { get; set; }
        public uint DataOffset { get; set; }
        public long DataSize { get; set; }
        public static AnimationTrack[] ParseAnimationTracks(BufferReader reader)
        {
            BufferArrayPointer pointer = reader.ReadArrayPointer();

            AnimationTrack[] Out = new AnimationTrack[pointer.ElementCount];

            for (int i = 0; i < Out.Length; i++)
            {
                Out[i] = ParseAnimationTrack(reader);
            }

            pointer.End();

            return Out;
        }

        public static AnimationTrack ParseAnimationTrack(BufferReader reader)
        {
            AnimationTrack Out = new AnimationTrack();

            Out.Name = reader.ReadStringOffset();
            Out.Flags = reader.ReadObject<uint>();
            Out.FrameCount = reader.ReadObject<uint>();
            Out.Unk3 = reader.ReadObject<uint>();
            Out.DataOffset = reader.ReadObject<uint>();
            Out.DataSize = reader.ReadLong();

            return Out;
        }
    }
}
