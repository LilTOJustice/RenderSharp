using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Model for use in <see cref="ModelActor"/>.
    /// </summary>
    public class Model
    {
        private readonly Triangle[] triangles;

        internal int TriangleCount => triangles.Length;

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
                    return new OBJReader().Read(file);
                default:
                    throw new ArgumentException($"Unsupported file type, {file.Extension}");
            }
        }

        internal Model(Triangle[] triangles)
        {
            this.triangles = triangles;
        }

        internal Model(Model model, FVec3 size, RVec3 rotation, FVec3 position, FVec3? cameraPos = null)
        {
            triangles = model.triangles.Select(t => new Triangle(t, size, rotation, position, cameraPos)).ToArray();
        }

        internal bool Sample(in FVec3 worldVec, double minDepth, double time, out RGBA sample, out double depth)
        {
            double outDepth = double.MaxValue;
            sample = new RGBA();
            foreach (Triangle triangle in triangles)
            {
                if (triangle.Intersects(worldVec, minDepth, out depth))
                {
                    sample = new RGBA(255, 255, 255, 255);
                    outDepth = Math.Min(outDepth, depth);
                }
            }

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
