using Game_Engine.Managers;
using Game_Engine.Scenes;
using System;
using System.Drawing;

namespace OpenGL_Game.Scenes
{
    class MainMenuScene : MenuScene
    {
        public MainMenuScene(SceneManager sceneManager) : base(sceneManager, "Main Menu", Color.CornflowerBlue) { }

        public override void Load()
        {
            Button button = new Button();
            button.text = "New Game";
            buttons.Add(button);

            button = new Button();
            button.text = "Options";
            buttons.Add(button);

            button = new Button();
            button.text = "Exit Game";
            buttons.Add(button);

            base.Load();
        }
        protected override Rectangle ButtonLocation(int buttonNumber, out float fontSize)
        {
            float width = sceneManager.Width, height = sceneManager.Height;
            fontSize = Math.Min(width, height) / 20f;
            return new Rectangle((int)((width / 2f) - (width / 8f)), (int)((height / 3f) + (5.0f * buttonNumber * fontSize) / 2f), (int)(width / 4f), (int)(fontSize * 2f));
        }
        
        protected override void ChangeScene()
        {
            if (currentHighlighted == 0)
            {
                sceneManager.ChangeScene(new GameScene(sceneManager));
            }
            else if (currentHighlighted == 1)
            {
                sceneManager.ChangeScene(new OptionsMenuScene(sceneManager));
            }
            else if (currentHighlighted == 2)
            {
                sceneManager.Exit();
            }
        }
    }
}