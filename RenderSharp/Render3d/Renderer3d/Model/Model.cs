using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Model for use in <see cref="ModelActor"/>.
    /// </summary>
    public class Model
    {
        private Triangle[] triangles;

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

        internal bool Sample(in FVec3 worldVec, in FVec3 cameraPos, double minDepth, double time, out RGBA sample, out double depth)
        {
            foreach (Triangle triangle in triangles)
            {
                if (triangle.Intersects(worldVec, cameraPos, minDepth, out depth))
                {
                    sample = new RGBA(255, 255, 255, 255);
                    return true;
                }
            }

            depth = -1;
            sample = new RGBA();
            return false;
        }
    }
}
