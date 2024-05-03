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

        internal override Sample Sample(in Ray ray, double time)
        {
            (double, double) closeFar;
            FVec3 relPosition = Position - ray.origin;
            if (sphere.Intersects(ray, out closeFar))
            {
                FRGBA fOut;
                FVec2 uvFar = GetUV((ray.direction * closeFar.Item2 - relPosition).Rotate(Rotation));
                FragShader(Texture[uvFar], out fOut, (Vec2)(uvFar * Texture.Size), Texture.Size, time);

                if (closeFar.Item1 == double.PositiveInfinity)
                {
                    FVec3 intersectionBack = ray.origin + ray.direction * closeFar.Item2;
                    return new Sample(
                        intersectionBack,
                        (intersectionBack - Position).Norm(),
                        closeFar.Item2,
                        fOut);
                }

                FVec2 uvClose = GetUV((ray.direction * closeFar.Item1 - relPosition).Rotate(Rotation));
                RGBA back = fOut;
                FragShader(Texture[uvClose], out fOut, (Vec2)(uvClose * Texture.Size), Texture.Size, time);
                FVec3 intersection = ray.origin + ray.direction * closeFar.Item1;
                return new Sample(
                    intersection,
                    (intersection - Position).Norm(),
                    closeFar.Item2,
                    ColorFunctions.AlphaBlend(fOut, back));
            }

            return new();
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
