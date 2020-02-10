using OpenTK;

namespace Game_Engine.Component
{
    public class ComponentPosition : IComponent
    {
        Vector3 position;
        Vector3 oldPosition;
        Vector3 startPosition;

        public ComponentPosition(float x, float y, float z)
        {
            position = new Vector3(x, y, z);
            oldPosition = position;
            startPosition = new Vector3(x, y, z);
        }

        public ComponentPosition(Vector3 pos)
        {
            position = pos;
            oldPosition = pos;
            startPosition = pos;
        }

        public Vector3 Position
        {
            get { return position; }
            set {
                oldPosition = position;
                position = value;
            }
        }

        public Vector3 StartPosition
        {
            get { return startPosition; }
        }

        public void SwapBack()
        {
            position = oldPosition;
        }
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_POSITION; }
        }

        public void Delete() { }
    }
}
