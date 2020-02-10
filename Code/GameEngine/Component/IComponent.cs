using System;

namespace Game_Engine.Component
{
    [FlagsAttribute]
    public enum ComponentTypes
    {
        COMPONENT_NONE        = 0,
	    COMPONENT_POSITION    = 1 << 0,
        COMPONENT_GEOMETRY    = 1 << 1,
        COMPONENT_TEXTURE     = 1 << 2,
        COMPONENT_VELOCITY    = 1 << 3,
        COMPONENT_DIRECTION   = 1 << 4,
        COMPONENT_AUDIO       = 1 << 5,
        COMPONENT_SHADER      = 1 << 6,
        COMPONENT_INPUT       = 1 << 7,
        COMPONENT_COLLISION   = 1 << 8,
        COMPONENT_ENTITY_TYPE = 1 << 9,
        COMPONENT_ARTIFICAL   = 1 << 10,
        COMPONENT_ANIMATION   = 1 << 11
    }

    public interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }

        void Delete();
    }
}
