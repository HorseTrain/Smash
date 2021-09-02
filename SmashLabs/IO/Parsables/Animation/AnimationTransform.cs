using SmashLabs.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.IO.Parsables.Animation
{
    public struct AnimationTransform
    {
        public Vector3 Scale;
        public Vector4 Rotation;//Quaternion
        public Vector3 Position;
        public float CompensateScale;

        public override string ToString()
        {
            return Scale + "\n" + Rotation + "\n" + Position + "\n" + CompensateScale;
        }
    }
}
