using MathSharp;
using System.Linq.Expressions;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Actor representing a cube in 3D space.
    /// </summary>
    public class CubeActor : Actor
    {
        private Cube cube;

        internal CubeActor(
            in FVec3 position,
            in FVec3 size,
            in RVec3 rotation,
            Texture texture,
            FragShader fragShader)
            : base(position, size, rotation, texture, fragShader)
        {
            cube = new Cube(position, size, rotation);
        }

        internal override void Sample(in Ray ray, double minDepth, double time, out RGBA sample, out double depth)
        {
            (Cube.Face, Cube.Face) faceCloseFar;
            (double, double) closeFar;
            sample = new RGBA();
            depth = double.PositiveInfinity;
            FVec3 relPosition = Position - ray.origin;
            if (cube.Intersects(ray, minDepth, out closeFar, out faceCloseFar))
            {
                FRGBA fOut;
                FVec2 uvFar = GetUV(faceCloseFar.Item2, (ray.direction * closeFar.Item2 - relPosition).Rotate(Rotation) / Size);
                FragShader(Texture[uvFar], out fOut, (Vec2)(uvFar * Texture.Size), Texture.Size, time);
                sample = ColorFunctions.AlphaBlend(fOut, sample);
                depth = closeFar.Item2;

                if (closeFar.Item1 == double.PositiveInfinity)
                {
                    return;
                }

                FVec2 uvClose = GetUV(faceCloseFar.Item1, (ray.direction * closeFar.Item1 - relPosition).Rotate(Rotation) / Size);
                FragShader(Texture[uvClose], out fOut, (Vec2)(uvClose * Texture.Size), Texture.Size, time);
                sample = ColorFunctions.AlphaBlend(fOut, sample);
                depth = closeFar.Item1;
            }
        }

        internal override Actor Copy()
        {
            return new CubeActor(
                Position,
                Size,
                Rotation,
                Texture,
                FragShader);
        }

        private static FVec2 GetUV(Cube.Face face, in FVec3 fromCenter)
        {
            switch (face)
            {
                case Cube.Face.PosX:
                    return new FVec2(
                        1 - Operations.Mod((fromCenter.Z + 1) / 2, 1),
                        1 - Operations.Mod((fromCenter.Y + 1) / 2, 1));
                case Cube.Face.NegX:
                    return new FVec2(
                        Operations.Mod((fromCenter.Z + 1) / 2, 1),
                        1 - Operations.Mod((fromCenter.Y + 1) / 2, 1));
                case Cube.Face.PosY:
                    return new FVec2(
                        Operations.Mod((fromCenter.X + 1) / 2, 1),
                        1 - Operations.Mod((fromCenter.Z + 1) / 2, 1));
                case Cube.Face.NegY:
                    return new FVec2(
                        1 - Operations.Mod((fromCenter.X + 1) / 2, 1),
                        1 - Operations.Mod((fromCenter.Z + 1) / 2, 1));
                case Cube.Face.PosZ:
                    return new FVec2(
                        1 - Operations.Mod((fromCenter.X + 1) / 2, 1),
                        1 - Operations.Mod((fromCenter.Y + 1) / 2, 1));
                case Cube.Face.NegZ:
                    return new FVec2(
                        Operations.Mod((fromCenter.X + 1) / 2, 1),
                        1 - Operations.Mod((fromCenter.Y + 1) / 2, 1));
                default:
                    return new FVec2();
            }
        }
    }
}
