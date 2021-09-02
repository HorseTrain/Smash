using SmashLabs.IO.Parsables.Mesh;
using SmashLabs.IO.Parsables.Mesh.Rigging;
using SmashLabs.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Tools.Accessors
{
    public class RigAccessor
    {
        HSEM file;

        public RigAccessor(HSEM file)
        {
            this.file = file;
        }

        public VertexRig[] ReadVertexWeightData(MeshObject Object, Dictionary<string, int> SkeletonDic, out bool Rigged)
        {
            SBInfluence[] inf = ReadRigBuffer(Object.Name,(int)Object.SubIndex);

            if (inf.Length == 0)
            {
                Rigged = false;

                VertexRig[] Out = new VertexRig[Object.MeshData.VertexCount];

                int index = 0;
                
                if (SkeletonDic.ContainsKey(Object.ParentBoneName))
                    index = SkeletonDic[Object.ParentBoneName];

                for (int i = 0; i < Out.Length;i++)
                {
                    Out[i].VertexWeight = new Vector4(1);
                    Out[i].VertexWeightIndex = new Vector4I(index);
                }

                return Out;
            }
            else
            {
                Rigged = true;

                VertexRig[] Out = new VertexRig[Object.MeshData.VertexCount];

                foreach (SBInfluence i in inf)
                {
                    if (SkeletonDic.ContainsKey(i.BoneName))
                    {
                        int bone = SkeletonDic[i.BoneName];

                        if (Out[i.VertexIndex].VertexWeight.X == 0)
                        {
                            Out[i.VertexIndex].VertexWeight.X = i.VertexWeight;
                            Out[i.VertexIndex].VertexWeightIndex.X = bone;
                        }
                        else if (Out[i.VertexIndex].VertexWeight.Y == 0)
                        {
                            Out[i.VertexIndex].VertexWeight.Y = i.VertexWeight;
                            Out[i.VertexIndex].VertexWeightIndex.Y = bone;
                        }
                        else if (Out[i.VertexIndex].VertexWeight.Z == 0)
                        {
                            Out[i.VertexIndex].VertexWeight.Z = i.VertexWeight;
                            Out[i.VertexIndex].VertexWeightIndex.Z = bone;
                        }
                        else if (Out[i.VertexIndex].VertexWeight.W == 0)
                        {
                            Out[i.VertexIndex].VertexWeight.W = i.VertexWeight;
                            Out[i.VertexIndex].VertexWeightIndex.W = bone;
                        }
                    }
                }

                return Out;
            }
        }

        public SBInfluence[] ReadRigBuffer(string MeshName, int subIndex)
        {
            MeshRiggingGroup group = FindRiggingGroup(MeshName, subIndex);

            if (group == null)
            {
                return new SBInfluence[0];
            }

            List<SBInfluence> Out = new List<SBInfluence>();

            foreach (MeshBoneBuffer buffer in group.Buffers)
            {
                file.reader.Seek(buffer.offset);

                for (int i = 0; i < buffer.Size / 6; i++)
                {
                    SBInfluence temp = new SBInfluence();

                    temp.BoneName = buffer.name;

                    temp.VertexIndex = file.reader.ReadShort();
                    temp.VertexWeight = file.reader.ReadSingle();

                    Out.Add(temp);
                }
            }

            return Out.ToArray();
        }

        public MeshRiggingGroup FindRiggingGroup(string meshName, int subIndex)
        {
            foreach (MeshRiggingGroup g in file.RigBuffers)
            {
                if (g.Name == meshName && g.SubIndex == subIndex)
                {
                    return g;
                }
            }

            return null;
        }
    }
}
