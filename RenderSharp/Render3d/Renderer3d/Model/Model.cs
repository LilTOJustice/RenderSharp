using MathSharp;
using RenderSharp.Common;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Model for use in <see cref="ModelActor"/>.
    /// </summary>
    public struct Model
    {
        internal readonly Face[] faces;

        private BVH bvh;

        /// <summary>
        /// Create a new model from a file.
        /// </summary>
        /// <param name="filename">Must be of type OBJ.</param>
        public static Model FromFile(string filename)
        {
            FileInfo file = new(filename);
            switch (file.Extension)
            {
                case ".obj":
                    return new OBJReader().Read(file).MakeModel();
                default:
                    throw new ArgumentException($"Unsupported file type, {file.Extension}");
            }
        }

        internal Model(Face[] faces)
        {
            this.faces = faces;
            bvh = new BVH(faces.SelectMany(f => f.triangles).ToArray());
        }

        internal Model(in Model model, in FVec3 size, in RVec3 rotation, in FVec3 position)
        {
            faces = new Face[model.faces.Length];
            for (int i = 0; i < model.faces.Length; i++)
            {
                faces[i] = new Face(model.faces[i], size, rotation, position);
            }

            bvh = new BVH(faces.SelectMany(f => f.triangles).ToArray());
        }

        internal void Sample(in FVec3 worldVec, double minDepth, out List<(RGBA, FVec2, Material, double)> renderQueue, out double depth)
        {
            renderQueue = new();
            depth = double.PositiveInfinity;

            HashSet<FaceTriangle> potentialTriangles = bvh.GetPotentialIntersectingTriangles(worldVec);

            foreach (FaceTriangle triangle in potentialTriangles)
            {
                if (triangle.Intersects(worldVec, minDepth, out FVec2 uv, out double d))
                {
                    renderQueue.Add((triangle.material.Diffuse[uv], uv, triangle.material, d));
                }
            }
        }
    }
}
