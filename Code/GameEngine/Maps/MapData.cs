using Game_Engine.Managers;
using Game_Engine.Objects;

namespace Game_Engine.Maps
{
    public abstract class MapData
    {
        protected int[,] map;

        protected float startX;
        protected float startY;
        private float powerupTime = 0;
        private float powerupDuration;

        protected int width;
        protected int height;
        protected int coinsToCollect = 0;
        protected int lives;

        private bool powerup = false;
       
        public MapData(int pLives, float pPowerupDuration)
        {
            lives = pLives;
            powerupDuration = pPowerupDuration;
        }

        public int[,] GetMap()
        {
            return map;
        }

        public float GetStartX()
        {
            return startX;
        }

        public float GetStartY()
        {
            return startY;
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public abstract void LoadMap(string fileName);

        public abstract Entity GetPlayer();

        public int GetNumberOfCoins()
        {
            return coinsToCollect;
        }

        public int GetNumberOfLives()
        {
            return lives;
        }

        public abstract void LoseLive();

        public void Powerup()
        {
            powerup = true;
            powerupTime = 20f;
        }

        public void Update()
        {
            powerupTime -= SceneManager.dt;
            if (powerupTime < 0)
            {
                powerup = false;
                powerupTime = 0f;
            }
        }

        public bool HasPowerup()
        {
            return powerup;
        }

        public abstract void KillGhost(Entity entity);
    }
}
