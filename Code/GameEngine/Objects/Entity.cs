using Game_Engine.Component;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Game_Engine.Objects
{
    public class Entity
    {
        List<IComponent> componentList = new List<IComponent>();
        string name;
        ComponentTypes mask;
 
        public Entity(string name)
        {
            this.name = name;
        }

        /// <summary>Adds a single component</summary>
        public void AddComponent(IComponent component)
        {
            Debug.Assert(component != null, "Component cannot be null");

            componentList.Add(component);
            mask |= component.ComponentType;
        }

        public void DeleteComponents()
        {
            for (int i = 0; i < componentList.Count; i++)
            {
                componentList[i].Delete();
            }
        }

        public String Name
        {
            get { return name; }
        }

        public ComponentTypes Mask
        {
            get { return mask; }
        }

        public List<IComponent> Components
        {
            get { return componentList; }
        }
    }
}
