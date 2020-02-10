using System.Collections.Generic;

namespace Game_Engine.Managers
{
    public class SystemManager
    {
        private static SystemManager instance = null;

        List<Systems.System> renderSystemList;
        List<Systems.System> updateSystemList;

        private SystemManager()
        {
            Reset();
        }

        public static SystemManager Instance()
        {
            if (instance == null)
            {
                instance = new SystemManager();
            }
            return instance;
        }

        public void Reset()
        {
            renderSystemList = new List<Systems.System>();
            updateSystemList = new List<Systems.System>();
        }

        public void ActionRenderSystems()
        {
            foreach(Systems.System system in renderSystemList)
            {
                system.OnAction();
            }
        }

        public void ActionUpdateSystems()
        {
            foreach(Systems.System system in updateSystemList)
            {
                system.OnAction();
            }
        }

        public void AddRenderSystem(Systems.System system)
        {
            EntityManager entityManager = EntityManager.Instance();
            renderSystemList.Add(system);
            system.GetEntities(entityManager);
        }

        public void AddUpdateSystem(Systems.System system)
        {
            EntityManager entityManager = EntityManager.Instance();
            updateSystemList.Add(system);
            system.GetEntities(entityManager);
        }

        public void UpdateSystems()
        {
            EntityManager entityManager = EntityManager.Instance();
            foreach (Systems.System system in renderSystemList)
            {
                system.GetEntities(entityManager);
            }

            foreach (Systems.System system in updateSystemList)
            {
                system.GetEntities(entityManager);
            }
        }
    }
}
