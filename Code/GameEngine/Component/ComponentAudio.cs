using Game_Engine.Objects;

namespace Game_Engine.Component
{
    public class ComponentAudio : IComponent
    {
        Audio audio;

        public ComponentAudio(string audioName, bool looping)
        {
            audio = new Audio(audioName, looping);
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }

        public Audio Audio()
        {
            return audio;
        }

        public void Delete()
        {
            audio.Delete();
        }
    }
}
