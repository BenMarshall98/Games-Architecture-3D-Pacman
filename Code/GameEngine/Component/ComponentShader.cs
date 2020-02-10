using Game_Engine.Managers;
using Game_Engine.Objects;

namespace Game_Engine.Component
{
    public class ComponentShader : IComponent
    {
        Shader shader;

        public ComponentShader(string vertexShader, string fragmentShader)
        {
            shader = ResourceManager.LoadShader(vertexShader, fragmentShader);
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_SHADER; }
        }

        public Shader Shader()
        {
            return shader;
        }

        public void Delete() { }
    }
}
