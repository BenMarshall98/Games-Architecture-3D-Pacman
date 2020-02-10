using Game_Engine.ArtificialIntelligence;

namespace Game_Engine.Component
{
    public class ComponentArtificalInput : IComponent
    {
        private ArtificialIntelligence.ArtificialIntelligence algorithm;
        private float distance;
        private Directions preferredDirection;
        
        public ComponentArtificalInput(Directions pPreferredDirection, float fullSpeedDistance, ArtificialIntelligence.ArtificialIntelligence pAlgorithm)
        {
            preferredDirection = pPreferredDirection;
            algorithm = pAlgorithm;
            distance = fullSpeedDistance;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_ARTIFICAL; }
        }

        public void Delete() { }

        public Directions GetPreferredDirection()
        {
            return preferredDirection;
        }

        public ArtificialIntelligence.ArtificialIntelligence GetAlgorithm()
        {
            return algorithm;
        }

        public float FullSpeed()
        {
            return distance;
        }
    }
}
