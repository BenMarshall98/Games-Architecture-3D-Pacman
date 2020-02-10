using System.Collections.Generic;
using Game_Engine.Component;
using Game_Engine.Managers;
using Game_Engine.Objects;

namespace Game_Engine.Systems
{
    public abstract class System
    {
        protected List<Entity> entities = new List<Entity>();
        protected ComponentTypes MASK;

        public void GetEntities(EntityManager entityManager)
        {
            entities.Clear();
            foreach(Entity entity in entityManager.Entities())
            {
                if ((entity.Mask & MASK) == MASK)
                {
                    entities.Add(entity);
                }
            }
        }
        abstract public void OnAction();

        // Property signatures: 
        abstract public string Name
        {
            get;
        }
    }
}
