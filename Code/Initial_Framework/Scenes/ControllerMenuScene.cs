using Game_Engine.Controllers;
using Game_Engine.Managers;
using Game_Engine.Scenes;
using OpenTK;
using System;
using System.Drawing;

namespace OpenGL_Game.Scenes
{
    class ControllerMenuScene : MenuScene
    {
        private TextureBrush texture;
        private Bitmap bitmap;
        private float showError = 0f;

        public ControllerMenuScene(SceneManager sceneManager) : base(sceneManager, "Controller Options", Color.CornflowerBlue) { }

        public override void Load()
        {
            bitmap = new Bitmap("Textures/ControllerLayout.png");
            

            texture = new TextureBrush(bitmap);
            texture.WrapMode = System.Drawing.Drawing2D.WrapMode.Clamp;

            Button button = new Button();
            button.text = "Keyboard and Mouse";
            buttons.Add(button);

            button = new Button();
            button.text = "Gamepad";
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
            return new Rectangle((int)(width / 16f), (int)((height / 3f) + (5.0f * buttonNumber * fontSize) / 2f), (int)(width / 2f), (int)(fontSize * 2f));
        }
        protected override void ChangeScene()
        {
            if (currentHighlighted == 0)
            {
                InputManager inputManager = InputManager.Instance();
                inputManager.SetController(new KeyboardMouseController());
            }
            else if (currentHighlighted == 1)
            {
                if (OpenTK.Input.GamePad.GetState(0).IsConnected == false)
                {
                    showError = 5f;
                }
                else
                {
                    InputManager inputManager = InputManager.Instance();
                    inputManager.SetController(new GamePadController());
                }
            }
            else if (currentHighlighted == 2)
            {
                sceneManager.ChangeScene(new OptionsMenuScene(sceneManager));
            }
        }

        public override void Render(FrameEventArgs e)
        {
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 30f;

            InputManager inputManager = InputManager.Instance();
            string currentInput = "Current Input Device is set to: " + inputManager.GetControllerType();
            GUI.Label(new Rectangle(0, (int)(height - (2 * fontSize)), (int)width, (int)(fontSize * 2f)), currentInput, (int)fontSize, StringAlignment.Center);

            texture.ResetTransform();
            texture.TranslateTransform((int)(width * 7f / 12f), (int)(height / 4f));
            texture.ScaleTransform((width * 5f / 12f) / bitmap.Width, (height / 1.5f) / bitmap.Height);
            GUI.Texture(new Rectangle((int)(width * 7f / 12f), (int)(height / 4f), (int)(width * 5f / 12f), (int)(height / 1.5f)), texture);
            if (showError > 0)
            {
                GUI.Label(new Rectangle(0, (int)(height - (4 * fontSize)), (int)width, (int)(fontSize)), "Connect a GamePad first before selecting controller setup", (int)(fontSize / 1.5f), StringAlignment.Center);
            }
            else
            {
                showError = 0f;
            }

            base.Render(e);
        }

        public override void Close()
        {
            texture.Dispose();
            base.Close();
        }

        public override void Update(FrameEventArgs e)
        {
            showError -= SceneManager.dt;
            base.Update(e);
        }
    }
}
