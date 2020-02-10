using OpenTK;

namespace Game_Engine.Component
{
    public class ComponentDirection : IComponent
    {
        Vector3 direction;
        float maxDirectionChange;
        float directionChange;

        public ComponentDirection(float x, float y, float z, float dy)
        {
            direction = new Vector3(x, y, z);
            maxDirectionChange = dy;
            directionChange = 0;
        }

        public ComponentDirection(Vector3 dir, float dy)
        {
            direction = dir;
            maxDirectionChange = dy;
            directionChange = 0;
        }

        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public float MaxChange
        {
            get { return maxDirectionChange; }
            set { maxDirectionChange = value; }
        }

        public float DirectionChange
        {
            get { return directionChange; }
            set { directionChange = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_DIRECTION; }
        }

        public void Delete() { }
    }
}
