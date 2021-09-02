using SmashLabs.Structs;
using SmashLabs.Tools.Exporter;
using SmashLabs.Tools.Exporters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Skeleton
{
    //Use updated model.
    public class LEKS : IParsable
    {
        public BoneEntry[] BoneEntries { get; set; }
        public Dictionary<string,int> BoneDic { get; set; }

        public override void LoadData()
        {
            long EntryDataLocation = reader.ReadOffset();

            BoneEntries = new BoneEntry[reader.ReadLong()];

            long MatrixDataLocation = reader.ReadOffset();

            reader.Seek(EntryDataLocation);

            BoneDic = new Dictionary<string, int>();

            for (int i = 0; i < BoneEntries.Length; i++)
            {
                BoneEntries[i] = new BoneEntry()
                {
                    Name = reader.ReadStringOffset(),
                    Index = reader.ReadShort(),
                    ParentIndex = reader.ReadShort(),
                    Type = reader.ReadInt()
                };

                BoneDic.Add(BoneEntries[i].Name,BoneEntries[i].Index);
            }

            reader.Seek(MatrixDataLocation);

            Matrix4[] WorldTransforms = reader.ReadObject<Matrix4>(BoneEntries.Length);
            Matrix4[] InverseWorldTransforms = reader.ReadObject<Matrix4>(BoneEntries.Length);
            Matrix4[] Transforms = reader.ReadObject<Matrix4>(BoneEntries.Length);
            Matrix4[] InverseTransforms = reader.ReadObject<Matrix4>(BoneEntries.Length);

            for (int i = 0; i < BoneEntries.Length; i++)
            {
                BoneEntries[i].WorldTransform = WorldTransforms[i];
                BoneEntries[i].InverseWorldTransform = InverseWorldTransforms[i];
                BoneEntries[i].LocalTransform = Transforms[i];
                BoneEntries[i].InverseLocalTransform = InverseTransforms[i];
            }
        }

        public override unsafe byte[] GetData()
        {
            ExportableBufferCollection Out = new ExportableBufferCollection();

            ByteBuffer Header = Out.AddBuffer();

            ByteBuffer EntryBuffer = Out.AddBuffer();

            ByteBuffer StringBuffer = Out.AddBuffer();

            ByteBuffer WorldTransformBuffer = Out.AddBuffer();
            ByteBuffer InverseWorldTransformBuffer = Out.AddBuffer();

            ByteBuffer LocalTransformBuffer = Out.AddBuffer();
            ByteBuffer InverseLocalTransformBuffer = Out.AddBuffer();

            Header.AddObject(this.Header);
            Header.AddObject(this.Magic);

            Out.AddPointer(Header,EntryBuffer);

            Header.AddObject((long)BoneEntries.Length); Out.AddPointer(Header,WorldTransformBuffer);
            Header.AddObject((long)BoneEntries.Length); Out.AddPointer(Header, InverseWorldTransformBuffer);
            Header.AddObject((long)BoneEntries.Length); Out.AddPointer(Header, LocalTransformBuffer);
            Header.AddObject((long)BoneEntries.Length); Out.AddPointer(Header, InverseLocalTransformBuffer);

            Header.AddObject((long)BoneEntries.Length);

            foreach (BoneEntry entry in BoneEntries)
            {
                Out.AddStringWithPointer(entry.Name,EntryBuffer,StringBuffer);
                EntryBuffer.AddObject(entry.Index);
                EntryBuffer.AddObject(entry.ParentIndex);
                EntryBuffer.AddObject(entry.Type);

                WorldTransformBuffer.AddObject(entry.WorldTransform);
                InverseWorldTransformBuffer.AddObject(entry.InverseWorldTransform);

                LocalTransformBuffer.AddObject(entry.LocalTransform);
                InverseLocalTransformBuffer.AddObject(entry.InverseLocalTransform);
            }

            return Out.FinalBuffer();
        }
    }
}
