using MathSharp;

namespace RenderSharp.Render3d
{
    internal struct Ray
    {
        public readonly FVec3 origin, direction, inv;

        public Ray(in FVec3 origin, in FVec3 direction)
        {
            this.origin = origin;
            this.direction = direction;
            inv = new FVec3(1 / direction.X, 1 / direction.Y, 1 / direction.Z);
        }
    }
}
