using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Game_Engine.Scenes;
using Game_Engine.Controllers;

namespace Game_Engine.Managers
{
    public class SceneManager : GameWindow
    {
        public static int width = 1600, height = 900;
        public static Point mouseCenterPos;
        public static float dt = 0;

        EntityManager entityManager;
        SystemManager systemManager;
        InputManager inputManager;
        CollisionManager collisionManager;
        AudioManager audioManager;

        Scene scene;
        
        public delegate void SceneDelegate(FrameEventArgs e);
        public SceneDelegate renderer;
        public SceneDelegate updater;

        public SceneManager() : base(width, height, new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(8, 8, 8, 8), 32))
        {
            entityManager = EntityManager.Instance();
            systemManager = SystemManager.Instance();
            inputManager = InputManager.Instance();
            audioManager = AudioManager.Instance();
            inputManager.SetController(new KeyboardMouseController());
            collisionManager = CollisionManager.Instance();
            this.WindowBorder = WindowBorder.Fixed;
            this.Size = new Size(1600, 900);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Enable(EnableCap.DepthTest);

            //Load the GUI
            GUI.SetUpGUI(width, height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            dt = (float)e.Time;

            if (dt > (1f / 5f))
            {
                dt = 0;
            }

            CollisionManager.Instance().Update();
            updater(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            renderer(e);

            GL.Flush();
            SwapBuffers();
        }

        public void ChangeScene(Scene sceneType)
        {
            if (scene != null)
            {
                scene.Close();
            }
            Reset();
            scene = sceneType;
            scene.Load();
        }

        public void Reset()
        {
            entityManager.Reset();
            systemManager.Reset();
            inputManager.Reset();
            collisionManager.Reset();
            audioManager.Reset();
        }

        public static int WindowWidth
        {
            get { return width; }
        }

        public static int WindowHeight
        {
            get { return height; }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            width = Width;
            height = Height;

            Point window = Location;
            window.X = (DisplayDevice.Default.Width - width) / 2;
            window.Y = (DisplayDevice.Default.Height - height) / 2;
            Location = window;

            Point center = new Point(width / 2, height / 2);
            mouseCenterPos = PointToScreen(center);
            //Load the GUI
            GUI.SetUpGUI(Width, Height);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            audioManager.Delete();
            ResourceManager.ClearResources();
            scene.Close();
        }
    }

}

