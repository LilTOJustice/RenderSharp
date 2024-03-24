using MathSharp;

namespace RenderSharp.Render3d
{
    internal class OBJReader : ModelReader
    {
        public override Model Read(FileInfo file)
        {
            Console.WriteLine($"Loading object from file {file}");
            List<FVec3> vertices = new();
            StreamReader reader = file.OpenText();
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                string start = line.Split(' ')[0];
                switch (start)
                {
                    case "v":
                        string[] coords = line[2..].Split(' ');
                        vertices.Add(new FVec3(
                           float.Parse(coords[0]),
                           float.Parse(coords[1]),
                           float.Parse(coords[2])));
                        break;
                }
            }
            reader.Close();

            int verticesCount = vertices.Count;
            Console.WriteLine($"Found {verticesCount} vertices");

            List<Triangle> triangles = new();
            double minX = vertices.Min(v => v.X);
            double maxX = vertices.Max(v => v.X);
            double minY = vertices.Min(v => v.Y);
            double maxY = vertices.Max(v => v.Y);
            double minZ = vertices.Min(v => v.Z);
            double maxZ = vertices.Max(v => v.Z);
            FVec3 center = new FVec3(
                (minX + maxX) / 2,
                (minY + maxY) / 2,
                (minZ + maxZ) / 2);

            List<FVec3> centeredVertices = vertices.Select(v => v - center).ToList();

            for (int i = 2; i < verticesCount; i ++)
            {
                triangles.Add(new Triangle(centeredVertices[0], centeredVertices[i - 1], centeredVertices[i]));
            }

            return new Model(triangles.ToArray());
        }
    }
}
