using SmashLabs.Structs;
using SmashLabs.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Animation
{
    public class AnimationNode
    {
        public string Name { get; set; }
        public AnimationTrack[] Tracks { get; set; }
        public static AnimationNode[] ParseAnimationNodes(BufferReader reader)
        {
            BufferArrayPointer pointer = reader.ReadArrayPointer();

            AnimationNode[] Out = new AnimationNode[pointer.ElementCount];

            for (int i = 0; i < Out.Length; i++)
            {
                Out[i] = ParseAnimationNode(reader);
            }

            pointer.End();

            return Out;
        }

        public static AnimationNode ParseAnimationNode(BufferReader reader)
        {
            AnimationNode Out = new AnimationNode();

            Out.Name = reader.ReadStringOffset();

            Out.Tracks = AnimationTrack.ParseAnimationTracks(reader);

            return Out;
        }
    }
}
