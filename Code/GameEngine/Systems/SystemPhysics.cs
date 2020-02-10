using System;
using System.Collections.Generic;
using OpenTK;
using Game_Engine.Objects;
using Game_Engine.Component;
using Game_Engine.Managers;

namespace Game_Engine.Systems
{
    public class SystemPhysics : System
    {
        public SystemPhysics()
        {
            MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_VELOCITY | ComponentTypes.COMPONENT_DIRECTION);
        }

        public override string Name
        {
            get { return "SystemPhysics"; }
        }

        public override void OnAction()
        {
            foreach (Entity entity in entities)
            {
                if ((entity.Mask & MASK) == MASK)
                {
                    List<IComponent> components = entity.Components;

                    IComponent positionComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                    });

                    IComponent velocityComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_VELOCITY;
                    });

                    float velocity = ((ComponentVelocity)velocityComponent).Velocity;

                    IComponent directionComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_DIRECTION;
                    });

                    Motion((ComponentPosition)positionComponent, velocity, (ComponentDirection)directionComponent);
                }
            }
        }

        public void Motion(ComponentPosition position, float velocity, ComponentDirection direction)
        {
            Matrix3 rot = Matrix3.CreateRotationY(DegreeToRadians(direction.DirectionChange) * SceneManager.dt);
            direction.Direction = rot * direction.Direction;

            position.Position += direction.Direction * velocity * SceneManager.dt;
        }

        public float DegreeToRadians(float angle)
        {
            return angle * (float)(Math.PI / 180.0);
        }
    }
}
