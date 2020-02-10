using Game_Engine.Objects;
using System.Collections.Generic;

namespace Game_Engine.Managers
{
    public class EntityManager
    {
        List<Entity> entityList = new List<Entity>();

        private static EntityManager instance = null;

        private EntityManager()
        {
            Reset();
        }

        public static EntityManager Instance()
        {
            if (instance == null)
            {
                instance = new EntityManager();
            }
            return instance;
        }

        public void Reset()
        {
            for (int i = 0; i < entityList.Count; i++)
            {
                entityList[i].DeleteComponents();
            }
            entityList = new List<Entity>();
        }

        public void AddEntity(Entity entity)
        {
            Entity result = FindEntity(entity.Name);
            //Debug.Assert(result != null, "Entity '" + entity.Name + "' already exists");
            entityList.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            for(int i = 0; i < entityList.Count; i++)
            {
                if(entityList[i] == entity)
                {
                    entityList[i].DeleteComponents();
                    entityList.RemoveAt(i);
                    break;
                }
            }
        }

        private Entity FindEntity(string name)
        {
            return entityList.Find(delegate(Entity e)
            {
                return e.Name == name;
            }
            );
        }

        public List<Entity> Entities()
        {
            return entityList;
        }
    }
}
