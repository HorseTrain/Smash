using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Smash.Core.Graphics;
using Smash.Game;
using Smash.Game.Asset;
using Smash.Game.Fighter;
using Smash.Game.Graphics;
using Smash.Game.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash
{
    public class Entry
    {
        public static void Main()
        {
            /*
            RenderWindow window = new RenderWindow("Smash",500,500);

            ArenaScene scene = new ArenaScene(window);

            scene.LoadFighter(0,0, "falco", 3);

            while (window.UpdateWindow())
            {
                scene.UpdateScene();
            } 
            */

            SlopeForm slope0 = SlopeForm.GetFromLine(Line.FromDirection(new Vector2(-100, 0), new Vector2(0, 10)));
            SlopeForm slope1 = SlopeForm.GetFromLine(Line.FromDirection(new Vector2(10, 0), new Vector2(11, 10)));

            Console.WriteLine(SlopeForm.CalculateIntersection(slope0, slope1, out bool p));
        }
    }
}
