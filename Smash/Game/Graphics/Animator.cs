using Smash.Core.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Game.Graphics
{
    public class Animator
    {
        public Dictionary<string, RenderAnimation> Animations   { get; set; }
        public Model model                                      { get; set; }

        public string CurrentAnimationName                      { get; private set; }
        public RenderAnimation CurrentAnimation                 => Animations[CurrentAnimationName];
        float Time                                              { get; set; }

        public Animator(Model model)
        {
            this.model = model;

            Animations = new Dictionary<string, RenderAnimation>();
        }

        public void Play(string name,float CrossFade = 0)
        {
            CurrentAnimationName = name;

        }

        void UpdateTime()
        {
            Time += (float)RenderWindow.MainWindow.DeltaTime;

            while (!(Time < CurrentAnimation.FrameCount))
            {
                Time -= CurrentAnimation.FrameCount;
            }
        }

        public void Update()
        {
            UpdateSkeleton();

            UpdateTime();
        }

        void UpdateSkeleton()
        {
            foreach (Bone bone in model.skeleton.Bones)
            {
                bool HasInfo;

                BoneKeyFrame info = CurrentAnimation.GetBoneKey($"{bone.Name}_Transform", Time, out HasInfo);

                if (!HasInfo)
                    continue;

                bone.transform.Rotation = info.LocalRotation;
                bone.transform.Translation = info.LocalTranslation;
                bone.transform.Scale = info.LocalScale;
            }
        }
    }
}
