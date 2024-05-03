using MathSharp;

namespace RenderSharp.Render3d
{
    internal struct Sample
    {
        public FVec3 hitPoint, hitNormal;
        public double hitDistance;
        public RGBA color;

        public Sample()
        {
            hitPoint = new();
            hitNormal = new();
            hitDistance = double.PositiveInfinity;
            color = new();
        }

        public Sample(in FVec3 hitPoint, in FVec3 hitNormal, double hitDistance, in RGBA color)
        {
            this.hitPoint = hitPoint;
            this.hitNormal = hitNormal;
            this.hitDistance = hitDistance;
            this.color = color;
        }
    }
}
