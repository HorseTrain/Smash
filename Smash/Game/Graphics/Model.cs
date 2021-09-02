using OpenTK.Mathematics;
using Smash.Core.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Game.Graphics
{
    public class ModelMesh
    {
        public bool Active              { get; set; }
        public bool Rigged              { get; set; }
        public string ParentBone        { get; set; }
        public RenderMesh mesh          { get; set; }
        public Material material        { get; set; }

        public Matrix4 ParentBoneTransform  { get; set; }

        public void GenericDraw()
        {
            if (material != null)
                material.Use();

            material.UniformInt("IsRigged", Rigged ? 1 : 0);

            if (!Rigged)
                material.UniformMat4("ParentBone", ParentBoneTransform);

            mesh.GenericDraw();
        }
    }

    public class Model
    {
        public Skeleton skeleton        { get; set; }
        public List<ModelMesh> Meshes   { get; set; }

        public void GenericDraw()
        {
            skeleton.UploadSkeletonData(out Matrix4[] WorldTransforms);

            foreach (ModelMesh mesh in Meshes)
            {
                Bone parent = skeleton.GetBone(mesh.ParentBone);

                if (parent != null)
                {
                    mesh.ParentBoneTransform = WorldTransforms[parent.ID];
                }

                skeleton.Use(mesh.material.shader);

                mesh.GenericDraw();
            }
        }
    }
}
