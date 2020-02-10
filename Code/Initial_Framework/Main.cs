#region Using Statements
using Game_Engine.Managers;
using OpenGL_Game.Scenes;
using System;
#endregion

namespace OpenGL_Game
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class MainEntry
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new SceneManager())
            {
                game.ChangeScene(new MainMenuScene(game));
                game.Run();
            }
        }
    }
#endif
}
