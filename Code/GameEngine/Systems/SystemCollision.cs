using Game_Engine.Component;
using Game_Engine.Managers;
using OpenTK;
using System;
using System.Collections.Generic;

namespace Game_Engine.Systems
{
    public class SystemCollision : System
    {
        private CollisionManager collisionManager;
        public SystemCollision()
        {
            MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION);
            collisionManager = CollisionManager.Instance();
        }

        public override string Name
        {
            get { return "SystemCollision"; }
        }

        public override void OnAction()
        {
            for(int i = 0; i < entities.Count; i++)
            {
                List<IComponent> componentsOne = entities[i].Components;

                IComponent typeComponent = componentsOne.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_ENTITY_TYPE;
                });

                if(typeComponent == null)
                {
                    continue;
                }

                EntityType entityType = ((ComponentEntityType)typeComponent).Type;

                if (entityType != EntityType.PLAYER && entityType != EntityType.GHOST)
                {
                    continue;
                }

                IComponent positionComponent = componentsOne.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector3 positionOne = ((ComponentPosition)positionComponent).Position;

                IComponent collisionComponent = componentsOne.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_COLLISION;
                });
                ICollision collisionOne = ((ICollision)collisionComponent);

                for(int j = 0; j < entities.Count; j++)
                {
                    if (j == i)
                    {
                        continue;
                    }
                    List<IComponent> componentsTwo = entities[j].Components;

                    positionComponent = componentsTwo.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                    });
                    Vector3 positionTwo = ((ComponentPosition)positionComponent).Position;

                    collisionComponent = componentsTwo.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_COLLISION;
                    });
                    ICollision collisionTwo = ((ICollision)collisionComponent);

                    if(collisionOne.CollisionType == CollisionTypes.COLLISION_CIRCLE && collisionTwo.CollisionType == CollisionTypes.COLLISION_CIRCLE)
                    {
                        CollideCircleCircle(i, positionOne, (ComponentCollisionCircle)collisionOne, j, positionTwo, (ComponentCollisionCircle)collisionTwo);
                    }
                    else if(collisionOne.CollisionType == CollisionTypes.COLLISION_CIRCLE && collisionTwo.CollisionType == CollisionTypes.COLLISION_SQUARE)
                    {
                        CollideCircleSquare(i, positionOne, (ComponentCollisionCircle)collisionOne, j, positionTwo, (ComponentCollisionSquare)collisionTwo);
                    }
                    else if(collisionOne.CollisionType == CollisionTypes.COLLISION_SQUARE && collisionTwo.CollisionType == CollisionTypes.COLLISION_CIRCLE)
                    {
                        CollideCircleSquare(j, positionTwo, (ComponentCollisionCircle)collisionTwo, i, positionOne, (ComponentCollisionSquare)collisionOne);
                    }
                }
            }
        }

        public void CollideCircleCircle(int entityOne, Vector3 positionOne, ComponentCollisionCircle circleOne, int entityTwo, Vector3 positionTwo, ComponentCollisionCircle circleTwo)
        {
            float diffX = positionOne.X - positionTwo.X;
            float diffY = positionOne.Z - positionTwo.Z;

            float minDistance = circleOne.Radius() + circleTwo.Radius();

            float distance = (float)Math.Sqrt(Math.Pow(diffX, 2) + Math.Pow(diffY, 2));

            if (distance < minDistance)
            {
                collisionManager.CollisionDetected(entities[entityOne], entities[entityTwo]);
            }
        }

        public void CollideCircleSquare(int entityOne, Vector3 positionOne, ComponentCollisionCircle circle, int entityTwo, Vector3 positionTwo, ComponentCollisionSquare square)
        {
            float circleX = positionOne.X;
            float circleY = positionOne.Z;
            float rectMinX = positionTwo.X - (square.Width() / 2);
            float rectMaxX = positionTwo.X + (square.Width() / 2);
            float rectMinY = positionTwo.Z - (square.Height() / 2);
            float rectMaxY = positionTwo.Z + (square.Height() / 2);

            float nearestX = Math.Min(rectMaxX, Math.Max(circleX, rectMinX));
            float nearestY = Math.Min(rectMaxY, Math.Max(circleY, rectMinY));
            //float nearestX = Math.Max(positionTwo.X + (square.Width() / 2), Math.Min(positionOne.X, positionTwo.X - (square.Width() / 2)));
            //float nearestY = Math.Max(positionTwo.Z + (square.Height() / 2), Math.Min(positionOne.Z, positionTwo.Z - (square.Height() / 2)));

            float diffX = circleX - nearestX;
            float diffY = circleY - nearestY;

            float distance = (float)Math.Sqrt(Math.Pow(diffX, 2) + Math.Pow(diffY, 2));

            if (distance < circle.Radius())
            {
                collisionManager.CollisionDetected(entities[entityOne], entities[entityTwo]);
            }
        }
    }
}
