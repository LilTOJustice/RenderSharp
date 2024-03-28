using MathSharp;
using RenderSharp.Common;

namespace RenderSharp.Render3d
{
    internal struct Face
    {
        public readonly Material material;

        public readonly FaceTriangle[] triangles;

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

        public Face(in Face f, in FVec3 size, in RVec3 rotation, in FVec3 position)
        {
            material = f.material;
            triangles = new FaceTriangle[f.triangles.Length];
            for (int i = 0; i < f.triangles.Length; i++)
            {
                triangles[i] = new FaceTriangle(f.triangles[i], size, rotation, position);
            }
        }
    }
}
