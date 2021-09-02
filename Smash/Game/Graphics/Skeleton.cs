using OpenTK.Mathematics;
using Smash.Core.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Game.Graphics
{
    public class Bone
    {
        public int ID                       => transform.ID;
        public string Name                  { get; set; }
        public Transform transform          { get; set; } = new Transform();
        public Matrix4 InverseTransform     { get; set; } = Matrix4.Identity;
    }

    public class Skeleton
    {
        public const string UBOName = "TransformData";

        public UniformBufferBlock Buffer        { get; set; }
        public Bone RootNode                    { get; private set; }
        public List<Bone> Bones                 { get; set; }
        public Dictionary<string, Bone> BoneMap { get; set; }

        public Bone GetBone(string Name)
        {
            if (BoneMap.ContainsKey(Name))
                return BoneMap[Name];

            return null;
        }

        unsafe public Skeleton()
        {
            RootNode = new Bone();
            Buffer = new UniformBufferBlock(sizeof(Matrix4) * 400, UBOName);
        }

        public void BuildMap()
        {
            BoneMap = new Dictionary<string, Bone>();

            foreach (Bone bone in Bones)
            {
                BoneMap.Add(bone.Name, bone);
            }
        }

        public Matrix4[] GetInverseWorldTransforms()
        {
            Matrix4[] Out = new Matrix4[Bones.Count];

            for (int i = 0; i < Bones.Count; ++i)
            {
                Out[i] = Bones[i].InverseTransform;
            }

            return Out;
        }

        public Matrix4[] FastCalculateWorldTransforms()
        {
            void FillTransformBuffer(Transform bone, ref Matrix4[] Out)
            {
                if (bone.Parent == null)
                {
                    Out[bone.ID] = bone.LocalTransform;
                }
                else
                {
                    Out[bone.ID] = bone.LocalTransform * Out[bone.Parent.ID];
                }

                for (int i = 0; i < bone.ChildCount; ++i)
                {
                    FillTransformBuffer(bone.GetChild(i), ref Out);
                }
            }

            Matrix4[] Out = new Matrix4[Bones.Count];

            FillTransformBuffer(RootNode.transform, ref Out);

            return Out;
        }

        public Matrix4[] CalculateWorldTransforms()
        {
            Matrix4[] Out = new Matrix4[Bones.Count];

            foreach (Bone bone in Bones)
            {
                Out[bone.ID] = bone.transform.WorldTransform;
            }

            return Out;
        }

        public unsafe void UploadSkeletonData(out Matrix4[] WorldTransforms)
        {
            WorldTransforms = FastCalculateWorldTransforms();
            Matrix4[] InverseTransforms = GetInverseWorldTransforms();

            for (int i = 0; i < Bones.Count; ++i)
            {
                Buffer.Write((uint)i, InverseTransforms[i] * WorldTransforms[i]);
            }

            Buffer.Upload();
        }

        public void Use(RenderShader shader)
        {
            Buffer.Use(shader);
        }
    }
}
