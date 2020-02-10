using Game_Engine.Managers;
using Game_Engine.Scenes;
using System;
using System.Drawing;

namespace OpenGL_Game.Scenes
{
    class OptionsMenuScene : MenuScene
    {
        public OptionsMenuScene(SceneManager sceneManager) : base(sceneManager, "Options", Color.CornflowerBlue) { }

        public override void Load()
        {
            Button button = new Button();
            button.text = "Change Controller";
            buttons.Add(button);

            button = new Button();
            button.text = "Change Window Size";
            buttons.Add(button);

            button = new Button();
            button.text = "Back to Main Menu";
            buttons.Add(button);

            base.Load();
        }

        protected override Rectangle ButtonLocation(int buttonNumber, out float fontSize)
        {
            float width = sceneManager.Width, height = sceneManager.Height;
            fontSize = Math.Min(width, height) / 20f;
            return new Rectangle((int)((width / 2f) - (width / 4f)), (int)((height / 3f) + (5.0f * buttonNumber * fontSize) / 2f), (int)(width / 2f), (int)(fontSize * 2f));
        }

        protected override void ChangeScene()
        {
            if (currentHighlighted == 0)
            {
                sceneManager.ChangeScene(new ControllerMenuScene(sceneManager));
            }
            else if (currentHighlighted == 1)
            {
                sceneManager.ChangeScene(new WindowSizeMenu(sceneManager));
            }
            else if (currentHighlighted == 2)
            {
                sceneManager.ChangeScene(new MainMenuScene(sceneManager));
            }
        }
    }
}
