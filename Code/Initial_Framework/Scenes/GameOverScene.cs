using Game_Engine.Managers;
using Game_Engine.Scenes;
using System;
using System.Drawing;

namespace OpenGL_Game.Scenes
{
    class GameOverScene : MenuScene
    {
        public GameOverScene(SceneManager sceneManager) : base(sceneManager, "Game Over!", Color.DarkRed) { }

        public override void Load()
        {
            Button button = new Button();
            button.text = "Retry Level";
            buttons.Add(button);

            button = new Button();
            button.text = "Return to Main Menu";
            buttons.Add(button);

            base.Load();
        }

        protected override Rectangle ButtonLocation(int buttonNumber, out float fontSize)
        {
            float width = sceneManager.Width, height = sceneManager.Height;
            fontSize = Math.Min(width, height) / 20f;
            return new Rectangle((int)(width / 4f), (int)((height / 2.5f) + (5.0f * buttonNumber * fontSize) / 2f), (int)(width / 2f), (int)(fontSize * 2f));
        }

        protected override void ChangeScene()
        {
            if (currentHighlighted == 0)
            {
                sceneManager.ChangeScene(new GameScene(sceneManager));
            }
            else if (currentHighlighted == 1)
            {
                sceneManager.ChangeScene(new MainMenuScene(sceneManager));
            }
        }
    }
}
