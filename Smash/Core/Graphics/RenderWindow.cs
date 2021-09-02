using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Diagnostics;

namespace Smash.Core.Graphics
{
    public class RenderWindow
    {
        public GameWindow _tkWindow     { get; set; }

        public int Width        => _tkWindow.Size.X;
        public int Height       => _tkWindow.Size.Y;
        public float Aspect     => (float)Width / (float)Height;

        public static RenderWindow MainWindow   { get; private set; }
        Stopwatch stopwatch                     { get; set; }

        public double CurrentFrameRate          { get; private set; }
        public double TargetFrameRate           { get; set; } = 60.0f;
        public double DeltaTime
        {
            get
            {
                double Out = TargetFrameRate / CurrentFrameRate;

                if (Out >= 1)
                    return 1;

                return Out;
            }
        }

        public RenderWindow(string name, int Width, int Height)
        {
            GameWindowSettings gameWindowSettings = new GameWindowSettings();
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings();

            nativeWindowSettings.Size = new OpenTK.Mathematics.Vector2i(Width, Height);

            nativeWindowSettings.Profile = ContextProfile.Compatability;

            _tkWindow = new GameWindow(gameWindowSettings, nativeWindowSettings);
            _tkWindow.Context.MakeCurrent();

            _tkWindow.Title = name;

            MainWindow = this;

            stopwatch = new Stopwatch();

            _tkWindow.VSync = VSyncMode.Off;

            GL.Enable(EnableCap.DepthTest);
        }

        public void GetFrameRate()
        {
            stopwatch.Stop();

            double Elapsed = stopwatch.ElapsedTicks;

            CurrentFrameRate = 10000000d / Elapsed;

            stopwatch.Reset();
            stopwatch.Start();

            _tkWindow.Title = CurrentFrameRate.ToString();
        }

        public bool UpdateWindow()
        {
            GetFrameRate();

            RenderMesh.CollectTrash();
            RenderShader.CollectTrash();
            UniformBufferBlock.CollectTrash();
            RenderTexture.CollectTrash();

            if (!_tkWindow.Exists)
                return false;

            _tkWindow.SwapBuffers();

            _tkWindow.Context.MakeCurrent();
            _tkWindow.ProcessEvents();

            GL.Viewport(0,0,Width, Height);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0,0,0,1);

            return true;
        }
    }
}
