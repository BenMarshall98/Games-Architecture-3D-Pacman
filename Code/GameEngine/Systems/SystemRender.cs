using System;
using System.Collections.Generic;
using Game_Engine.Component;
using Game_Engine.Objects;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game_Engine.Systems
{
    public class SystemRender : System
    {
        private Matrix4 mView;
        private Matrix4 mProjection;
        private Vector3 mLightPosition;

        private Camera.aCamera mCamera;

        public SystemRender(Camera.aCamera pCamera)
        {
            mCamera = pCamera;
            MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_DIRECTION | ComponentTypes.COMPONENT_GEOMETRY | ComponentTypes.COMPONENT_TEXTURE | ComponentTypes.COMPONENT_SHADER);
        }

        public override string Name
        {
            get { return "SystemRender"; }
        }

        public override void OnAction()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, mCamera.FrameBuffer);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            mView = mCamera.View;
            mProjection = mCamera.Projection;
            mLightPosition = mCamera.LightPosition();

            foreach (Entity entity in entities)
            {
                if ((entity.Mask & MASK) == MASK)
                {
                    List<IComponent> components = entity.Components;

                    IComponent geometryComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_GEOMETRY;
                    });
                    Geometry geometry = ((ComponentGeometry)geometryComponent).Geometry();

                    IComponent shaderComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_SHADER;
                    });
                    int shader = ((ComponentShader)shaderComponent).Shader().ShaderID;

                    IComponent positionComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                    });

                    IComponent directionComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_DIRECTION;
                    });

                    Vector3 position = ((ComponentPosition)positionComponent).Position;
                    Vector3 direction = ((ComponentDirection)directionComponent).Direction;

                    double angle = Math.Atan2(direction.X, direction.Z);
                    Matrix4 world = Matrix4.CreateRotationY((float)angle);
                    world *= Matrix4.CreateTranslation(position);
                    

                    IComponent textureComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_TEXTURE;
                    });
                    int texture = ((ComponentTexture)textureComponent).Texture;

                    Draw(world, geometry, texture, shader);
                }
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Draw(Matrix4 world, Geometry geometry, int texture, int shader)
        {
            GL.UseProgram(shader);

            int ViewMatLocation = GL.GetUniformLocation(shader, "ViewMat");
            int ModelMatLocation = GL.GetUniformLocation(shader, "ModelMat");
            int ProjMatLocation = GL.GetUniformLocation(shader, "ProjMat");
            int TextureLocation = GL.GetUniformLocation(shader, "sTexture");
            int LightPosLocation = GL.GetUniformLocation(shader, "sLightPos");
            int ViewPosLocation = GL.GetUniformLocation(shader, "sViewPos");
            int LightColorLocation = GL.GetUniformLocation(shader, "sLightColor");

            GL.Uniform1(TextureLocation, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Enable(EnableCap.Texture2D);

            Vector3 viewPos = mCamera.Position;
            Vector3 lightColor = new Vector3(1, 1, 1);

            GL.UniformMatrix4(ViewMatLocation, false, ref mView);
            GL.UniformMatrix4(ProjMatLocation, false, ref mProjection);
            GL.UniformMatrix4(ModelMatLocation, false, ref world);
            GL.Uniform3(LightPosLocation, ref mLightPosition);
            GL.Uniform3(ViewPosLocation, ref viewPos);
            GL.Uniform3(LightColorLocation, ref lightColor);

            geometry.Render();

            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }
    }
}
