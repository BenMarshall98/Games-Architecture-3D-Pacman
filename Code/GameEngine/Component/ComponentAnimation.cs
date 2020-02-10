
namespace Game_Engine.Component
{
    public enum AnimationType
    {
        ROTATION_Y,
        OSCILLATION_Y
    }
    public class ComponentAnimation : IComponent
    {
        private float period;
        private float maxHeight;
        private float currentTime = 0.0f;
        private AnimationType type;

        public ComponentAnimation(AnimationType pType, float pPeriod, float pMaxHeight = 0.0f)
        {
            type = pType;
            period = pPeriod;
            maxHeight = pMaxHeight;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_ANIMATION; }
        }

        public AnimationType Type
        {
            get { return type; }
        }

        public float Period
        {
            get { return period; }
        }

        public float MaxHeight
        {
            get { return maxHeight; }
        }

        public float CurrentTime
        {
            get { return currentTime; }
            set { currentTime = value; }
        }

        public void Delete() { }
    }
}
