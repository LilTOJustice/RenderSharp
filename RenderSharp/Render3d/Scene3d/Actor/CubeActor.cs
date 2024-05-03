using MathSharp;
using System.Linq.Expressions;
using System.Security.AccessControl;

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

        internal override Sample Sample(in Ray ray, double time)
        {
            (Cube.Face, Cube.Face) faceCloseFar;
            (double, double) closeFar;
            FVec3 relPosition = Position - ray.origin;
            if (cube.Intersects(ray, out closeFar, out faceCloseFar))
            {
                FRGBA fOut;
                FVec2 uvFar = GetUV(faceCloseFar.Item2, (ray.direction * closeFar.Item2 - relPosition).Rotate(Rotation) / Size);
                FragShader(Texture[uvFar], out fOut, (Vec2)(uvFar * Texture.Size), Texture.Size, time);

                if (closeFar.Item1 == double.PositiveInfinity)
                {
                    return new Sample(
                        ray.origin + ray.direction * closeFar.Item2,
                        GetNormal(faceCloseFar.Item2),
                        closeFar.Item2,
                        fOut);
                }

                FVec2 uvClose = GetUV(faceCloseFar.Item1, (ray.direction * closeFar.Item1 - relPosition).Rotate(Rotation) / Size);
                RGBA back = fOut;
                FragShader(Texture[uvClose], out fOut, (Vec2)(uvClose * Texture.Size), Texture.Size, time);
                return new Sample(
                    ray.origin + ray.direction * closeFar.Item1,
                    GetNormal(faceCloseFar.Item1),
                    closeFar.Item2,
                    ColorFunctions.AlphaBlend(fOut, back));
            }

            return new();
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

        private static FVec3 GetNormal(Cube.Face face)
        {
            switch (face)
            {
                case Cube.Face.PosX:
                    return new FVec3(1, 0, 0);
                case Cube.Face.NegX:
                    return new FVec3(-1, 0, 0);
                case Cube.Face.PosY:
                    return new FVec3(0, 1, 0);
                case Cube.Face.NegY:
                    return new FVec3(0, -1, 0);
                case Cube.Face.PosZ:
                    return new FVec3(0, 0, 1);
                case Cube.Face.NegZ:
                    return new FVec3(0, 0, -1);
                default:
                    return new FVec3();
            }
        }

        private static FVec2 GetUV(Cube.Face face, in FVec3 fromCenter)
        {
            switch (face)
            {
                case Cube.Face.PosX:
                    return new FVec2(
                        1 - Operations.Mod((fromCenter.Z + 1) / 2, 1),
                        Operations.Mod((fromCenter.Y + 1) / 2, 1));
                case Cube.Face.NegX:
                    return new FVec2(
                        Operations.Mod((fromCenter.Z + 1) / 2, 1),
                        Operations.Mod((fromCenter.Y + 1) / 2, 1));
                case Cube.Face.PosY:
                    return new FVec2(
                        Operations.Mod((fromCenter.X + 1) / 2, 1),
                        Operations.Mod((fromCenter.Z + 1) / 2, 1));
                case Cube.Face.NegY:
                    return new FVec2(
                        1 - Operations.Mod((fromCenter.X + 1) / 2, 1),
                        Operations.Mod((fromCenter.Z + 1) / 2, 1));
                case Cube.Face.PosZ:
                    return new FVec2(
                        1 - Operations.Mod((fromCenter.X + 1) / 2, 1),
                        Operations.Mod((fromCenter.Y + 1) / 2, 1));
                case Cube.Face.NegZ:
                    return new FVec2(
                        Operations.Mod((fromCenter.X + 1) / 2, 1),
                        Operations.Mod((fromCenter.Y + 1) / 2, 1));
                default:
                    return new FVec2();
            }
        }
    }
}
