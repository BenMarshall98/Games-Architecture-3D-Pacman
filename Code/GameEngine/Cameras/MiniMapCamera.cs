using OpenTK;

namespace Game_Engine.Camera
{
    public class MiniMapCamera : aCamera
    {
        float mapWidth;
        float mapHeight;

        public MiniMapCamera(float pMapWidth, float pMapHeight, float pWidth, float pHeight, int pFramebuffer = -1) : base(pWidth, pHeight, pFramebuffer)
        {
            mapWidth = pMapWidth;
            mapHeight = pMapHeight;
        }
        protected override void SetViewProjection()
        {
            mView = Matrix4.LookAt(mPosition, mPosition + mDirection, mUp);
            mProjection = Matrix4.CreateOrthographic(mapWidth, mapHeight, -10, 10);
        }

        public override Vector3 LightPosition()
        {
            return new Vector3(0f, 50f, 0f);
        }
    }
}
