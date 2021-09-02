using OpenTK.Mathematics;
using Smash.Core.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Game.Graphics
{
    public enum CameraType
    {
        Perspective,
        Orthographic
    }

    public class Camera
    {
        public CameraType Type          { get; set; }
        public float FOV                { get; set; } = 30;
        public Transform transform      { get; set; }

        Matrix4 GetViewTransformMatrix() => Matrix4.CreateScale(transform.Scale)* Matrix4.CreateFromQuaternion(transform.Rotation) * Matrix4.CreateTranslation(-transform.Translation);

        public Camera(CameraType Type)
        {
            this.Type = Type;

            transform = new Transform();
        }

        public Matrix4 GetView(float aspect)
        {
            switch (Type)
            {
                case CameraType.Orthographic: return GetViewTransformMatrix() * Matrix4.CreateOrthographicOffCenter(-FOV * aspect, FOV * aspect, -FOV, FOV, 1, 100f); break;
                default: return Matrix4.Identity;
            }
        }
    }
}
