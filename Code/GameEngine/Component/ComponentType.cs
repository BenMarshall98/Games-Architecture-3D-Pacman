
namespace Game_Engine.Component
{
    public enum EntityType
    {
        NONE,
        PLAYER,
        GHOST,
        COLLECTABLE,
        POWERUP
    }
    public class ComponentEntityType : IComponent
    {
        private EntityType type;

        public ComponentEntityType(EntityType pType)
        {
            type = pType;
        }

        public EntityType Type
        {
            get { return type; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_ENTITY_TYPE; }
        }

        public void Delete() { }
    }
}
