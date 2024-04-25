using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Actor representing a sphere in 3D space.
    /// </summary>
    public class SphereActor : Actor
    {
        private Sphere sphere;

        internal SphereActor(
            in FVec3 position,
            in FVec3 size,
            in RVec3 rotation,
            Texture texture,
            FragShader fragShader)
            : base(position, size, rotation, texture, fragShader)
        {
            sphere = new Sphere(position, size, rotation);
        }

        internal override void Sample(in Ray ray, double minDepth, double time, out RGBA sample, out double depth)
        {
            (double, double) closeFar;
            sample = new RGBA();
            depth = double.PositiveInfinity;
            FVec3 relPosition = Position - ray.origin;
            if (sphere.Intersects(ray, minDepth, out closeFar))
            {
                FRGBA fOut;
                FVec2 uvFar = GetUV((ray.direction * closeFar.Item2 - relPosition).Rotate(Rotation));
                FragShader(Texture[uvFar], out fOut, (Vec2)(uvFar * Texture.Size), Texture.Size, time);
                sample = ColorFunctions.AlphaBlend(fOut, sample);
                depth = closeFar.Item2;

                if (closeFar.Item1 == double.PositiveInfinity)
                {
                    return;
                }

                FVec2 uvClose = GetUV((ray.direction * closeFar.Item1 - relPosition).Rotate(Rotation));
                FragShader(Texture[uvClose], out fOut, (Vec2)(uvClose * Texture.Size), Texture.Size, time);
                sample = ColorFunctions.AlphaBlend(fOut, sample);
                depth = closeFar.Item1;
            }
        }

        internal override Actor Copy()
        {
            return new SphereActor(
                Position,
                Size,
                Rotation,
                Texture,
                FragShader);
        }

        private static FVec2 GetUV(in FVec3 fromCenter)
        {
            double theta1 = fromCenter.X == 0 ?
                (fromCenter.Y < 0 ? -Math.PI / 2 : Math.PI / 2)
                : Math.Atan(fromCenter.Y / fromCenter.X);
            double theta2 = fromCenter.X == 0 ?
                (fromCenter.Z < 0 ? -Math.PI / 2 : Math.PI / 2)
                : Math.Atan(fromCenter.Z / fromCenter.X);
            return new FVec2(
                (theta2 + Math.PI / 2) / Math.PI,
                (theta1 + Math.PI / 2) / Math.PI);

        }
    }
}
