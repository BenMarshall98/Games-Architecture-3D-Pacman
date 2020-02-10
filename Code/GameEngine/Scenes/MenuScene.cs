using Game_Engine.Controllers;
using Game_Engine.Managers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Game_Engine.Scenes
{
    public abstract class MenuScene : Scene
    {
        protected List<Button> buttons = new List<Button>();
        private Color color;
        private InputManager inputManager;

        private string title;

        protected int currentHighlighted = 0;
        private const int delay = 20;
        
        private static int inputDelay = 20;
        private static int mouseDelay = 20;

        public MenuScene(SceneManager sceneManager, string pTitle, Color pColor) : base(sceneManager)
        {
            title = pTitle;
            color = pColor;
            inputManager = InputManager.Instance();
        }

        public override void Load()
        {
            //Set the title of the window
            sceneManager.Title = title;
            //Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;

            SetButtonLocation();
        }
        private void SetButtonLocation()
        {
            for (int i = -0; i < buttons.Count; i++)
            {
                Pen pen = new Pen(Brushes.White);
                pen.Width = 4f;

                if (i == currentHighlighted)
                {
                    pen.Width = 8f;
                    pen.Brush = Brushes.Gray;
                }

                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;

                buttons[i].pen = pen;

                float fontSize;
                buttons[i].rect = ButtonLocation(i, out fontSize);
                buttons[i].textSize = fontSize;
            }
        }

        protected abstract Rectangle ButtonLocation(int buttonNumber, out float fontSize);

        protected abstract void ChangeScene();

        public override void Update(FrameEventArgs e)
        {
            GetInput();
            SetButtonLocation();
        }

        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Width, 0, sceneManager.Height, -1, 1);

            GUI.clearColour = color;

            //Display the Title
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 2f)), title, (int)fontSize, StringAlignment.Center);

            for (int i = 0; i < buttons.Count; i++)
            {
                GUI.Button(buttons[i], StringAlignment.Center);
            }

            GUI.Render();
        }

        private void GetInput()
        {
            inputManager.PollKeys();

            if (inputManager.GetEnter())
            {
                if (mouseDelay < delay)
                {
                    mouseDelay++;
                }
                else
                {
                    ChangeScene();
                    mouseDelay = 0;
                }
            }
            else
            {
                mouseDelay = delay;
            }

            MousePosition position = inputManager.GetMousePosition();

            if (position != null)
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    Rectangle rect = buttons[i].rect;

                    if (position.XPos >= rect.Left && position.XPos <= rect.Right && position.YPos >= rect.Top && position.YPos <= rect.Bottom)
                    {
                        currentHighlighted = i;
                        return;
                    }
                }
            }

            float up = inputManager.GetKeyState(Keys.UP);
            float down = inputManager.GetKeyState(Keys.DOWN);

            if (up == 0f && down == 0f)
            {
                inputDelay = delay;
            }

            if (inputDelay < delay)
            {
                inputDelay++;
                return;
            }

            if (up > 0.5f && currentHighlighted > 0)
            {
                currentHighlighted--;
                inputDelay = 0;
            }
            if (down > 0.5f && currentHighlighted < (buttons.Count - 1))
            {
                currentHighlighted++;
                inputDelay = 0;
            }
        }

        public override void Close()
        {
        }
    }
}
