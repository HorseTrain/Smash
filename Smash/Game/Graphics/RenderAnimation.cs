using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Game.Graphics
{
    public struct BoneKeyFrame
    {
        public Vector3 LocalTranslation;
        public Vector3 LocalScale;
        public Quaternion LocalRotation;

        public static BoneKeyFrame Lerp(BoneKeyFrame Left, BoneKeyFrame Right, float lerp)
        {
            return new BoneKeyFrame()
            {
                LocalTranslation = Vector3.Lerp(Left.LocalTranslation, Right.LocalTranslation, lerp),
                LocalScale = Vector3.Lerp(Left.LocalScale, Right.LocalScale, lerp),
                LocalRotation = Quaternion.Slerp(Left.LocalRotation, Right.LocalRotation, lerp)
            };
        }
    }

    public class KeyframeCollection
    {
        public string Name      { get; set; }
        public object[] Data    { get; set; }
        public int Length       => Data.Length;

        public object GetKey(int index)
        {
            if (index < Length)
                return Data[index];

            return Data[Length - 1];
        }
    }

    public class RenderAnimation
    {
        public int FrameCount                                       { get; set; }
        public Dictionary<string, KeyframeCollection> Keyframes     { get; set; } = new Dictionary<string, KeyframeCollection>();

        public bool ContainsCollection(string name) => Keyframes.ContainsKey(name);

        public BoneKeyFrame GetBoneKey(string name, float Time, out bool exists)
        {
            exists = Keyframes.ContainsKey(name);

            if (!exists)
                return new BoneKeyFrame();

            KeyframeCollection collection = Keyframes[name];

            int First = (int)Time;
            int Next = (int)Time + 1;

            if (Next > FrameCount - 1)
            {
                Next = 0;
            }

            BoneKeyFrame FirstKey = (BoneKeyFrame)collection.GetKey(First);
            BoneKeyFrame SecondKey = (BoneKeyFrame)collection.GetKey(Next);

            return BoneKeyFrame.Lerp(FirstKey, SecondKey, Time - (int)Time);
        }

        public bool GetVisibilityKey(string name, float Time)
        {
            return (bool)Keyframes[name].GetKey((int)Time);
        }

        public static void SetTransform(BoneKeyFrame Key, Transform transform)
        {
            transform.Translation = Key.LocalTranslation;
            transform.Rotation = Key.LocalRotation;
            transform.Scale = Key.LocalScale;
        }
    }
}
