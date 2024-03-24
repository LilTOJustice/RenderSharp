using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Actor representing a box in 3D space.
    /// </summary>
    public class BoxActor : Actor
    {
        private Box box;

        internal BoxActor(
            in FVec3 size,
            in RVec3 rotation,
            in FVec3 position,
            Texture texture,
            FragShader fragShader)
            : base(size, rotation, position, texture, fragShader)
        {
            box = new Box(position, size, rotation);
        }

        internal override bool Sample(in FVec3 worldVec, in FVec3 cameraPos, double minDepth, double time, out RGBA sample, out double depth)
        {
            Box.Face face;
            if (box.Intersects(worldVec, cameraPos, minDepth, out depth, out face))
            {
                FVec3 fromCenter = (worldVec * depth - (Position - cameraPos)).Rotate(Rotation) / BoundingBoxSize;
                FVec2 uv;
            switch (face)
                {
                    case Box.Face.PosX:
                        uv = new FVec2(
                            1 - Operations.Mod((fromCenter.Z + 1) / 2, 1),
                            1 - Operations.Mod((fromCenter.Y + 1) / 2, 1));
                        break;
                    case Box.Face.NegX:
                        uv = new FVec2(
                            Operations.Mod((fromCenter.Z + 1) / 2, 1),
                            1 - Operations.Mod((fromCenter.Y + 1) / 2, 1));
                        break;
                    case Box.Face.PosY:
                        uv = new FVec2(
                            Operations.Mod((fromCenter.X + 1) / 2, 1),
                            1 - Operations.Mod((fromCenter.Z + 1) / 2, 1));
                        break;
                    case Box.Face.NegY:
                        uv = new FVec2(
                            1 - Operations.Mod((fromCenter.X + 1) / 2, 1),
                            1 - Operations.Mod((fromCenter.Z + 1) / 2, 1));
                        break;
                    case Box.Face.NegZ:
                        uv = new FVec2(
                            Operations.Mod((fromCenter.X + 1) / 2, 1),
                            1 - Operations.Mod((fromCenter.Y + 1) / 2, 1));
                        break;
                    case Box.Face.PosZ:
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
            return new BoxActor(
                BoundingBoxSize,
                Rotation,
                Position,
                Texture,
                FragShader);
        }
    }
}
