using Game_Engine.Component;
using Game_Engine.Managers;
using Game_Engine.Objects;
using OpenTK;
using System.Collections.Generic;

namespace Game_Engine.Systems
{
    public class SystemAudio : System
    {
        AudioManager audioManager = AudioManager.Instance();
        public SystemAudio()
        {
            MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_AUDIO);
        }

        public override string Name
        {
            get { return "SystemAudio"; }
        }

        public override void OnAction()
        {
            foreach (Entity entity in entities)
            {
                List<IComponent> components = entity.Components;

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector3 position = ((ComponentPosition)positionComponent).Position;

                IComponent audioComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });

                Audio audio = ((ComponentAudio)audioComponent).Audio();

                Audio(position, audio);
            }
        }

        public void Audio(Vector3 position, Audio audio)
        {
            audioManager.PlayAudio(audio, position);
        }
    }
}
