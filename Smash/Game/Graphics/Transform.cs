using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Game.Graphics
{
    public class Transform
    {
        public int ID               { get; set; }

        public Vector3 Translation  { get; set; }
        public Quaternion Rotation  { get; set; } = Quaternion.Identity;
        public Vector3 Scale        { get; set; } = Vector3.One;

        Transform _parent           { get; set; }
        List<Transform> Children    { get; set; } = new List<Transform>();

        public Matrix4 LocalTransform
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateFromQuaternion(Rotation) * Matrix4.CreateTranslation(Translation);
            }

            set
            {
                Translation = value.ExtractTranslation();
                Rotation = value.ExtractRotation();
                Scale = value.ExtractScale();
            }
        }

        public Matrix4 WorldTransform
        {
            get
            {
                if (Parent == null)
                    return LocalTransform;

                return LocalTransform * Parent.WorldTransform;
            }
        }

        public Transform Parent 
        { 
            get => _parent; 

            set
            {
                if (_parent != null)
                {
                    _parent.Children.Remove(this);
                }

                _parent = value;
                _parent.Children.Add(this);
            }
        }

        public int ChildCount                   => Children.Count;
        public Transform GetChild(int index)    => Children[index];
    }
}
