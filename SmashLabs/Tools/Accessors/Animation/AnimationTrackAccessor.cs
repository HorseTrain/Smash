using SmashLabs.IO.Parsables.Animation;
using SmashLabs.IO.Parsables.Animation.Enums;
using SmashLabs.Structs;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SmashLabs.Tools.Accessors.Animation
{
    //Thank you ploaj :)
    public class AnimationTrackAccessor
    {
        public MINA animationfile;

        public AnimationTrackAccessor(MINA file)
        {
            animationfile = file;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct SsbhAnimCompressedHeader
        {
            public ushort Unk_4; // always 4?
            public ushort Flags;
            public ushort DefaultDataOffset;
            public ushort BitsPerEntry;
            public int CompressedDataOffset;
            public int FrameCount;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct SbhAnimCompressedItem
        {
            public float Start;
            public float End;
            public ulong Count;
        }

        public bool CheckFlag(uint flags, uint mask, AnimTrackFlags expectedValue)
        {
            return (flags & mask) == (uint)expectedValue;
        }

        public object[] ReadTrack(AnimationTrack track)
        {
            List<object> output = new List<object>();

            BufferReader parser = new BufferReader(animationfile.AnimationDataBuffer);

            parser.Seek(track.DataOffset);

            if (CheckFlag(track.Flags, 0xFF00, AnimTrackFlags.Constant))
            {
                output.Add(ReadDirect(parser, track.Flags));
            }
            if (CheckFlag(track.Flags, 0xFF00, AnimTrackFlags.ConstTransform))
            {
                output.Add(ReadDirect(parser, track.Flags)); //?
            }
            if (CheckFlag(track.Flags, 0xFF00, AnimTrackFlags.Direct))
            {
                for (int i = 0; i < track.FrameCount; i++)
                    output.Add(ReadDirect(parser, track.Flags));
            }
            if (CheckFlag(track.Flags, 0xFF00, AnimTrackFlags.Compressed))
            {
                output.AddRange(ReadCompressed(parser, track.Flags));
            }

            return output.ToArray();
        }

        public object ReadDirect(BufferReader reader, uint flags)
        {
            if (CheckFlag(flags, 0x00FF, AnimTrackFlags.Transform))
                return reader.ReadObject<AnimationTransform>();

            if (CheckFlag(flags, 0x00FF, AnimTrackFlags.Texture))
                return reader.ReadObject<AnimTrackTexture>();

            if (CheckFlag(flags, 0x00FF, AnimTrackFlags.Float))
                return reader.ReadSingle();

            if (CheckFlag(flags, 0x00FF, AnimTrackFlags.PatternIndex))
                return reader.ReadInt();

            if (CheckFlag(flags, 0x00FF, AnimTrackFlags.Boolean))
                return reader.ReadByte() == 1;

            if (CheckFlag(flags, 0x00FF, AnimTrackFlags.Vector4))
                return reader.ReadObject<Vector4>();

            return null;
        }

        private object[] ReadCompressed(BufferReader reader, uint flags)
        {
            List<object> output = new List<object>();

            uint dataOffset = (uint)reader.BufferLocation;
            SsbhAnimCompressedHeader header = reader.ReadObject<SsbhAnimCompressedHeader>();

            if (CheckFlag(flags, 0x00FF, AnimTrackFlags.Boolean))
            {
                ReadBools(reader, output, dataOffset, header);
            }
            if (CheckFlag(flags, 0x00FF, AnimTrackFlags.Texture))
            {
                //Not Implamented
            }
            if (CheckFlag(flags, 0x00FF, AnimTrackFlags.Float))
            {
                //Not Implamented
            }
            if (CheckFlag(flags, 0x00FF, AnimTrackFlags.PatternIndex))
            {
                //Not Implamented
            }
            if (CheckFlag(flags, 0x00FF, AnimTrackFlags.Vector4))
            {
                ReadVector4(reader, output, dataOffset, header);
            }
            if (CheckFlag(flags, 0x00FF, AnimTrackFlags.Transform))
            {
                ReadTransform(reader, output, dataOffset, header);
            }

            return output.ToArray();
        }

        void ReadVector4(BufferReader reader, List<object> output, uint dataoffset, SsbhAnimCompressedHeader header)
        {
            var decompressed = DecompressValues(reader, dataoffset, header, 4);

            foreach (var decom in decompressed)
                output.Add(new Vector4(decom[0], decom[1], decom[2], decom[3]));
        }

        void ReadTransform(BufferReader reader, List<object> output, uint dataOffset, SsbhAnimCompressedHeader header)
        {
            var decompressed = DecompressTransform(reader, dataOffset, header);
            foreach (var v in decompressed)
            {
                output.Add(v);
            }
        }

        void ReadBools(BufferReader reader, List<object> output, uint dataOffset, SsbhAnimCompressedHeader header)
        {
            reader.Seek((int)dataOffset + header.CompressedDataOffset);
            // note: there is a section for "default" and "compressed items" but they seem to always be 0-ed out

            for (int i = 0; i < header.FrameCount; i++)
            {
                output.Add(reader.ReadBits(header.BitsPerEntry) == 1);
            }
        }

        List<float[]> DecompressValues(BufferReader parser, uint dataOffset, SsbhAnimCompressedHeader header, int valueCount)
        {
            List<float[]> transforms = new List<float[]>(header.FrameCount);

            // PreProcess
            SbhAnimCompressedItem[] items = parser.ReadObject<SbhAnimCompressedItem>(valueCount);

            parser.Seek(dataOffset + header.DefaultDataOffset);

            float[] defaultValues = GetDefaultValues(parser, valueCount);

            parser.Seek(dataOffset + header.CompressedDataOffset);
            for (int frameIndex = 0; frameIndex < header.FrameCount; frameIndex++)
            {
                // Copy default values.
                float[] values = new float[valueCount];
                for (int i = 0; i < valueCount; i++)
                    values[i] = defaultValues[i];

                for (int itemIndex = 0; itemIndex < items.Length; itemIndex++)
                {
                    var item = items[itemIndex];

                    // Decompress
                    int valueBitCount = (int)item.Count;
                    if (valueBitCount == 0)
                        continue;

                    int value = parser.ReadBits(valueBitCount);
                    int scale = 0;
                    for (int k = 0; k < valueBitCount; k++)
                        scale |= 0x1 << k;

                    float frameValue = Lerp(item.Start, item.End, 0, 1, value / (float)scale);
                    if (float.IsNaN(frameValue))
                        frameValue = 0;

                    values[itemIndex] = frameValue;
                }

                transforms.Add(values);
            }

            return transforms;
        }

        private AnimationTransform[] DecompressTransform(BufferReader parser, uint dataOffset, SsbhAnimCompressedHeader header)
        {
            AnimationTransform[] transforms = new AnimationTransform[header.FrameCount];

            // PreProcess
            SbhAnimCompressedItem[] items = parser.ReadObject<SbhAnimCompressedItem>(9);

            parser.Seek(dataOffset + header.DefaultDataOffset);

            float xsca = parser.ReadSingle();
            float ysca = parser.ReadSingle();
            float zsca = parser.ReadSingle();
            float xrot = parser.ReadSingle();
            float yrot = parser.ReadSingle();
            float zrot = parser.ReadSingle();
            float wrot = parser.ReadSingle();
            float xpos = parser.ReadSingle();
            float ypos = parser.ReadSingle();
            float zpos = parser.ReadSingle();
            float csca = parser.ReadSingle();

            parser.Seek(dataOffset + header.CompressedDataOffset);
            for (int frame = 0; frame < header.FrameCount; frame++)
            {
                AnimationTransform transform = new AnimationTransform()
                {
                    Position = new Vector3(xpos, ypos, zpos),
                    Rotation = new Vector4(xrot, yrot, zrot, wrot),
                    Scale = new Vector3(xsca, ysca, zsca),
                    CompensateScale = csca
                };
                for (int itemIndex = 0; itemIndex < items.Length; itemIndex++)
                {
                    // First check if this track should be parsed
                    // TODO: Don't hard code these flags.
                    if (!((itemIndex == 0 && (header.Flags & 0x3) == 0x3) //isotropic scale
                                || (itemIndex >= 0 && itemIndex <= 2 && (header.Flags & 0x3) == 0x1) //normal scale
                                || (itemIndex > 2 && itemIndex <= 5 && (header.Flags & 0x4) > 0)
                                || (itemIndex > 5 && itemIndex <= 8 && (header.Flags & 0x8) > 0)))
                    {
                        continue;
                    }

                    var item = items[itemIndex];

                    // Decompress
                    int valueBitCount = (int)item.Count;
                    if (valueBitCount == 0)
                        continue;

                    int value = parser.ReadBits(valueBitCount);
                    int scale = 0;
                    for (int k = 0; k < valueBitCount; k++)
                        scale |= 0x1 << k;

                    float frameValue = Lerp(item.Start, item.End, 0, 1, value / (float)scale);
                    if (float.IsNaN(frameValue))
                        frameValue = 0;

                    // the Transform type relies a lot on flags

                    if ((header.Flags & 0x3) == 0x3)
                    {
                        //Scale Compensate
                        if (itemIndex == 0)
                        {
                            transform.CompensateScale = frameValue;
                        }
                    }
                    if ((header.Flags & 0x3) == 0x1)
                    {
                        //Scale normal
                        switch (itemIndex)
                        {
                            case 0:
                                transform.Scale.X = frameValue;
                                break;
                            case 1:
                                transform.Scale.Y = frameValue;
                                break;
                            case 2:
                                transform.Scale.Z = frameValue;
                                break;
                        }
                    }
                    //Rotation and Position
                    switch (itemIndex)
                    {
                        case 3:
                            transform.Rotation.X = frameValue;
                            break;
                        case 4:
                            transform.Rotation.Y = frameValue;
                            break;
                        case 5:
                            transform.Rotation.Z = frameValue;
                            break;
                        case 6:
                            transform.Position.X = frameValue;
                            break;
                        case 7:
                            transform.Position.Y = frameValue;
                            break;
                        case 8:
                            transform.Position.Z = frameValue;
                            break;
                    }
                }

                // Rotations have an extra bit at the end
                if ((header.Flags & 0x4) > 0)
                {
                    bool wFlip = parser.ReadBits(1) == 1;

                    // W is calculated
                    transform.Rotation.W = (float)Math.Sqrt(Math.Abs(1 - (transform.Rotation.X * transform.Rotation.X + transform.Rotation.Y * transform.Rotation.Y + transform.Rotation.Z * transform.Rotation.Z)));

                    if (wFlip)
                        transform.Rotation.W = -transform.Rotation.W;
                }


                transforms[frame] = transform;
            }

            return transforms;
        }

        private static float[] GetDefaultValues(BufferReader parser, int valueCount)
        {
            float[] defaultValues = new float[valueCount];
            for (int i = 0; i < valueCount; i++)
            {
                defaultValues[i] = parser.ReadSingle();
            }

            return defaultValues;
        }

        public static float Lerp(float av, float bv, float v0, float v1, float t)
        {
            if (v0 == v1) return av;

            if (t == v0) return av;
            if (t == v1) return bv;

            float mu = (t - v0) / (v1 - v0);
            float value = ((av * (1 - mu)) + (bv * mu));
            if (float.IsNaN(value))
                return 0;
            return value;
        }
    }
}
