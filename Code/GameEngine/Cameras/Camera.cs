using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace Game_Engine.Camera
{
    public abstract class aCamera
    {
        protected Matrix4 mView;
        protected Matrix4 mProjection;

        protected Vector3 mPosition;
        protected Vector3 mDirection;
        protected Vector3 mUp;

        protected float width;
        protected float height;

        private int FBO;
        private int texture;
        private int renderBuffer;

        public aCamera(float pWidth, float pHeight, int pFramebuffer = -1)
        {
            width = pWidth;
            height = pHeight;

            if (pFramebuffer != -1)
            {
                FBO = pFramebuffer;
            }
            else
            {
                FBO = GL.GenFramebuffer();
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);

                texture = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, texture);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, (int)width, (int)height,
                    0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                GL.BindTexture(TextureTarget.Texture2D, 0);

                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texture, 0);

                renderBuffer = GL.GenRenderbuffer();
                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderBuffer);
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, (int)width, (int)height);
                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

                GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, renderBuffer);

                if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                {
                    Console.WriteLine("Error occured while creating Framebuffer");
                }

                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            }
            
        }

        public void Close()
        {
            GL.DeleteFramebuffer(FBO);
            GL.DeleteTexture(texture);
            GL.DeleteRenderbuffer(renderBuffer);
        }

        public Vector3 Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }

        public Vector3 Direction
        {
            get { return mDirection; }
            set { mDirection = value; }
        }

        public Vector3 Up
        {
            get { return mUp; }
            set { mUp = value; }
        }

        public Matrix4 View
        {
            get { return mView; }
            set { mView = value; }
        }

        public Matrix4 Projection
        {
            get { return mProjection; }
            set { mProjection = value; }
        }

        public int FrameBuffer
        {
            get { return FBO; }
        }

        public int Texture
        {
            get { return texture; }
        }

        virtual public void Update()
        {
            SetViewProjection();
        }

        abstract public Vector3 LightPosition();

        virtual protected void SetViewProjection()
        {
            mView = Matrix4.LookAt(mPosition, mPosition + mDirection, mUp);
            mProjection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), width / height, 0.5f, 100f);
        }
    }
}
