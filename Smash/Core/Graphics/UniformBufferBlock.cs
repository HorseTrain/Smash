using OpenTK.Graphics.OpenGL;
using Smash.Game;
using Smash.Game.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Core.Graphics
{
    public unsafe class UniformBufferBlock
    {
        public string Name          { get; private set; }
        int UBO                     { get; set; } = -1;
        byte[] Buffer               { get; set; }

        public UniformBufferBlock(int size, string name)
        {
            Buffer = new byte[size];
            Name = name;
        }

        public void Write(uint Address, void* Data, uint size)
        {
            Debug.Assert(Address + size <= Buffer.Length);

            fixed (byte* data = Buffer)
            {
                BinaryTools.memcpy(data + Address, Data, (int)size);
            }
        }

        public void Write<T>(uint index, T Data) where T : unmanaged
        {
            Write(index * (uint)sizeof(T), &Data, (uint)sizeof(T));
        }

        public void Upload()
        {
            if (UBO == -1)
            {
                UBO = GL.GenBuffer();
            }
            else
            {
                GL.DeleteBuffer(UBO);
            }

            GL.BindBuffer(BufferTarget.UniformBuffer, UBO);
            GL.BufferData(BufferTarget.UniformBuffer, Buffer.Length, Buffer, BufferUsageHint.StaticDraw);
        }

        public void Use(RenderShader Shader)
        {
            Shader.Use();

            int BlockIndex = GL.GetUniformBlockIndex(Shader.Handle,Name );

            Debug.Assert(BlockIndex != -1);

            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, BlockIndex, UBO);
            GL.UniformBlockBinding(Shader.Handle, BlockIndex,BlockIndex );
        }

        static List<int> Trash { get; set; } = new List<int>();

        public static void CollectTrash()
        {
            foreach (int trash in Trash)
            {
                GL.DeleteBuffer(trash);
            }

            Trash = new List<int>();
        }
    }
}
