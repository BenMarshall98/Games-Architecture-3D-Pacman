
namespace Game_Engine.Component
{
    public enum CollisionTypes
    {
        COLLISION_NONE,
        COLLISION_CIRCLE,
        COLLISION_SQUARE
    }

    public interface ICollision
    {
        CollisionTypes CollisionType
        {
            get;
        }
    }
}
