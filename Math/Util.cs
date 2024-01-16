namespace RenderSharp.Math
{
    public class Util
    {
        public static int Mod(int x, int y)
        {
            return (x % y + y) % y;
        }

        public class Transforms
        {
            public static Vec2 ScreenToWorld2(Vec2 screenSize, Vec2 screenCoords, Vec2 cameraCenter, double cameraZoom, double cameraRotation)
            {
                return (Vec2)(new Vec2(screenCoords.X - (screenSize.X / 2),
                         screenCoords.Y - (screenSize.Y / 2)) / cameraZoom + cameraCenter).Rotate(cameraRotation);
            }

            public static Vec2 WorldToActor2(Vec2 worldCoord, Vec2 actorPosition, double actorRotation)
            {
                return (Vec2)((FVec2)(worldCoord - actorPosition)).Rotate(actorRotation);
            }
        }
    }
}
