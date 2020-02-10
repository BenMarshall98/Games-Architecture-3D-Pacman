
namespace Game_Engine.Component
{
    public class ComponentCollisionSquare : IComponent, ICollision
    {
        float width;
        float height;

        public ComponentCollisionSquare(float pWidth, float pHeight)
        {
            width = pWidth;
            height = pHeight;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION; }
        }

        public CollisionTypes CollisionType
        {
            get { return CollisionTypes.COLLISION_SQUARE; }
        }

        public float Width()
        {
            return width;
        }

        public float Height()
        {
            return height;
        }

        public void Delete() { }
    }
}
