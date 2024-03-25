using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Model for use in <see cref="ModelActor"/>.
    /// </summary>
    public class Model
    {
        private readonly Face[] faces;

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
        }

        internal Model(Model model, FVec3 size, RVec3 rotation, FVec3 position, FVec3? cameraPos = null)
        {
            faces = model.faces.Select(f => new Face()
            {
                Material = f.Material,
                Triangles = f.Triangles.Select(t => new Triangle(t, size, rotation, position, cameraPos)).ToArray()
            }).ToArray();
        }

        internal bool Sample(in FVec3 worldVec, double minDepth, double time, out RGBA sample, out double depth)
        {
            double outDepth = double.MaxValue;
            sample = new RGBA();
            foreach (Face face in faces)
            {
                foreach (Triangle triangle in face.Triangles)
                {
                    if (triangle.Intersects(worldVec, minDepth, out depth))
                    {
                        if (depth < outDepth)
                        {
                            outDepth = depth;
                            sample = face.Material[0, 0];
                        }
                    }
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
