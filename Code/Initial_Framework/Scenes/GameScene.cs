using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Misc;
using OpenGL_Game.Maps;
using Game_Engine.Scenes;
using Game_Engine.Camera;
using Game_Engine.Renderers;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Game_Engine.Objects;
using Game_Engine.Component;
using GameEngine.Scenes;

namespace OpenGL_Game.Scenes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class GameScene : LevelScene
    {
        public static int coinsToCollect; 
        public static int lives; 
        EntityCamera entityCamera;
        MiniMapCamera mapCamera;
        //EntityCamera eC2;
        MiniMapRender customRender;
        CubeMapRender cubemapRender;

        EntityManager entityManager;
        SystemManager systemManager;
        InputManager inputManager;
        AudioManager audioManager;

        SystemArtificialInput artificialInput;

        MapLoader map;

        public override void CollectCoin()
        {
            coinsCollected++;
            score += 10;
        }

        public override void KillGhostScore()
        {
            score += 400;
        }

        public override void CollectPowerupScore()
        {
            score += 100;
        }

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            
        }

        public override void Load()
        {
            CollisionManager.Instance().WallCollideReset();

            entityManager = EntityManager.Instance();
            systemManager = SystemManager.Instance();
            inputManager = InputManager.Instance();

            audioManager = AudioManager.Instance();
            audioManager.AddAudio("Death", "Audio/WilhelmScream.wav", false);
            audioManager.AddAudio("Collect", "Audio/CollectCoin.wav", false);
            audioManager.AddAudio("Powerup", "Audio/ElevatorMusic.wav", true);
            audioManager.AddAudio("KillGhost", "Audio/ko.wav", false);

            coinsCollected = 0;

            // Set the title of the window
            sceneManager.Title = "Game";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            // Set Keyboard events to go to a method in this class
            sceneManager.Keyboard.KeyDown += Keyboard_KeyDown;

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            map = new MapLoader();
            map.LoadMap("Maps/Level1.txt");

            mapCamera = new MiniMapCamera(map.GetWidth(), map.GetHeight(), sceneManager.Width, sceneManager.Height);
            entityCamera = new EntityCamera(sceneManager.Width, sceneManager.Height);

            mapCamera.Position = new Vector3(0, 0, 0);
            mapCamera.Direction = new Vector3(0, -1, 0);
            mapCamera.Up = new Vector3(0, 0, -1);

            CollisionManager.Instance().SetMap(map);
            CollisionManager.Instance().SetLevel(this);
            Entity player = map.GetPlayer();
            entityCamera.SetEntity(player);
            audioManager.SetCamera(entityCamera);

            CreateSystems();

            CubeMapTexture cubeMap = ResourceManager.LoadCubeMap("Textures/darkskies_up.png", "Textures/darkskies_dn.png", "Textures/darkskies_lf.png",
                "Textures/darkskies_rt.png", "Textures/darkskies_ft.png", "Textures/darkskies_bk.png");

            Shader shader = ResourceManager.LoadShader("Shaders/cubemapVS.glsl", "Shaders/cubemapFS.glsl");
            Geometry geometry = ResourceManager.LoadGeometry("Geometry/SkyBox.obj");

            cubemapRender = new CubeMapRender(entityCamera, cubeMap, shader, geometry);
            customRender = new MiniMapRender(entityCamera, mapCamera);
        }

        private void CreateSystems()
        {
            SystemManager systemManager = SystemManager.Instance();
            Game_Engine.Systems.System newSystem;

            newSystem = new SystemRender(entityCamera);
            systemManager.AddRenderSystem(newSystem);

            newSystem = new SystemRender(mapCamera);
            systemManager.AddRenderSystem(newSystem);

            newSystem = new SystemInput();
            systemManager.AddUpdateSystem(newSystem);

            newSystem = new SystemPhysics();
            systemManager.AddUpdateSystem(newSystem);

            newSystem = new SystemCollision();
            systemManager.AddUpdateSystem(newSystem);

            newSystem = new SystemArtificialInput(map);
            artificialInput = (SystemArtificialInput)newSystem;
            systemManager.AddUpdateSystem(newSystem);

            newSystem = new SystemAnimation();
            systemManager.AddUpdateSystem(newSystem);

            newSystem = new SystemAudio();
            systemManager.AddUpdateSystem(newSystem);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Update(FrameEventArgs e)
        {
            if (GamePad.GetState(1).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Key.Escape))
                sceneManager.Exit();

            inputManager.PollKeys();
            map.Update();

            systemManager.ActionUpdateSystems();

            entityCamera.Update(); 
            mapCamera.Update();

            audioManager.Update();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Enable(EnableCap.DepthTest);
            
            systemManager.ActionRenderSystems();
            cubemapRender.Render();
            coinsToCollect = map.GetNumberOfCoins();
            lives = map.GetNumberOfLives();

            if (lives == 0)
            {
                sceneManager.ChangeScene(new GameOverScene(sceneManager));
            }

            if (coinsToCollect == coinsCollected)
            {
                audioManager.Delete(true);
                sceneManager.ChangeScene(new WinScene(sceneManager));
            }

            customRender.Render(map, coinsCollected, score);
        }

        /// <summary>
        /// This is called when the game exits.
        /// </summary>
        public override void Close()
        {
            mapCamera.Close();
            //eC2.Close();
            entityCamera.Close();
        }

        public void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    break;
                case Key.Down:
                    break;
                case Key.Escape:
                    sceneManager.Keyboard.KeyDown -= Keyboard_KeyDown;
                    sceneManager.ChangeScene(new MainMenuScene(sceneManager));
                    break;
                case Key.C:
                    CollisionManager.Instance().WallCollide();
                    break;
                case Key.G:
                    artificialInput.Movement();
                    break;
            }
        }

        
    }
}
