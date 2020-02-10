using Game_Engine.Managers;
using Game_Engine.Scenes;
using OpenTK;
using System;
using System.Drawing;

namespace OpenGL_Game.Scenes
{
    class WindowSizeMenu : MenuScene
    {
        public WindowSizeMenu(SceneManager sceneManager) : base(sceneManager, "Adjust Window Size", Color.CornflowerBlue) { }

        public override void Load()
        {
            Button button = new Button();
            button.text = "1280 x 720";
            buttons.Add(button);

            button = new Button();
            button.text = "1600 x 900";
            buttons.Add(button);

            button = new Button();
            button.text = "1920 x 1080";
            buttons.Add(button);

            button = new Button();
            button.text = "Fullscreen";
            buttons.Add(button);

            button = new Button();
            button.text = "Back to Options";
            buttons.Add(button);

            base.Load();
        }
        protected override Rectangle ButtonLocation(int buttonNumber, out float fontSize)
        {
            float width = sceneManager.Width, height = sceneManager.Height;
            fontSize = Math.Min(width, height) / 20f;
            return new Rectangle((int)(width / 16f), (int)((height / 4f) + (5.0f * buttonNumber * fontSize) / 2f), (int)(width / 2f), (int)(fontSize * 2f));
        }

        protected override void ChangeScene()
        {
            if (currentHighlighted == 0)
            {
                sceneManager.WindowState = WindowState.Normal;
                sceneManager.Size = new Size(1280, 720);
            }
            else if (currentHighlighted == 1)
            {
                sceneManager.WindowState = WindowState.Normal;
                sceneManager.Size = new Size(1600, 900);
            }
            else if (currentHighlighted == 2)
            {
                sceneManager.WindowState = WindowState.Normal;
                sceneManager.Size = new Size(1920, 1080);
            }
            else if (currentHighlighted == 3)
            {
                sceneManager.WindowState = WindowState.Fullscreen;
            }
            else if (currentHighlighted == 4)
            {
                sceneManager.ChangeScene(new OptionsMenuScene(sceneManager));
            }
        }

        public override void Render(FrameEventArgs e)
        {
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 30f;

            InputManager inputManager = InputManager.Instance();
            string currentInput = "Current Window Size is set to: ";

            if (sceneManager.WindowState == WindowState.Fullscreen)
            {
                currentInput += "Fullscreen";
            }
            else
            {
                currentInput += sceneManager.Size.Width + " x " + sceneManager.Size.Height;
            }
            GUI.Label(new Rectangle(0, (int)(height - (2 * fontSize)), (int)width, (int)(fontSize * 2f)), currentInput, (int)fontSize, StringAlignment.Center);

            base.Render(e);
        }
    }
}
