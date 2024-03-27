using MathSharp;
using RenderSharp.Common;

namespace RenderSharp.Render3d
{
    internal struct Face
    {
        private Material material;

        private FaceTriangle[] triangles;

        public bool Sample(in FVec3 worldVec, double minDepth, out RGBA sample, out double depth)
        {
            double outDepth = double.MaxValue;
            RGBA outSample = new RGBA();

            FVec2 uv;
            foreach (FaceTriangle triangle in triangles)
            {
                if (triangle.Sample(worldVec, minDepth, out uv, out depth))
                {
                    if (depth < outDepth)
                    {
                        outSample = material.Diffuse[uv];
                        outDepth = depth;
                    }
                }
            }

            sample = outSample;

            if (outDepth != double.MaxValue)
            {
                depth = outDepth;
                return true;
            }

            depth = -1;
            return false;
        }

        public Face(Material material, FaceTriangle[] triangles)
        {
            this.material = material;
            this.triangles = triangles;
        }

        public Face(Face f, FVec3 size, RVec3 rotation, FVec3 position)
        {
            material = f.material;
            triangles = f.triangles.Select(t => new FaceTriangle(t, size, rotation, position)).ToArray();
        }
    }
}
