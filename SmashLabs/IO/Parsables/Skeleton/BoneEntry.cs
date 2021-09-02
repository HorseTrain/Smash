using SmashLabs.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Skeleton
{
    public class BoneEntry
    {
        public string Name { get; set; }
        public short Index { get; set; }
        public short ParentIndex { get; set; }
        public int Type { get; set; }
        public Matrix4 WorldTransform { get; set; }
        public Matrix4 InverseWorldTransform { get; set; }
        public Matrix4 LocalTransform { get; set; }
        public Matrix4 InverseLocalTransform { get; set; }
        public override string ToString()
        {
            return Name + " " + Index + " " + ParentIndex + " " + Type;
        }
    }
}
