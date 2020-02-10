using OpenTK;
using System.Collections.Generic;
using Game_Engine.Component;
using Game_Engine.Objects;

namespace Game_Engine.Camera
{
    public class EntityCamera : aCamera
    {
        private Entity mEntity;

        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_DIRECTION);

        public EntityCamera(float pWidth, float pHeight, int pFramebuffer = -1) : base(pWidth, pHeight, pFramebuffer)
        {
            Up = -Vector3.UnitY;
        }

        public void SetEntity(Entity pEntity)
        {
            if ((pEntity.Mask & MASK) == MASK)
            {
                mEntity = pEntity;
            }
        }

        public override void Update()
        {
            List<IComponent> components = mEntity.Components;

            IComponent positionComponent = components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
            });

            mPosition = ((ComponentPosition)positionComponent).Position;

            IComponent directionComponent = components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_DIRECTION;
            });

            mDirection = ((ComponentDirection)directionComponent).Direction;

            SetViewProjection();
        }

        public override Vector3 LightPosition()
        {
            Vector3 position = mPosition - (5 * mDirection);
            return new Vector3(position.X, 15f, position.Z);
        }
        
    }
}
