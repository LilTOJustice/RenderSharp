using MathSharp;

namespace RenderSharp.Render2d
{
    internal class Transforms
    {
        public static FVec2 ScreenToWorld2(Vec2 screenSize, double aspectRatio, Vec2 screenCoords, FVec2 cameraCenter, double cameraZoom, double cameraRotation)
        {
            FVec2 result = (new FVec2(screenCoords.X - (screenSize.X / 2),
                     (screenSize.Y / 2) - screenCoords.Y) / screenSize / cameraZoom + cameraCenter).Rotate(cameraRotation);
            result.X *= aspectRatio;
            return result;
        }

        public static Vec2 WorldToBgTexture2(FVec2 worldCoord, Vec2 bgTextureSize, FVec2 worldRelativeSize)
        {
            FVec2 fromTl = worldCoord - new FVec2(-worldRelativeSize.X / 2, worldRelativeSize.Y / 2);
            Vec2 ind = (Vec2)(fromTl * bgTextureSize);
            return new Vec2(ind.X, bgTextureSize.Y - ind.Y - 1);
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
            FVec2 fromTl = new FVec2(
                actorCoords.X + actorSize.X / 2,
                actorSize.Y - (actorCoords.Y + actorSize.Y / 2)
                );
            Vec2 result = (Vec2)(fromTl / actorSize * textureSize);
            return (result.X < 0
                || result.Y < 0
                || result.X >= textureSize.X
                || result.Y >= textureSize.Y)
                ? null : result;
        }

        public static Vec2 ScreenToStretchBgTexture(Vec2 screenPos, Vec2 res, Vec2 bgTextureSize)
        {
            return (Vec2)((FVec2)screenPos / res * bgTextureSize);
        }
    }
}
