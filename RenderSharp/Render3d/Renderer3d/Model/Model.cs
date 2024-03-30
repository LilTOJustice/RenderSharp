using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Model for use in <see cref="ModelActor"/>.
    /// </summary>
    public struct Model
    {
        internal readonly Face[] faces;
        internal readonly BoundingBox boundingBox;

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

        private BoundingBox GetBoundingBox(Face[] faces)
        {
            double minX = faces.Min(f => f.triangles.Min(t => Math.Min(Math.Min(t.triangle.v0.X, t.triangle.v1.X), t.triangle.v2.X)));
            double minY = faces.Min(f => f.triangles.Min(t => Math.Min(Math.Min(t.triangle.v0.Y, t.triangle.v1.Y), t.triangle.v2.Y)));
            double minZ = faces.Min(f => f.triangles.Min(t => Math.Min(Math.Min(t.triangle.v0.Z, t.triangle.v1.Z), t.triangle.v2.Z)));
            double maxX = faces.Max(f => f.triangles.Max(t => Math.Max(Math.Max(t.triangle.v0.X, t.triangle.v1.X), t.triangle.v2.X)));
            double maxY = faces.Max(f => f.triangles.Max(t => Math.Max(Math.Max(t.triangle.v0.Y, t.triangle.v1.Y), t.triangle.v2.Y)));
            double maxZ = faces.Max(f => f.triangles.Max(t => Math.Max(Math.Max(t.triangle.v0.Z, t.triangle.v1.Z), t.triangle.v2.Z)));
            FVec3 min = new FVec3(minX, minY, minZ);
            FVec3 max = new FVec3(maxX, maxY, maxZ);
            return new BoundingBox(min, max);
        }

        internal Model(Face[] faces)
        {
            this.faces = faces;
            boundingBox = GetBoundingBox(faces);
        }

        internal Model(in Model model, in FVec3 size, in RVec3 rotation, in FVec3 position)
        {
            faces = new Face[model.faces.Length];
            for (int i = 0; i < model.faces.Length; i++)
            {
                faces[i] = new Face(model.faces[i], size, rotation, position);
            }

            boundingBox = GetBoundingBox(faces);
        }

        internal bool Sample(in FVec3 worldVec, double minDepth, double time, out RGBA sample, out double depth)
        {
            List<(RGBA, double)> renderQueue = new();
            sample = new RGBA();
            depth = -1;

            if (!boundingBox.Intersects(worldVec))
            {
                return false;
            }
            else
            {
                renderQueue.Add((new RGBA(255, 255, 255, 128), 0));
            }

            foreach (Face face in faces)
            {
                if (face.Sample(worldVec, minDepth, out sample, out depth))
                {
                    renderQueue.Add((sample, depth));
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
