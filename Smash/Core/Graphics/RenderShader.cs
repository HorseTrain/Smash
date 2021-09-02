using OpenTK.Graphics.OpenGL;
using Smash.Game;
using Smash.Game.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Core.Graphics
{
    public class ShaderSource
    {
        public ShaderType Type  { get; set; }
        public string Source    { get; set; }

        public ShaderSource(ShaderType Type, string Source)
        {
            this.Type = Type;
            this.Source = Source;
        }
    }

    public class RenderShader
    {
        public List<ShaderSource> Sources   { get; set; } = new List<ShaderSource>();

        public int Handle                   { get; set; }

        public void Compile()
        {
            DeleteShader();

            Handle = GL.CreateProgram();

            foreach (ShaderSource source in Sources)
            {
                int sHandle = CompileSource(source);

                GL.AttachShader(Handle, sHandle);

                GL.DeleteShader(sHandle);
            }

            GL.LinkProgram(Handle);
            GL.ValidateProgram(Handle);
        }

        static int CompileSource(ShaderSource Source)
        {
            int Out = GL.CreateShader(Source.Type);

            string _stringSource = ShaderCompiler.FillShader(Source.Source);

            GL.ShaderSource(Out, _stringSource);
            GL.CompileShader(Out);

            string Error = GL.GetShaderInfoLog(Out);

            if (Error != "")
            {
                string[] Parts = _stringSource.ToString().Split('\n');

                int i = 0;

                foreach (string part in Parts)
                {
                    Console.WriteLine($@"{i:d4}: {part}");

                    ++i;
                }

                Console.WriteLine("\n" + Error);

                throw new Exception(Error);
            }

            return Out;
        }

        void DeleteShader()
        {
            if (Handle == -1)
                return;

            GL.DeleteProgram(Handle);

            Handle = -1;
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        static List<int> Trash      { get; set; } = new List<int>();
        ~RenderShader()
        {
            if (Handle == -1)
                return;

            Trash.Add(Handle);
        }

        public static void CollectTrash()
        {
            foreach (int handle in Trash)
            {
                GL.DeleteProgram(handle);
            }

            Trash = new List<int>();
        }
    }
}
