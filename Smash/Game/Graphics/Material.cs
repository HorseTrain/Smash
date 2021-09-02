using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Smash.Core.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Game.Graphics
{
    public class Material
    {
        public RenderShader shader              { get; private set; }
        public RenderTexture[] Textures         { get; private set; }
        Dictionary<string, int> UniformNames    { get; set; }

        public int GetUniformLocation(string name)
        {
            shader.Use();

            if (!UniformNames.ContainsKey(name))
                UniformNames.Add(name, GL.GetUniformLocation(shader.Handle, name));

            return UniformNames[name];
        }

        public void UniformInt(string name, int value)
        {
            shader.Use();

            GL.Uniform1(GetUniformLocation(name), value);
        }

        public void UniformMat4(string name, Matrix4 value)
        {
            shader.Use();

            GL.UniformMatrix4(GetUniformLocation(name), false, ref value);
        }

        public Material(RenderShader shader, int TextureAllocation = 8)
        {
            this.shader = shader;
            Textures = new RenderTexture[TextureAllocation];

            UniformNames = new Dictionary<string, int>();
        }

        public void Use()
        {
            shader.Use();

            for (int i = 0; i < Textures.Length; ++i)
            {

            }
        }
    }
}
