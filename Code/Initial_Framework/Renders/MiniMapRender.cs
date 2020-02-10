using Game_Engine.Camera;
using Game_Engine.Objects;
using Game_Engine.Managers;
using OpenGL_Game.Scenes;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using Game_Engine.Scenes;
using Game_Engine.Maps;

namespace OpenGL_Game.Misc
{
    class MiniMapRender
    {
        aCamera[] cameras = new aCamera[2];
        Geometry mScreen;
        Shader screenShader;

        float size = 0.4f;
        float heartSize = 0.04f;
        float gapSize = 1.0f;

        int heartTexture;
        int steelHeartTexture;
        int coinTexture;
        int textureLocation;
        int modelMatrixLocation;

        public MiniMapRender(aCamera screen, aCamera minimap)
        {
            cameras[0] = screen;
            cameras[1] = minimap;

            mScreen = ResourceManager.LoadGeometry("Geometry/SquareGeometry.txt");

            screenShader = ResourceManager.LoadShader("Shaders/minimapVertex.glsl", "Shaders/minimapFragment.glsl");
            heartTexture = ResourceManager.LoadTexture("Textures/Heart.png");
            steelHeartTexture = ResourceManager.LoadTexture("Textures/SteelHeart.png");
            coinTexture = ResourceManager.LoadTexture("Textures/MiniMapCoin.png");
            textureLocation = GL.GetUniformLocation(screenShader.ShaderID, "texture");
            modelMatrixLocation = GL.GetUniformLocation(screenShader.ShaderID, "modelMat");
        }

        public void Render(MapData map, int coinsCollected, int score)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Disable(EnableCap.DepthTest);
            Matrix4 model = Matrix4.Identity;
            
            GL.UseProgram(screenShader.ShaderID);

            GL.Uniform1(textureLocation, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, cameras[0].Texture);
            GL.Enable(EnableCap.Texture2D);

            GL.UniformMatrix4(modelMatrixLocation, false, ref model);

            mScreen.Render();

            Vector3 scaleMiniMap;
            Vector3 positionMiniMap;

            float gap;

            if (SceneManager.height > SceneManager.width)
            {
                gap = (float)SceneManager.width * size;

                float heightY = (SceneManager.height - (gapSize * gap)) / SceneManager.height;

                float scaleY = gap / SceneManager.height;

                scaleMiniMap = new Vector3(size, scaleY, 0);
                positionMiniMap = new Vector3(1 - (gapSize * size), heightY, 0);
            }
            else
            {
                gap = (float)SceneManager.height * size;

                float widthX = (SceneManager.width - (gapSize * gap)) / SceneManager.width;

                float scaleX = gap / SceneManager.width;

                scaleMiniMap = new Vector3(scaleX, size, 0);
                positionMiniMap = new Vector3(widthX, 1 - (gapSize * size), 0);
            }

            model = Matrix4.CreateScale(scaleMiniMap) * Matrix4.CreateTranslation(positionMiniMap);

            GL.UniformMatrix4(modelMatrixLocation, false, ref model);

            GL.BindTexture(TextureTarget.Texture2D, cameras[1].Texture);
            mScreen.Render();

            for (int i = 0; i < GameScene.lives; i++)
            {
                if (map.HasPowerup())
                {
                    RenderShape(i, steelHeartTexture);
                }
                else
                {
                    RenderShape(i, heartTexture);
                }
            }

            RenderShape(4, coinTexture);

            GL.BindVertexArray(0);
            GL.UseProgram(0);

            float width = SceneManager.width, height = SceneManager.height, fontSize = Math.Min(width, height) / 35f;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, width, 0, height, -1, 1);
            GUI.clearColour = Color.Transparent;
            string label = coinsCollected.ToString() + " / " + GameScene.coinsToCollect.ToString();
            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)(2 * width - (gapSize - 0.6f) * gap), (int)((gapSize + 1.06f) * gap)), label, (int)fontSize, StringAlignment.Center);

            label = "Score: " + score;
            fontSize *= 2;
            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)(width), (int)(fontSize * 2f)), label, (int)fontSize, StringAlignment.Center, Color.Black);
            GUI.Render();
        }

        private void RenderShape(float heart, int texture)
        {
            Vector3 scaleHeart;
            Vector3 positionHeart;

            float gap;

            if (SceneManager.height > SceneManager.width)
            {
                gap = (float)SceneManager.width * size;
                float heartGap = (float)SceneManager.width * heartSize;

                float heightY = (SceneManager.height - ( 2.0f * gapSize * gap - (2.5f * heart * heartGap) - heartGap)) / SceneManager.height;

                float scaleY = heartGap / SceneManager.height;

                scaleHeart = new Vector3(heartSize, scaleY, 0);
                positionHeart = new Vector3(1 - (2.0f * gapSize * size) - (1.1f * heartSize), heightY, 0);
            }
            else
            {
                gap = (float)SceneManager.height * size;
                float heartGap = (float)SceneManager.height * heartSize;

                float widthX = (SceneManager.width - (2.0f * gapSize * gapSize * gap - (2.5f * heart * heartGap) - heartGap)) / SceneManager.width;

                float scaleX = heartGap / SceneManager.width;

                scaleHeart = new Vector3(scaleX, heartSize, 0);
                positionHeart = new Vector3(widthX, 1 - (2.0f * gapSize * size) - (1.1f * heartSize), 0);
            }

            Matrix4 model = Matrix4.CreateScale(scaleHeart) * Matrix4.CreateTranslation(positionHeart);

            GL.UniformMatrix4(modelMatrixLocation, false, ref model);

            GL.BindTexture(TextureTarget.Texture2D, texture);
            mScreen.Render();
        }
    }
}
