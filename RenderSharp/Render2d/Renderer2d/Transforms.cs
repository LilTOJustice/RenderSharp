using MathSharp;

namespace RenderSharp.Render2d
{
    internal class Transforms
    {
        public static FVec2 ScreenToWorld(in Vec2 screenSize, double aspectRatio, in Vec2 screenCoords, in FVec2 cameraCenter, double cameraZoom, double cameraRotation)
        {
            FVec2 result = (new FVec2(screenCoords.X - (screenSize.X / 2),
                     (screenSize.Y / 2) - screenCoords.Y) / screenSize / cameraZoom + cameraCenter).Rotate(new Radian(cameraRotation));
            result.X *= aspectRatio;
            return result;
        }

        public static Vec2 WorldToBgTexture(in FVec2 worldCoord, in Vec2 bgTextureSize, in FVec2 worldRelativeSize)
        {
            FVec2 fromTl = worldCoord - new FVec2(-worldRelativeSize.X / 2, worldRelativeSize.Y / 2);
            Vec2 ind = (Vec2)(fromTl * bgTextureSize);
            return new Vec2(ind.X, bgTextureSize.Y - ind.Y - 1);
        }

        public static bool WorldToActor(in FVec2 worldCoord, in FVec2 actorPosition, in FVec2 actorSize, double actorRotation, out FVec2 actorLoc)
        {
            actorLoc = (worldCoord - actorPosition).Rotate(new Radian(actorRotation));
            return (actorLoc.X >= -actorSize.X / 2
                && actorLoc.Y >= -actorSize.Y / 2
                && actorLoc.X <= actorSize.X / 2
                && actorLoc.Y <= actorSize.Y / 2);
        }

        public static bool ActorToTexture(in FVec2 actorCoords, in FVec2 actorSize, in Vec2 textureSize, out Vec2 textureInd)
        {
            FVec2 fromTl = new FVec2(
                actorCoords.X + actorSize.X / 2,
                actorSize.Y - (actorCoords.Y + actorSize.Y / 2)
                );
            textureInd = (Vec2)(fromTl / actorSize * textureSize);
            return textureInd.X >= 0
                && textureInd.Y >= 0
                && textureInd.X < textureSize.X
                && textureInd.Y < textureSize.Y;
        }

        public static Vec2 ScreenToStretchBgTexture(in Vec2 screenPos, in Vec2 res, in Vec2 bgTextureSize)
        {
            return (Vec2)((FVec2)screenPos / res * bgTextureSize);
        }
    }
}
