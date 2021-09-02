using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Smash.Core.Graphics;
using Smash.Game.Graphics;
using SmashLabs.IO;
using SmashLabs.IO.Parsables.Animation;
using SmashLabs.IO.Parsables.Mesh;
using SmashLabs.IO.Parsables.Model;
using SmashLabs.IO.Parsables.Skeleton;
using SmashLabs.Structs;
using SmashLabs.Tools.Accessors;
using SmashLabs.Tools.Accessors.Animation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Game.Asset
{
    public class SmashAssetLoader
    {
        const string AssetPath = @"E:\root\";

        public static string GetPath(string source) => AssetPath + source;

        static RenderShader SimpleShader = LoadShader(SmashAssetLoader.GetPath(@"shader\fighter\"));

        static List<ModelMesh> ParseMeshCollection(LEKS skel,HSEM mesh)
        {
            List<ModelMesh> Out = new List<ModelMesh>();

            VertexAccesor vertexAccesor = new VertexAccesor(mesh);
            RigAccessor rigAccessor = new RigAccessor(mesh);

            foreach (MeshObject meshObject in mesh.Objects)
            {
                RenderMesh oMesh = new RenderMesh();

                uint[] Indecies = vertexAccesor.ReadIndicies(meshObject);
                SmashVertex[] SmashVerticies = vertexAccesor.ReadVertexData(meshObject);
                VertexRig[] SmashRigs = rigAccessor.ReadVertexWeightData(meshObject, skel.BoneDic, out bool rigged);

                oMesh.Indicies.AddRange(Indecies);

                for (int i = 0; i < SmashVerticies.Length; ++i)
                {
                    SmashVertex sVertex = SmashVerticies[i];
                    VertexRig rVertex = SmashRigs[i];

                    RenderVertex renderVertex = new RenderVertex();

                    renderVertex.Position = BinaryTools.ConvertStructs<OpenTK.Mathematics.Vector3, SmashLabs.Structs.Vector3>(sVertex.VertexPosition);
                    renderVertex.Normal = BinaryTools.ConvertStructs<OpenTK.Mathematics.Vector3, SmashLabs.Structs.Vector3>(sVertex.VertexNormal);
                    renderVertex.TextCoord0 = BinaryTools.ConvertStructs<OpenTK.Mathematics.Vector2, SmashLabs.Structs.Vector2>(sVertex.VertexMap0);
                    renderVertex.TextCoord1 = BinaryTools.ConvertStructs<OpenTK.Mathematics.Vector2, SmashLabs.Structs.Vector2>(sVertex.VertexMap1);
                    renderVertex.Color0 = BinaryTools.ConvertStructs<OpenTK.Mathematics.Vector4, SmashLabs.Structs.Vector4>(sVertex.VertexColor);

                    renderVertex.Weights = BinaryTools.ConvertStructs<OpenTK.Mathematics.Vector4, SmashLabs.Structs.Vector4>(rVertex.VertexWeight);
                    renderVertex.WeightIndex = BinaryTools.ConvertStructs<OpenTK.Mathematics.Vector4i, SmashLabs.Structs.Vector4I>(rVertex.VertexWeightIndex);

                    oMesh.Vertices.Add(renderVertex);
                }

                ModelMesh mMesh = new ModelMesh();

                mMesh.Rigged = rigged;
                mMesh.mesh = oMesh;
                mMesh.ParentBone = meshObject.ParentBoneName;

                mMesh.material = new Material(SimpleShader);

                Out.Add(mMesh);
            }

            return Out;
        }

        public static Skeleton LoadSmashSkeleton(LEKS leks)
        {
            Skeleton Out = new Skeleton();

            Out.Bones = new List<Bone>(new Bone[leks.BoneEntries.Length]);

            for (int i = 0; i < leks.BoneEntries.Length; ++i)
            {
                BoneEntry entry = leks.BoneEntries[i];

                Bone bone = new Bone();

                bone.Name = entry.Name;

                bone.transform.LocalTransform = BinaryTools.ConvertStructs<OpenTK.Mathematics.Matrix4, SmashLabs.Structs.Matrix4>(entry.LocalTransform);
                bone.InverseTransform = BinaryTools.ConvertStructs<OpenTK.Mathematics.Matrix4, SmashLabs.Structs.Matrix4>(entry.InverseWorldTransform);

                bone.transform.ID = entry.Index;

                Out.Bones[entry.Index] = bone;
            }

            for (int i = 0; i < Out.Bones.Count; ++i)
            {
                BoneEntry entry = leks.BoneEntries[i];

                if (entry.ParentIndex == -1)
                {
                    Out.Bones[i].transform.Parent = Out.RootNode.transform;

                    continue;
                }

                Out.Bones[i].transform.Parent = Out.Bones[entry.ParentIndex].transform;
            }

            Out.BuildMap();

            return Out;
        }

        public static Model LoadSmashModel(string path)
        {
            Model Out = new Model();

            LDOM modelFile = (LDOM)IParsable.FromFile(path + "\\model.numdlb");
            LEKS skeletonFile = (LEKS)IParsable.FromFile(path + $"\\{modelFile.SkeletonFileName}");
            HSEM meshFile = (HSEM)IParsable.FromFile(path + $"\\{modelFile.MeshCollectionFileName}");

            Out.Meshes = ParseMeshCollection(skeletonFile, meshFile);
            Out.skeleton = LoadSmashSkeleton(skeletonFile);

            return Out;
        }

        public static RenderAnimation LoadSmashAnimation(string path)
        {
            RenderAnimation Out = new RenderAnimation();

            MINA anim = (MINA)IParsable.FromFile(path);

            AnimationTrackAccessor accessor = new AnimationTrackAccessor(anim);

            Out.FrameCount = anim.FrameCount;

            foreach (AnimationGroup Group in anim.Animations)
            {
                foreach (AnimationNode Node in Group.Nodes)
                {
                    foreach (AnimationTrack track in Node.Tracks)
                    {
                        object[] Data = accessor.ReadTrack(track);

                        KeyframeCollection collection = new KeyframeCollection();

                        collection.Data = new object[track.FrameCount];

                        if (track.Name == "Transform")
                        {
                            for (int i = 0; i < Data.Length; ++i)
                            {
                                AnimationTransform transform = (AnimationTransform)Data[i];
                                BoneKeyFrame boneKey = new BoneKeyFrame();

                                boneKey.LocalTranslation = BinaryTools.ConvertStructs<OpenTK.Mathematics.Vector3, SmashLabs.Structs.Vector3>(transform.Position);
                                boneKey.LocalScale = BinaryTools.ConvertStructs<OpenTK.Mathematics.Vector3, SmashLabs.Structs.Vector3>(transform.Scale);
                                boneKey.LocalRotation = BinaryTools.ConvertStructs<Quaternion, SmashLabs.Structs.Vector4>(transform.Rotation);

                                collection.Data[i] = boneKey;
                            }
                        }

                        if (track.Name == "Visibility")
                        {
                            for (int i = 0; i < Data.Length; ++i)
                            {
                                collection.Data[i] = Data[i];
                            }
                        }

                        Out.Keyframes.Add(Node.Name + "_" + track.Name, collection);
                    }
                }
            }

            return Out;
        }

        public static RenderShader LoadShader(string path)
        {
            RenderShader Out = new RenderShader();

            string[] Files = Directory.GetFiles(path);

            foreach (string file in Files)
            {
                string shader = ShaderCompiler.FillShader(File.ReadAllText(file));

                string name = Path.GetFileName(file.Split('.')[0]);

                Out.Sources.Add(new ShaderSource((ShaderType)Enum.Parse(typeof(ShaderType), name), shader));
            }

            Out.Compile();

            return Out;
        }
    }
}
