using Game_Engine.Component;
using Game_Engine.Managers;
using Game_Engine.Objects;
using System.Collections.Generic;

namespace Game_Engine.Systems
{
    public class SystemInput : System
    {
        InputManager input;
        public SystemInput()
        {
            input = InputManager.Instance();
            MASK = (ComponentTypes.COMPONENT_INPUT | ComponentTypes.COMPONENT_VELOCITY | ComponentTypes.COMPONENT_DIRECTION);
        }

        public override string Name
        {
            get { return "SystemInput"; }
        }

        public override void OnAction()
        {
            foreach (Entity entity in entities)
            {
                List<IComponent> components = entity.Components;

                IComponent velocityComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_VELOCITY;
                });

                IComponent directionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_DIRECTION;
                });

                Input((ComponentVelocity)velocityComponent, (ComponentDirection)directionComponent);
            }
        }

        private void Input(ComponentVelocity velocity, ComponentDirection direction)
        {
            if(input.GetKeyState(Keys.DOWN) > 0)
            {
                velocity.Velocity = -(input.GetKeyState(Keys.DOWN) * velocity.MaxVelocity);
            }
            else if(input.GetKeyState(Keys.UP) > 0)
            {
                velocity.Velocity = input.GetKeyState(Keys.UP) * velocity.MaxVelocity;
            }
            else
            {
                velocity.Velocity = 0;
            }

            if(input.GetKeyState(Keys.LEFT) > 0)
            {
                direction.DirectionChange = -(input.GetKeyState(Keys.LEFT) * direction.MaxChange);
            }
            else if(input.GetKeyState(Keys.RIGHT) > 0)
            {
                direction.DirectionChange = input.GetKeyState(Keys.RIGHT) * direction.MaxChange;
            }
            else
            {
                direction.DirectionChange = 0;
            }
        }
    }
}
