using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Actor representing a sphere in 3D space.
    /// </summary>
    public class SphereActor : Actor
    {
        private Sphere sphere;
        FVec3 cameraRelPosition;

        internal SphereActor(
            in FVec3 size,
            in RVec3 rotation,
            in FVec3 position,
            Texture texture,
            FragShader fragShader,
            in FVec3? cameraPos = null)
            : base(size, rotation, position, texture, fragShader)
        {
            cameraRelPosition = position - cameraPos ?? new FVec3();
            sphere = new Sphere(cameraRelPosition, size, rotation);
        }

        internal override void Sample(in FVec3 worldVec, double minDepth, double time, out RGBA sample, out double depth)
        {
            (double, double) closeFar;
            sample = new RGBA();
            depth = double.PositiveInfinity;
            if (sphere.Intersects(worldVec, minDepth, out closeFar))
            {
                FRGBA fOut;
                FVec2 uvFar = GetUV((worldVec * closeFar.Item2 - cameraRelPosition).Rotate(Rotation));
                FragShader(Texture[uvFar], out fOut, (Vec2)(uvFar * Texture.Size), Texture.Size, time);
                sample = ColorFunctions.AlphaBlend(fOut, sample);
                depth = closeFar.Item2;

                if (closeFar.Item1 == double.PositiveInfinity)
                {
                    return;
                }

                FVec2 uvClose = GetUV((worldVec * closeFar.Item1 - cameraRelPosition).Rotate(Rotation));
                FragShader(Texture[uvClose], out fOut, (Vec2)(uvClose * Texture.Size), Texture.Size, time);
                sample = ColorFunctions.AlphaBlend(fOut, sample);
                depth = closeFar.Item1;
            }
        }

        internal override Actor Copy(in FVec3 cameraPos)
        {
            return new SphereActor(
                Size,
                Rotation,
                Position,
                Texture,
                FragShader,
                cameraPos);
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
