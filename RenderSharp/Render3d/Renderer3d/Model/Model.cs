using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Model for use in <see cref="ModelActor"/>.
    /// </summary>
    public struct Model
    {
        private readonly Face[] faces;
        private readonly BoundingBox boundingBox;

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

        private BoundingBox GetBoundingBox(Face[] faces, in FVec3? size = null, in RVec3? rotation = null, in FVec3? position = null)
        {
            FVec3 s = size ?? new FVec3(1, 1, 1);
            RVec3 r = rotation ?? new RVec3();
            FVec3 p = position ?? new FVec3();
            double minX = faces.Min(f => f.triangles.Min(t => Math.Min(Math.Min(t.triangle.v0.X, t.triangle.v1.X), t.triangle.v2.X)));
            double minY = faces.Min(f => f.triangles.Min(t => Math.Min(Math.Min(t.triangle.v0.Y, t.triangle.v1.Y), t.triangle.v2.Y)));
            double minZ = faces.Min(f => f.triangles.Min(t => Math.Min(Math.Min(t.triangle.v0.Z, t.triangle.v1.Z), t.triangle.v2.Z)));
            double maxX = faces.Max(f => f.triangles.Max(t => Math.Max(Math.Max(t.triangle.v0.X, t.triangle.v1.X), t.triangle.v2.X)));
            double maxY = faces.Max(f => f.triangles.Max(t => Math.Max(Math.Max(t.triangle.v0.Y, t.triangle.v1.Y), t.triangle.v2.Y)));
            double maxZ = faces.Max(f => f.triangles.Max(t => Math.Max(Math.Max(t.triangle.v0.Z, t.triangle.v1.Z), t.triangle.v2.Z)));
            FVec3 min = (new FVec3(minX, minY, minZ) * s).Rotate(r) + p;
            FVec3 max = (new FVec3(maxX, maxY, maxZ) * s).Rotate(r) + p;
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

            boundingBox = GetBoundingBox(model.faces, size, rotation, position);
        }

        internal bool Sample(in FVec3 worldVec, double minDepth, double time, out RGBA sample, out double depth)
        {
            if (!boundingBox.Intersects(worldVec)) // TODO: Check if we don't intersect bounding box
            {
                Console.WriteLine("No intersection");
                sample = new RGBA();
                depth = -1;
                return false;
            }

            double outDepth = double.MaxValue;
            RGBA outSample = new RGBA();
            foreach (Face face in faces)
            {
                if (face.Sample(worldVec, minDepth, out sample, out depth))
                {
                    if (depth < outDepth)
                    {
                        outSample = sample;
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
    }
}
