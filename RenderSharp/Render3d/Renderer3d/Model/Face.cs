using RenderSharp.Common;

namespace RenderSharp.Render3d
{
    internal struct Face
    {
        public Material Material { get; init; }

        public Triangle[] Triangles { get; init; }
    }
}
