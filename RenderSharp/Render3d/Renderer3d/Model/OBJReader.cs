using MathSharp;

namespace RenderSharp.Render3d
{
    internal class OBJReader : ModelReader
    {
        public override Model Read(FileInfo file)
        {
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

            List<Triangle> triangles = new();
            int verticesCount = vertices.Count;
            for (int i = 0; i + 2 < verticesCount; i += 3)
            {
                triangles.Add(new Triangle(vertices[i], vertices[i + 1], vertices[i + 2]));
            }

            return new Model(triangles.ToArray());
        }
    }
}
