using OpenTK.Graphics.OpenGL;
using OpenTK;
using Game_Engine.Objects;

namespace Game_Engine.Renderers
{
    public class CubeMapRender
    {
        private Camera.aCamera camera;
        private CubeMapTexture cubemap;
        private Shader shader;
        private Geometry geometry;

        int ViewMatLocation;
        int ProjMatLocation;
        int ModelMatLocation;
        int SkyBoxLocation;

        public CubeMapRender(Camera.aCamera pCamera, CubeMapTexture pCubemap, Shader pShader, Geometry pGeometry)
        {
            camera = pCamera;
            cubemap = pCubemap;
            shader = pShader;
            geometry = pGeometry;
            ViewMatLocation = GL.GetUniformLocation(shader.ShaderID, "ViewMat");
            ProjMatLocation = GL.GetUniformLocation(shader.ShaderID, "ProjMat");
            ModelMatLocation = GL.GetUniformLocation(shader.ShaderID, "ModelMat");
            SkyBoxLocation = GL.GetUniformLocation(shader.ShaderID, "SkyBox");
        }

        public void Render()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, camera.FrameBuffer);
            int texture = cubemap.Texture();

            Matrix4 view = camera.View;
            Matrix4 projection = camera.Projection;
            Matrix4 model = Matrix4.CreateTranslation(camera.Position);

            GL.DepthFunc(DepthFunction.Lequal);

            GL.UseProgram(shader.ShaderID);

            GL.Uniform1(SkyBoxLocation, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, texture);

            GL.UniformMatrix4(ViewMatLocation, false, ref view);
            GL.UniformMatrix4(ProjMatLocation, false, ref projection);
            GL.UniformMatrix4(ModelMatLocation, false, ref model);

            geometry.Render();

            GL.UseProgram(0);

            GL.DepthFunc(DepthFunction.Less);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
    }
}
