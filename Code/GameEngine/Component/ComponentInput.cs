
namespace Game_Engine.Component
{
    public class ComponentInput : IComponent
    {
        public ComponentInput()
        {
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_INPUT; }
        }

        public void Delete() { }
    }
}
