namespace RenderSharp.Render3d
{
    internal abstract class ModelReader
    {
        /// <summary>
        /// Read a model from a file. If called multiple times, the resulting model will be aggregated from prior reads.
        /// </summary>
        public abstract ModelReader Read(FileInfo file);

        /// <summary>
        /// Make a model from the read data.
        /// </summary>
        public abstract Model MakeModel();
    }
}
