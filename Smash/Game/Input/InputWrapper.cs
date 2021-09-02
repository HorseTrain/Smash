using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Smash.Core.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Game.Input
{
    public enum Mode
    {
        Keyboard,
        Controller,
    }

    public class ControllerDevice
    {
        protected InputWrapper controller { get; set; }

        protected ControllerDevice(InputWrapper controller)
        {
            this.controller = controller;
            controller.Devices.Add(this);
        }

        public virtual void Update()
        {

        }

        public static int SubtractBools(bool neg, bool pos)
        {
            int GetI(bool test)
            {
                return test ? 1 : 0;
            }

            return GetI(pos) - GetI(neg);
        }
    }

    public class Axis : ControllerDevice
    {
        public int AxisID       { get; private set; }
        public float Raw        { get; private set; }

        public Keys Neg         { get; private set; }
        public Keys Pos         { get; private set; }

        public Axis(InputWrapper controller, int AxisID) : base(controller)
        {
            this.AxisID = AxisID;
        }

        public Axis(InputWrapper controller, Keys Neg, Keys Pos) : base(controller)
        {
            this.Neg = Neg;
            this.Pos = Pos;
        }

        public override void Update()
        {
            switch (controller.CurrentMode)
            {
                case Mode.Controller:
                    {
                        Raw = controller.CurrentJoystickState.GetAxis(AxisID);
                    }
                    break;

                case Mode.Keyboard:
                    {
                        Raw = SubtractBools(controller.CurrentKeyboardState.IsKeyDown(Neg), controller.CurrentKeyboardState.IsKeyDown(Pos));
                    }
                    break;
                default: throw new NotImplementedException();
            }
        }
    }

    public class Stick : ControllerDevice
    {
        public Axis X   { get; set; }
        public Axis Y   { get; set; }

        public Stick(InputWrapper controller, int AxisX, int AxisY) : base (controller)
        {
            X = new Axis(controller, AxisX);
            Y = new Axis(controller, AxisY);
        }

        public Stick(InputWrapper controller, Keys xN, Keys xP, Keys yN, Keys yP) : base (controller)
        {
            X = new Axis(controller, xN, xP);
            Y = new Axis(controller, yN, yP);
        }

        public float Mag => new Vector2(X.Raw, Y.Raw).Length;
    }

    public class Button : ControllerDevice
    {
        public bool IsDown      { get; private set; }
        public bool Pressed     { get; private set; }
        public bool Released    { get; private set; }

        public int button       { get; private set; }
        public Keys key         { get; private set; }

        public Button(InputWrapper controller, int Button) : base(controller)
        {
            button = button;
        }

        public Button(InputWrapper controller, Keys key) : base(controller)
        {
            this.key = key;
        }

        bool JoystickLast       { get; set; }

        public override void Update()
        {
            switch (controller.CurrentMode)
            {
                case Mode.Controller:
                    {
                        IsDown = controller.CurrentJoystickState.IsButtonDown(button);
                        Released = !IsDown && JoystickLast;
                        Pressed = IsDown && !JoystickLast;

                        JoystickLast = IsDown;
                    }
                    break;

                case Mode.Keyboard:
                    {
                        IsDown = controller.CurrentKeyboardState.IsKeyDown(key);
                        Pressed = controller.CurrentKeyboardState.IsKeyPressed(key);
                        Released = controller.CurrentKeyboardState.IsKeyReleased(key);
                    }
                    break;
                default: throw new NotImplementedException();
            }
        }
    }

    public class InputWrapper
    {
        public Mode CurrentMode     { get; set; } 
        public RenderWindow Window  { get; set; }

        public List<ControllerDevice> Devices   { get; set; } = new List<ControllerDevice>();

        public Stick StickN     { get; set; }
        public Stick StickC     { get; set; }

        public Button Attack    { get; set; }
        public Button Special   { get; set; }
        public Button Jump      { get; set; }

        public int ControllerID                     { get; set; }
        public JoystickState CurrentJoystickState   { get; set; }
        public KeyboardState CurrentKeyboardState   { get; set; }

        public InputWrapper(RenderWindow window,int ControllerID)
        {
            Window = window;

            this.ControllerID = ControllerID;
            StickN = new Stick(this, 0, 1);
            StickC = new Stick(this, 2, 3);

            Attack = new Button(this, 1);
            Special = new Button(this, 0);
            Jump = new Button(this, 6);

            CurrentMode = Mode.Controller;
        }

        public InputWrapper(RenderWindow window)
        {
            Window = window;

            StickN = new Stick(this,Keys.Left, Keys.Right, Keys.Down, Keys.Up);
            StickC = new Stick(this, Keys.J, Keys.L, Keys.K, Keys.I);

            Attack = new Button(this, Keys.Z);
            Special = new Button(this, Keys.X);
            Jump = new Button(this, Keys.LeftShift);

            CurrentMode = Mode.Keyboard;
        }

        public void Update()
        {
            switch (CurrentMode)
            {
                case Mode.Keyboard: CurrentKeyboardState = Window._tkWindow.KeyboardState; break;
                case Mode.Controller: CurrentJoystickState = Window._tkWindow.JoystickStates[ControllerID]; break;
                default: throw new NotImplementedException();
            }

            foreach (ControllerDevice device in Devices)
            {
                device.Update();
            }
        }
    }
}
