using Game_Engine.Maps;
using OpenTK;
using System.Collections.Generic;

namespace Game_Engine.ArtificialIntelligence
{
    public enum Directions
    {
        None,
        Wall,
        Player,
        Left,
        Right,
        Up,
        Down
    }
    public struct Location
    {
        public int mapXPosition;
        public int mapYPosition;
    }
    public abstract class ArtificialIntelligence
    {
        public ArtificialIntelligence() { }

        public abstract Vector3 BuildPath(Vector2 pGhostLocation, Location pGhostMapLocation, MapData map, Location playerLocation, Directions preferedDirection);
    }
}
