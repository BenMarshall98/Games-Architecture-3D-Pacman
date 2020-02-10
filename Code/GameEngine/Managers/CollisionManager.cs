using Game_Engine.Component;
using Game_Engine.Maps;
using Game_Engine.Objects;
using GameEngine.Scenes;
using System.Collections.Generic;

namespace Game_Engine.Managers
{
    public class CollisionManager
    {
        private List<Entity> toDestroy;
        private MapData map;
        private LevelScene level;
        private bool wallCollide = true;

        private static CollisionManager instance = null;

        public CollisionManager()
        {
            Reset();
        }

        public static CollisionManager Instance()
        {
            if (instance == null)
            {
                instance = new CollisionManager();
            }
            return instance;
        }

        public void SetMap(MapData pMap)
        {
            map = pMap;
        }

        public void SetLevel(LevelScene pLevel)
        {
            level = pLevel;
        }

        public void WallCollide()
        {
            wallCollide = !wallCollide;
        }

        public void WallCollideReset()
        {
            wallCollide = true;
        }

        public void Update()
        {
            EntityManager entityManager = EntityManager.Instance();

            for(int i = 0; i < toDestroy.Count; i++)
            {
                List<IComponent> components = toDestroy[i].Components;

                IComponent entityTypeComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_ENTITY_TYPE;
                });

                EntityType type = (entityTypeComponent != null) ? ((ComponentEntityType)entityTypeComponent).Type : EntityType.NONE;

                if(type == EntityType.COLLECTABLE)
                {
                    level.CollectCoin();
                }
                else if (type == EntityType.POWERUP)
                {
                    level.CollectPowerupScore();
                }

                entityManager.RemoveEntity(toDestroy[i]);
            }

            if(toDestroy.Count > 0)
            {
                SystemManager.Instance().UpdateSystems();
            }

            toDestroy.Clear();
        }

        public void Reset()
        {
            toDestroy = new List<Entity>();
        }

        public void CollisionDetected(Entity entityOne, Entity entityTwo)
        {
            List<IComponent> componentsOne = entityOne.Components;

            IComponent entityTypeComponent = componentsOne.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_ENTITY_TYPE;
            });

            EntityType typeOne = (entityTypeComponent != null) ? ((ComponentEntityType)entityTypeComponent).Type : EntityType.NONE;

            List<IComponent> componentsTwo = entityTwo.Components;

            entityTypeComponent = componentsTwo.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_ENTITY_TYPE;
            });

            EntityType typeTwo = (entityTypeComponent != null) ? ((ComponentEntityType)entityTypeComponent).Type : EntityType.NONE;

            if (typeOne == EntityType.NONE || typeTwo == EntityType.NONE)
            {
                if (!wallCollide && (typeOne == EntityType.PLAYER || typeTwo == EntityType.PLAYER))
                {
                    return;
                }
                IComponent positionComponent = componentsOne.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });

                ((ComponentPosition)positionComponent).SwapBack();

                positionComponent = componentsTwo.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });

                ((ComponentPosition)positionComponent).SwapBack();
            }
            //else if (typeOne == EntityType.GHOST && typeTwo == EntityType.GHOST)
            //{
            //    IComponent positionComponent = componentsOne.Find(delegate (IComponent component)
            //    {
            //        return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
            //    });

            //    ((ComponentPosition)positionComponent).SwapBack();

            //    positionComponent = componentsTwo.Find(delegate (IComponent component)
            //    {
            //        return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
            //    });

            //    ((ComponentPosition)positionComponent).SwapBack();
            //}
            else if (typeOne == EntityType.PLAYER && typeTwo == EntityType.COLLECTABLE)
            {
                toDestroy.Add(entityTwo);
                AudioManager.Instance().PlayAudio("Collect");
            }
            else if (typeOne == EntityType.COLLECTABLE && typeTwo == EntityType.PLAYER)
            {
                toDestroy.Add(entityOne);
                AudioManager.Instance().PlayAudio("Collect");
            }
            else if (typeOne == EntityType.PLAYER && typeTwo == EntityType.POWERUP)
            {
                toDestroy.Add(entityTwo);
                AudioManager.Instance().PlayAudio("Powerup");
                map.Powerup();
            }
            else if (typeOne == EntityType.POWERUP && typeTwo == EntityType.PLAYER)
            {
                toDestroy.Add(entityOne);
                AudioManager.Instance().PlayAudio("Powerup");
                map.Powerup();
            }
            else if (typeOne == EntityType.PLAYER && typeTwo == EntityType.GHOST)
            {
                if (map.HasPowerup())
                {
                    AudioManager.Instance().PlayAudio("KillGhost");
                    level.KillGhostScore();
                    map.KillGhost(entityTwo);
                }
                else
                {
                    AudioManager.Instance().PlayAudio("Death");
                    map.LoseLive();
                }

            }
            else if (typeOne == EntityType.GHOST && typeTwo == EntityType.PLAYER)
            {
                if (map.HasPowerup())
                {
                    AudioManager.Instance().PlayAudio("KillGhost");
                    level.KillGhostScore();
                    map.KillGhost(entityOne);
                }
                else
                {
                    AudioManager.Instance().PlayAudio("Death");
                    map.LoseLive();
                }
            }
        }
    }
}
