using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Model for use in <see cref="ModelActor"/>.
    /// </summary>
    public struct Model
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

        internal Model(in Model model, in FVec3 size, in RVec3 rotation, in FVec3 position)
        {
            faces = new Face[model.faces.Length];
            for (int i = 0; i < model.faces.Length; i++)
            {
                faces[i] = new Face(model.faces[i], size, rotation, position);
            }
        }

        internal bool Sample(in FVec3 worldVec, double minDepth, double time, out RGBA sample, out double depth)
        {
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
