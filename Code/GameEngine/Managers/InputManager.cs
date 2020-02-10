using Game_Engine.Controllers;

namespace Game_Engine.Managers
{
    public enum Keys
    {
        DOWN,
        UP,
        LEFT,
        RIGHT
    }
    public struct KeyState
    {
        public float State;
    }
    public class InputManager
    {
        private KeyState[] keys;
        private Controller controller;

        private static InputManager instance = null;

        private InputManager()
        {
            Reset();
        }

        public static InputManager Instance()
        {
            if (instance == null)
            {
                instance = new InputManager();
            }
            return instance;
        }

        public void Reset()
        {
            keys = new KeyState[4];
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].State = 0;
            }
        }

        public void SetController(Controller pController)
        {
            controller = pController;
        }

        public void PollKeys()
        {
            keys[(int)Keys.DOWN].State = controller.GetBackwardsInput();
            keys[(int)Keys.UP].State = controller.GetForwardInput();
            keys[(int)Keys.RIGHT].State = controller.GetLeftInput();
            keys[(int)Keys.LEFT].State = controller.GetRightInput();
        }

        public float GetKeyState(Keys key)
        {
            return keys[(int)key].State;
        }

        public MousePosition GetMousePosition()
        {
            return controller.GetMousePosition();
        }

        public bool GetEnter()
        {
            return controller.GetEnter();
        }

        public string GetControllerType()
        {
            return controller.GetControllerType;
        }
    }
}
