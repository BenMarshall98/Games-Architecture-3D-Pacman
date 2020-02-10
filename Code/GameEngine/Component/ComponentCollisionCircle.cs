
namespace Game_Engine.Component
{
    public class ComponentCollisionCircle : IComponent, ICollision
    {
        float radius;

        public ComponentCollisionCircle(float pRadius)
        {
            radius = pRadius;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION; }
        }

        public CollisionTypes CollisionType
        {
            get { return CollisionTypes.COLLISION_CIRCLE; }
        }

        public float Radius()
        {
            return radius;
        }

        public void Delete() { }
    }
}
