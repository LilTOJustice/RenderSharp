using MathSharp;

namespace RenderSharp.Render3d
{
    internal struct Sample
    {
        public readonly FVec3 hitPoint, hitNormal, hitDistance;
        public readonly RGBA color;

        public Sample(FVec3 hitPoint, FVec3 hitNormal, FVec3 hitDistance, RGBA color)
        {
            this.hitPoint = hitPoint;
            this.hitNormal = hitNormal;
            this.hitDistance = hitDistance;
            this.color = color;
        }
    }
}
