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
        private FVec3 cameraRelPosition;

        internal CubeActor(
            in FVec3 size,
            in RVec3 rotation,
            in FVec3 position,
            Texture texture,
            FragShader fragShader,
            in FVec3? cameraPos = null)
            : base(size, rotation, position, texture, fragShader)
        {
            cameraRelPosition = position - cameraPos ?? new FVec3();
            cube = new Cube(cameraRelPosition, size, rotation);
        }

        internal override void Sample(in FVec3 worldVec, double minDepth, double time, out RGBA sample, out double depth)
        {
            (Cube.Face, Cube.Face) faceCloseFar;
            (double, double) closeFar;
            sample = new RGBA();
            depth = double.PositiveInfinity;
            if (cube.Intersects(worldVec, minDepth, out closeFar, out faceCloseFar))
            {
                FRGBA fOut;
                FVec2 uvFar = GetUV(faceCloseFar.Item2, (worldVec * closeFar.Item2 - cameraRelPosition).Rotate(Rotation) / Size);
                FragShader(Texture[uvFar], out fOut, (Vec2)(uvFar * Texture.Size), Texture.Size, time);
                sample = ColorFunctions.AlphaBlend(fOut, sample);
                depth = closeFar.Item2;

                if (closeFar.Item1 == double.PositiveInfinity)
                {
                    return;
                }

                FVec2 uvClose = GetUV(faceCloseFar.Item1, (worldVec * closeFar.Item1 - cameraRelPosition).Rotate(Rotation) / Size);
                FragShader(Texture[uvClose], out fOut, (Vec2)(uvClose * Texture.Size), Texture.Size, time);
                sample = ColorFunctions.AlphaBlend(fOut, sample);
                depth = closeFar.Item1;
            }
        }

        internal override Actor Copy(in FVec3 cameraPos)
        {
            return new CubeActor(
                Size,
                Rotation,
                Position,
                Texture,
                FragShader,
                cameraPos);
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
