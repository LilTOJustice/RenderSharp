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
            public static FVec2 ScreenToWorld2(Vec2 screenSize, Vec2 screenCoords, Vec2 cameraCenter, double cameraZoom, double cameraRotation)
            {
                return (new Vec2(screenCoords.X - (screenSize.X / 2),
                         screenCoords.Y - (screenSize.Y / 2)) / cameraZoom + cameraCenter).Rotate(cameraRotation);
            }

            public static FVec2 WorldToActor2(FVec2 worldCoord, FVec2 actorPosition, double actorRotation)
            {
                return (worldCoord - actorPosition).Rotate(actorRotation);
            }

            public static Vec2 ActorToTexture2(FVec2 actorCoords, FVec2 actorSize, Vec2 textureSize)
            {
                FVec2 actorTl = actorCoords + new FVec2(actorSize.X / 2, actorSize.Y / 2);
                return (Vec2)(actorTl / actorSize * textureSize);
            }
        }
    }
}
