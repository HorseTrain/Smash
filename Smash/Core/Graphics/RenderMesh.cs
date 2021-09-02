using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Smash.Core.Graphics
{
    public struct RenderVertex
    {
        public Vector2 TextCoord0;
        public Vector2 TextCoord1;

        public Vector3 Position;
        public Vector3 Normal;
        public Vector3 Tangent;
        public Vector3 BiTangent;

        public Vector4 Color0;
        public Vector4 Color1;
        public Vector4 Weights;

        public Vector4i WeightIndex;
    }

    struct MeshHandle
    {
        public int VAO;
        public int VBO;
        public int IBO;
        public bool IsUploaded;
    }

    public class RenderMesh
    {
        public List<RenderVertex> Vertices  { get; set; } = new List<RenderVertex>();
        public List<uint> Indicies          { get; set; } = new List<uint>();

        public int VertexCount              => Vertices.Count;
        public int IndexCount               => Indicies.Count;

        public BeginMode RenderMode         { get; private set; }

        //This can be used to toggle a reupload
        public bool Uploaded                { get; set; } = false;

        private MeshHandle Handle;

        public RenderMesh(BeginMode mode = BeginMode.Triangles)
        {
            RenderMode = mode;

            Handle.IsUploaded = false;
        }

        public void Bind()
        {
            GL.BindVertexArray(Handle.VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Handle.IBO);
        }

        public unsafe void Upload()
        {
            if (Handle.IsUploaded)
            {
                DeloadBuffers(Handle);
            }

            Handle.VAO = GL.GenVertexArray();
            Handle.IBO = GL.GenBuffer();
            Handle.VBO = GL.GenBuffer();

            GL.BindVertexArray(Handle.VAO);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Handle.IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indicies.Count * sizeof(uint), Indicies.ToArray(), BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle.VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Count * sizeof(RenderVertex), Vertices.ToArray(), BufferUsageHint.StaticDraw);

            SetVertexAttrib(0, 2, VertexAttribPointerType.Float, nameof(RenderVertex.TextCoord0));
            SetVertexAttrib(1, 2, VertexAttribPointerType.Float, nameof(RenderVertex.TextCoord1));

            SetVertexAttrib(2, 3, VertexAttribPointerType.Float, nameof(RenderVertex.Position));
            SetVertexAttrib(3, 3, VertexAttribPointerType.Float, nameof(RenderVertex.Normal));
            SetVertexAttrib(4, 3, VertexAttribPointerType.Float, nameof(RenderVertex.Tangent));
            SetVertexAttrib(5, 3, VertexAttribPointerType.Float, nameof(RenderVertex.BiTangent));

            SetVertexAttrib(6, 4, VertexAttribPointerType.Float, nameof(RenderVertex.Color0));
            SetVertexAttrib(7, 4, VertexAttribPointerType.Float, nameof(RenderVertex.Color1));
            SetVertexAttrib(8, 4, VertexAttribPointerType.Float, nameof(RenderVertex.Weights));

            SetVertexAttrib(9, 4, VertexAttribPointerType.Int, nameof(RenderVertex.WeightIndex));

            Uploaded = true;
        }

        unsafe void SetVertexAttrib(int index, int count, VertexAttribPointerType Type, string name)
        {
            if (Type == VertexAttribPointerType.Float)
            {
                GL.VertexAttribPointer(index, count, Type, false, sizeof(RenderVertex), Marshal.OffsetOf<RenderVertex>(name));
            }
            else if (Type == VertexAttribPointerType.Int)
            {
                GL.VertexAttribIPointer(index, count, (VertexAttribIntegerType)Type, sizeof(RenderVertex), Marshal.OffsetOf<RenderVertex>(name));
            }

            GL.EnableVertexAttribArray(index);
        }

        public void GenericDraw()
        {
            if (!Uploaded)
            {
                Upload();
            }

            Bind();

            GL.DrawElements(RenderMode,Indicies.Count, DrawElementsType.UnsignedInt,0);
        }

        ~RenderMesh()
        {
            Trash.Add(Handle);
        }

        static List<MeshHandle> Trash       { get; set; } = new List<MeshHandle>();

        public static void CollectTrash()
        {
            foreach (MeshHandle handle in Trash)
            {
                DeloadBuffers(handle);
            }

            Trash = new List<MeshHandle>();
        }

        static void DeloadBuffers(MeshHandle handle)
        {
            GL.DeleteVertexArray(handle.VAO);
            GL.DeleteBuffer(handle.VBO);
            GL.DeleteBuffer(handle.IBO);
        }
    }
}
