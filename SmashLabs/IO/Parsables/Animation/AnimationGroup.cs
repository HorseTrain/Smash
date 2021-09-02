using SmashLabs.IO.Parsables.Animation.Enums;
using SmashLabs.Structs;
using SmashLabs.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Animation
{
    public class AnimationGroup
    {
        public AnimType AnimationType { get; set; }
        public AnimationNode[] Nodes { get; set; }

        public static AnimationGroup[] ParseAnimationGroups(BufferReader reader)
        {
            BufferArrayPointer pointer = reader.ReadArrayPointer();

            AnimationGroup[] Out = new AnimationGroup[pointer.ElementCount];

            for (int i = 0; i < Out.Length; i++)
            {
                Out[i] = ParseAnimationGroup(reader);
            }

            pointer.End();

            return Out;
        }

        public static AnimationGroup ParseAnimationGroup(BufferReader reader)
        {
            AnimationGroup Out = new AnimationGroup();

            Out.AnimationType = reader.ReadObject<AnimType>();

            Out.Nodes = AnimationNode.ParseAnimationNodes(reader);

            return Out;
        }
    }
}
