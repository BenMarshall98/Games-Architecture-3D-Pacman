using Game_Engine.Component;
using Game_Engine.Managers;
using Game_Engine.Objects;
using OpenTK;
using System;
using System.Collections.Generic;

namespace Game_Engine.Systems
{
    public class SystemAnimation : System
    {
        public SystemAnimation()
        {
            MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_DIRECTION | ComponentTypes.COMPONENT_ANIMATION);
        }

        public override string Name
        {
            get { return "SystemAnimation"; }
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

                IComponent directionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_DIRECTION;
                });

                IComponent animationComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_ANIMATION;
                });

                Animation((ComponentPosition)positionComponent, (ComponentDirection)directionComponent, (ComponentAnimation)animationComponent);
            }
        }

        private void Animation(ComponentPosition position, ComponentDirection direction, ComponentAnimation animation)
        {
            float currentTime = animation.CurrentTime;
            float period = animation.Period;
            float dt = SceneManager.dt;
            float timeOver = 0.0f;

            currentTime += dt;

            if (currentTime > period)
            {
                timeOver = (currentTime - period) % period;
                animation.CurrentTime = timeOver;
            }
            else
            {
                animation.CurrentTime = currentTime;
            }

            AnimationType type = animation.Type;

            if (type == AnimationType.ROTATION_Y)
            {
                Matrix3 rot = Matrix3.CreateRotationY(DegreeToRadians(360 / period) * SceneManager.dt);
                direction.Direction = rot * direction.Direction;
            }
            else if (type == AnimationType.OSCILLATION_Y)
            {
                float angle = (currentTime / period) * 360;
                float height = (float)Math.Cos(angle / 180 * Math.PI);
                Vector3 positionV = position.Position;
                positionV.Y = position.StartPosition.Y + animation.MaxHeight * (-(height - 1) / 2f);
                position.Position = positionV;
            }
        }

        public float DegreeToRadians(float angle)
        {
            return angle * (float)(Math.PI / 180.0);
        }
    }
}
