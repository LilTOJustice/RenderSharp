namespace RenderSharp.Render3d
{
    internal abstract class ModelReader
    {
        public abstract ModelReader Read(FileInfo file);

        public abstract Model MakeModel();
    }
}
