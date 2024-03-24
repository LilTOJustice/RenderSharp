namespace RenderSharp.Render3d
{
    internal abstract class ModelReader
    {
        /// <summary>
        /// Read a model from a file.
        /// </summary>
        public abstract Model Read(FileInfo file);
    }
}
