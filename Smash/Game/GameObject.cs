using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Game
{
    public class GameObject
    {
        public Scene scene      { get; private set; }

        public GameObject(Scene scene)
        {
            this.scene = scene;

            scene.creationQue.Add(this);
        }

        public virtual void Update()
        {

        }

        public void Destroy()
        {
            scene.destructionQue.Add(this);
        }
    }
}
