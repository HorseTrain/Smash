using Smash.Core.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Game
{
    public class Scene
    {
        public List<GameObject> gameObjects     { get; set; } = new List<GameObject>();

        public List<GameObject> destructionQue  { get; set; } = new List<GameObject>();
        public List<GameObject> creationQue     { get; set; } = new List<GameObject>();
        public RenderWindow window              { get; private set; }

        public void SetWindow(RenderWindow window)
        {
            this.window = window;
        }

        void CreateAllObjects()
        {
            foreach (GameObject gameObject in creationQue)
            {
                gameObjects.Add(gameObject);
            }

            creationQue = new List<GameObject>();
        }

        void DestroyAllObjects()
        {
            foreach (GameObject gameObject in destructionQue)
            {
                gameObjects.Remove(gameObject);
            }

            destructionQue = new List<GameObject>();
        }

        public void UpdateScene()
        {
            Update();

            CreateAllObjects();
            DestroyAllObjects();

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update();
            }
        }

        protected virtual void Update()
        {

        }
    }
}
