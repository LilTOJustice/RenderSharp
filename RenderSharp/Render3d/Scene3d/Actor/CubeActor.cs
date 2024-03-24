using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Actor representing a cube in 3D space.
    /// </summary>
    public class CubeActor : Actor
    {
        private Cube cube;

        internal CubeActor(
            in FVec3 size,
            in RVec3 rotation,
            in FVec3 position,
            Texture texture,
            FragShader fragShader)
            : base(size, rotation, position, texture, fragShader)
        {
            cube = new Cube(position, size, rotation);
        }

        internal override bool Sample(in FVec3 worldVec, in FVec3 cameraPos, double minDepth, double time, out RGBA sample, out double depth)
        {
            Cube.Face face;
            if (cube.Intersects(worldVec, cameraPos, minDepth, out depth, out face))
            {
                FVec3 fromCenter = (worldVec * depth - (Position - cameraPos)).Rotate(Rotation) / Size;
                FVec2 uv;

                switch (face)
                {
                    case Cube.Face.PosX:
                        uv = new FVec2(
                            1 - Operations.Mod((fromCenter.Z + 1) / 2, 1),
                            1 - Operations.Mod((fromCenter.Y + 1) / 2, 1));
                        break;
                    case Cube.Face.NegX:
                        uv = new FVec2(
                            Operations.Mod((fromCenter.Z + 1) / 2, 1),
                            1 - Operations.Mod((fromCenter.Y + 1) / 2, 1));
                        break;
                    case Cube.Face.PosY:
                        uv = new FVec2(
                            Operations.Mod((fromCenter.X + 1) / 2, 1),
                            1 - Operations.Mod((fromCenter.Z + 1) / 2, 1));
                        break;
                    case Cube.Face.NegY:
                        uv = new FVec2(
                            1 - Operations.Mod((fromCenter.X + 1) / 2, 1),
                            1 - Operations.Mod((fromCenter.Z + 1) / 2, 1));
                        break;
                    case Cube.Face.NegZ:
                        uv = new FVec2(
                            Operations.Mod((fromCenter.X + 1) / 2, 1),
                            1 - Operations.Mod((fromCenter.Y + 1) / 2, 1));
                        break;
                    case Cube.Face.PosZ:
                        uv = new FVec2(
                            1 - Operations.Mod((fromCenter.X + 1) / 2, 1),
                            1 - Operations.Mod((fromCenter.Y + 1) / 2, 1));
                        break;
                    default:
                        uv = new FVec2();
                        break;
                }

                FRGBA fOut;
                FragShader(Texture[uv], out fOut, (Vec2)(uv * Texture.Size), Texture.Size, time);
                sample = fOut;
                return true;
            }

            sample = new RGBA();
            return false;
        }

        internal override Actor Copy()
        {
            return new CubeActor(
                Size,
                Rotation,
                Position,
                Texture,
                FragShader);
        }
    }
}
