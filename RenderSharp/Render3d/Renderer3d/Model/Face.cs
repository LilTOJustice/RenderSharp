using MathSharp;
using RenderSharp.Common;

namespace RenderSharp.Render3d
{
    internal struct Face
    {
        public readonly FaceTriangle[] triangles;

        public readonly int index;

        public Face(FaceTriangle[] triangles, int index)
        {
            this.triangles = triangles;
            this.index = index;
        }

        public Face(in Face f, in FVec3 size, in RVec3 rotation, in FVec3 position)
        {
            triangles = new FaceTriangle[f.triangles.Length];
            for (int i = 0; i < f.triangles.Length; i++)
            {
                triangles[i] = new FaceTriangle(f.triangles[i], size, rotation, position);
            }

            index = f.index;
        }
    }
}
