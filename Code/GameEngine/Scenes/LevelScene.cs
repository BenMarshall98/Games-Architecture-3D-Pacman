using Game_Engine.Managers;
using Game_Engine.Scenes;

namespace GameEngine.Scenes
{
    public abstract class LevelScene : Scene
    {
        protected int coinsCollected = 0;
        protected int score = 0;

        public LevelScene(SceneManager sceneManager) : base(sceneManager) { }

        public abstract void CollectCoin();
        public abstract void KillGhostScore();
        public abstract void CollectPowerupScore();
    }
}
