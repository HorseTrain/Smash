using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Core.Graphics
{
    public class RenderTexture
    {
        public int Handle           { get; private set; }
        public byte[] Buffer        { get; set; }
        static List<int> Garbage    { get; set; } = new List<int>();

        ~RenderTexture()
        {
            Garbage.Add(Handle);
        }

        public static void CollectTrash()
        {
            foreach (int handle in Garbage)
            {
                GL.DeleteTexture(handle);
            }

            Garbage = new List<int>();
        }
    }
}
