using Game_Engine.Managers;
using OpenTK.Input;
using System;

namespace Game_Engine.Controllers
{
    public class KeyboardMouseController : Controller
    {
        public KeyboardMouseController()
        {
        }

        public override float GetBackwardsInput()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Key.Down))
            {
                return 1;
            }
            return 0;
        }

        public override float GetForwardInput()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Key.Up))
            {
                return 1;
            }
            return 0;
        }

        public override float GetLeftInput()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Key.Left))
            {
                return 1;
            }
            MouseState mouseState = Mouse.GetCursorState();
            int mouseX = mouseState.X - SceneManager.mouseCenterPos.X;
            if (mouseX > -100)
            {
                return 0;
            }
            return Math.Min(1, -mouseX / ((float)(SceneManager.width / 2)- 100));
        }

        public override float GetRightInput()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Key.Right))
            {
                return 1;
            }
            MouseState mouseState = Mouse.GetCursorState();
            int mouseX = mouseState.X - SceneManager.mouseCenterPos.X;
            if (mouseX < 100)
            {
                return 0;
            }
            return Math.Min(1, mouseX / ((float)(SceneManager.width / 2) - 100));
        }

        public override string GetControllerType
        {
            get { return "Keyboard and Mouse"; }
        }

        public override MousePosition GetMousePosition()
        {
            MouseState state = Mouse.GetCursorState();
            MousePosition position = new MousePosition();
            position.XPos = (state.X - SceneManager.mouseCenterPos.X) + (SceneManager.width / 2);
            position.YPos = (state.Y - SceneManager.mouseCenterPos.Y) + (SceneManager.height / 2);
            return position;
        }

        public override bool GetEnter()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Key.Enter))
            {
                return true;
            }
            MouseState state = Mouse.GetState();
            return (state.LeftButton == ButtonState.Pressed);
        }
    }
}
