
namespace Game_Engine.Component
{
    public class ComponentVelocity : IComponent
    {
        float maxVelocity;
        float velocity;

        public ComponentVelocity(float dVel)
        {
            maxVelocity = dVel;
            velocity = 0;
        }

        public float Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public float MaxVelocity
        {
            get { return maxVelocity; }
            set { maxVelocity = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_VELOCITY; }
        }

        public void Delete() { }
    }
}
