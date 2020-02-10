using Game_Engine.Component;
using Game_Engine.Managers;
using Game_Engine.Maps;
using Game_Engine.Objects;
using OpenTK;
using System;
using System.Collections.Generic;
using Game_Engine.ArtificialIntelligence;

namespace Game_Engine.Systems
{
    class IntelligenceSetUp
    {
        public ArtificialIntelligence.ArtificialIntelligence algorithm;
        public Location ghostLocation;
        public ComponentVelocity velocity;
        public ComponentDirection direction;
        public ComponentPosition position;

        public float fullSpeed;
        public Directions preferredDirection;
        
    }

    public class SystemArtificialInput : System
    {
        Entity player;
        MapData map;
        bool hasMovement = true;

        public SystemArtificialInput(MapData pMap)
        {
            MASK = (ComponentTypes.COMPONENT_ARTIFICAL | ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_DIRECTION | ComponentTypes.COMPONENT_VELOCITY);

            map = pMap;
            ComponentTypes PLAYER_MASK = (ComponentTypes.COMPONENT_INPUT | ComponentTypes.COMPONENT_POSITION);

            EntityManager entityManager = EntityManager.Instance();
            foreach (Entity entity in entityManager.Entities())
            {
                if ((entity.Mask & PLAYER_MASK) == PLAYER_MASK)
                {
                    player = entity;
                    break;
                }
            }
        }

        public override string Name
        {
            get { return "SystemArtificial"; }
        }

        public void Movement()
        {
            hasMovement = !hasMovement;
        }

        public override void OnAction()
        {
            List<IComponent> components = player.Components;

            IComponent positionComponent = components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
            });

            Vector3 positionPlayer = ((ComponentPosition)positionComponent).Position;
            Location playerLocation = new Location();

            playerLocation.mapXPosition = (int)Math.Abs((map.GetStartX() - 0.5f) - positionPlayer.X);
            playerLocation.mapYPosition = (int)Math.Abs((map.GetStartY() - 0.5f) - positionPlayer.Z);

            List<IntelligenceSetUp> setUps = new List<IntelligenceSetUp>();
            
            foreach (Entity entity in entities)
            {
                components = entity.Components;

                positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });

                IComponent directionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_DIRECTION;
                });

                IComponent velocityComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_VELOCITY;
                });

                ComponentVelocity velocity = (ComponentVelocity)velocityComponent;

                ComponentDirection direction = (ComponentDirection)directionComponent;
                direction.DirectionChange = 0;
                velocity.Velocity = 0;

                
                if (!hasMovement)
                {
                    continue;
                }

                if (playerLocation.mapXPosition < 0 || playerLocation.mapXPosition > map.GetWidth() - 1 || playerLocation.mapYPosition < 0 || playerLocation.mapYPosition > map.GetWidth() - 1)
                {
                    //Don't do anything the player is outside the map
                    continue;
                }

                IComponent artificalComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_ARTIFICAL;
                });

                ComponentArtificalInput componentArtifical = (ComponentArtificalInput)artificalComponent;

                Vector3 positionGhost = ((ComponentPosition)positionComponent).Position;

                Location ghostPosition = new Location();

                ghostPosition.mapXPosition = (int)Math.Abs((map.GetStartX() - 0.5f) - positionGhost.X);
                ghostPosition.mapYPosition = (int)Math.Abs((map.GetStartY() - 0.5f) - positionGhost.Z);

                if (ghostPosition.mapXPosition < 0 || ghostPosition.mapXPosition > map.GetWidth() || ghostPosition.mapYPosition < 0 || ghostPosition.mapYPosition > map.GetWidth())
                {
                    //Don't do anything the player is outside the map
                    map.KillGhost(entity);
                    continue;
                }

                IntelligenceSetUp setUp = new IntelligenceSetUp();
                setUp.velocity = velocity;
                setUp.direction = direction;
                setUp.ghostLocation = ghostPosition;
                setUp.algorithm = componentArtifical.GetAlgorithm();
                setUp.preferredDirection = componentArtifical.GetPreferredDirection();
                setUp.position = (ComponentPosition)positionComponent;
                setUp.fullSpeed = componentArtifical.FullSpeed();

                setUps.Add(setUp);            
            }

            for (int i = 0; i < setUps.Count; i++)
            {
                Vector2 ghostMapLocation = new Vector2(setUps[i].position.Position.X, setUps[i].position.Position.Z);

                Vector3 target = setUps[i].algorithm.BuildPath(ghostMapLocation, setUps[i].ghostLocation, map, playerLocation, setUps[i].preferredDirection);
                GoToTarget(setUps[i].direction, setUps[i].velocity, setUps[i].position, target, setUps[i].fullSpeed, positionPlayer);
            }
        }

        private void GoToTarget(ComponentDirection direction, ComponentVelocity velocity, ComponentPosition position, Vector3 target, float fullSpeed, Vector3 playerPosition)
        {            
            Vector3 directionBetween = position.Position - target;
            Vector3 directionGhost = direction.Direction;
            directionBetween.Y = 0.0f;
            directionGhost.Y = 0.0f;

            if (directionBetween.Length < 0.1)
            {
                position.Position = target;
                return;
            }
            Vector3 ghostToPlayer = position.Position - playerPosition;
            ghostToPlayer.Y = 0.0f;
            float ghostDistance = (float)Math.Sqrt(Math.Pow(ghostToPlayer.X, 2) + Math.Pow(ghostToPlayer.Z, 2));
            directionBetween = Vector3.Normalize(directionBetween);
            directionGhost = Vector3.Normalize(directionGhost);
            double angleBetween = Math.Atan2(directionBetween.Z, directionBetween.X);
            double angleGhost = Math.Atan2(directionGhost.Z, directionGhost.X);
            double angle = RadiansToDegree(angleBetween) - RadiansToDegree(angleGhost);

            if (angle > 180)
            {
                angle = 180 - angle;
            }
            else if (angle < -180)
            {
                angle = -180 - angle;
            }

            if (angle > 10)
            {
                direction.DirectionChange = 1 * direction.MaxChange;
            }
            else if (angle < -10)
            {
                direction.DirectionChange = -1 * direction.MaxChange;
            }
            else
            {
                direction.Direction = directionBetween;
                float speed = 1f;
                if((position.Position - target).Length < 1)
                {
                    speed = directionBetween.Length * 0.5f;
                }

                if(ghostDistance < fullSpeed)
                {
                    velocity.Velocity = -1f * speed * velocity.MaxVelocity;
                }
                else
                {
                    velocity.Velocity = -0.5f * speed * velocity.MaxVelocity;
                }
                
            }
        }

        private double RadiansToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}
