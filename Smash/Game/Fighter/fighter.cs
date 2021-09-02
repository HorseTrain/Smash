using OpenTK.Mathematics;
using Smash.Game.Asset;
using Smash.Game.Graphics;
using Smash.Game.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Game.Fighter
{
    public class fighter : GameObject
    {
        ArenaScene arena            { get; set; }
        public Transform Root       => fighterModel.skeleton.RootNode.transform;
        public Model fighterModel   { get; set; }
        public Animator animator    { get; set; } 
        public InputWrapper controller    { get; set; }

        public fighter(Scene scene, string Name, int Id) : base(scene)
        {
            controller = new InputWrapper(scene.window, 1);

            LoadModel(Name, Id);
            LoadAnimations(Name, Id);

            arena = (ArenaScene)scene;

            animator.Play("run");
        }

        public void LoadModel(string name, int id)
        {
            fighterModel = SmashAssetLoader.LoadSmashModel(SmashAssetLoader.GetPath(@$"fighter\{name}\model\body\c0{id}\"));      
        }

        public void LoadAnimations(string name, int id)
        {
            animator = new Animator(fighterModel);

            string[] Files = Directory.GetFiles(SmashAssetLoader.GetPath(@$"fighter\{name}\motion\body\c0{id}\"));

            string GetAnimName(string source)
            {
                string Out = "";

                for (int i = 3; i < source.Length; ++i)
                {
                    Out += source[i];
                }

                return Out;
            }

            foreach (string file in Files)
            {
                if (!file.EndsWith(".nuanmb"))
                    continue;

                string animName = Path.GetFileName(file);

                RenderAnimation animation = SmashAssetLoader.LoadSmashAnimation(file);

                string rname = GetAnimName(animName).Split('.')[0];

                if (animator.Animations.ContainsKey(rname))
                    rname += "_1";

                animator.Animations.Add(rname, animation);
            }
        }

        public override void Update()
        {
            controller.Update();

            fighterModel.skeleton.Buffer.Write(300, arena.camera.GetView(scene.window.Aspect));

            animator.Update();

            fighterModel.GenericDraw();
        }
    }
}
