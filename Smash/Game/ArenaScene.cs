using OpenTK.Mathematics;
using Smash.Core.Graphics;
using Smash.Game.Fighter;
using Smash.Game.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Game
{
    public class ArenaScene : Scene
    {
        List<fighter> Fighters                              { get; set; }
        public Camera camera                                { get; private set; }

        public ArenaScene(RenderWindow window)
        {
            SetWindow(window);

            Fighters = new List<fighter>();

            camera = new Camera(CameraType.Orthographic);

            camera.transform.Translation = new Vector3(0,0,10);
        }

        public void LoadFighter(float x, float y, string Name, int id)
        {
            fighter _f = new fighter(this, Name, id);

            _f.Root.Translation = new Vector3(x, y, 0);
            _f.Root.Rotation = Quaternion.FromAxisAngle(Vector3.UnitY, 90.0f / (180 / MathF.PI)) ;
            _f.Root.Scale = Vector3.One;

            Fighters.Add(_f);
        }

        public void SetupCamera(CameraType Type)
        {
            camera = new Camera(Type);
        }

        protected override void Update()
        {
            if (gameObjects.Count >= 1000)
            {
                throw new Exception("Too many gameObjects");
            }


        }
    }
}
