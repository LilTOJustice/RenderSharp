using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Model for use in <see cref="ModelActor"/>.
    /// </summary>
    public struct Model
    {
        internal readonly Face[] faces;

        private BVHOctree bvh;

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
            bvh = new BVHOctree(faces.SelectMany(f => f.triangles).ToArray());
        }

        internal Model(in Model model, in FVec3 size, in RVec3 rotation, in FVec3 position)
        {
            faces = new Face[model.faces.Length];
            for (int i = 0; i < model.faces.Length; i++)
            {
                faces[i] = new Face(model.faces[i], size, rotation, position);
            }

            bvh = new BVHOctree(faces.SelectMany(f => f.triangles).ToArray());
        }

        internal bool Sample(in FVec3 worldVec, double minDepth, out RGBA sample, out double depth)
        {
            List<(RGBA, double)> renderQueue = new();
            sample = new RGBA();
            depth = -1;

            HashSet<FaceTriangle> potentialTriangles = bvh.GetPotentialIntersectingTriangles(worldVec);

            foreach (FaceTriangle triangle in potentialTriangles)
            {
                if (triangle.Intersects(worldVec, minDepth, out FVec2 uv, out double d))
                {
                    renderQueue.Add((triangle.material.Diffuse[uv], d));
                }
            }

            if (renderQueue.Count == 0)
            {
                return false;
            }

            renderQueue.Sort((a, b)  => b.Item2.CompareTo(a.Item2));
            depth = renderQueue.Last().Item2;

            foreach ((RGBA s, _) in renderQueue)
            {
                sample = ColorFunctions.AlphaBlend(s, sample);
            }

            return true;
        }
    }
}
