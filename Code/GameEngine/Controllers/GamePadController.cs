using OpenTK;
using OpenTK.Input;

namespace Game_Engine.Controllers
{
    public class GamePadController : Controller
    {
        public override float GetBackwardsInput()
        {
            GamePadState gamepadState = GamePad.GetState(0);
            Vector2 thumbState = gamepadState.ThumbSticks.Left;
            if(thumbState.Y > -0.1)
            {
                return 0;
            }
            return -thumbState.Y;
        }

        public override float GetForwardInput()
        {
            GamePadState gamepadState = GamePad.GetState(0);
            Vector2 thumbState = gamepadState.ThumbSticks.Left;
            if (thumbState.Y < 0.1)
            {
                return 0;
            }
            return thumbState.Y;
        }

        public override float GetLeftInput()
        {
            GamePadState gamepadState = GamePad.GetState(0);
            Vector2 thumbState = gamepadState.ThumbSticks.Right;
            if (thumbState.X > -0.1)
            {
                return 0;
            }
            return -thumbState.X;
        }

        public override float GetRightInput()
        {
            GamePadState gamepadState = GamePad.GetState(0);
            Vector2 thumbState = gamepadState.ThumbSticks.Right;
            if (thumbState.X < 0.1)
            {
                return 0;
            }
            return thumbState.X;
        }

        public override MousePosition GetMousePosition()
        {
            return null;
        }

        public override string GetControllerType
        {
            get { return "Gamepad"; }
        }

        public override bool GetEnter()
        {
            ButtonState state = GamePad.GetState(0).Buttons.A;
            return (state == ButtonState.Pressed);
        }
    }
}
