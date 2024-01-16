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

            public static FVec2? WorldToActor2(FVec2 worldCoord, FVec2 actorPosition, FVec2 actorSize, double actorRotation)
            {
                FVec2 result = (worldCoord - actorPosition).Rotate(actorRotation);
                return (result.X < -actorSize.X / 2
                    || result.Y < -actorSize.Y / 2
                    || result.X > actorSize.X / 2
                    || result.Y > actorSize.Y / 2)
                    ? null : result;
            }

            public static Vec2? ActorToTexture2(FVec2 actorCoords, FVec2 actorSize, Vec2 textureSize)
            {
                FVec2 fromTl = actorCoords + new FVec2(actorSize.X / 2, actorSize.Y / 2);
                Vec2 result = (Vec2)(fromTl / actorSize * textureSize);
                return (result.X < 0
                    || result.Y < 0
                    || result.X >= textureSize.X
                    || result.Y >= textureSize.Y)
                    ? null : result;
            }
        }
    }
}
