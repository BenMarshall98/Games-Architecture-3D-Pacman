
namespace Game_Engine.Controllers
{
    public class MousePosition
    {
        public float XPos;
        public float YPos;
    }
    public abstract class Controller
    {
        abstract public float GetLeftInput();
        abstract public float GetRightInput();
        abstract public float GetForwardInput();
        abstract public float GetBackwardsInput();
        abstract public string GetControllerType { get; }
        abstract public MousePosition GetMousePosition();
        abstract public bool GetEnter();
    }
}
