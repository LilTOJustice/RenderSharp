using MathSharp;
using System.Runtime.CompilerServices;

namespace RenderSharp.Render3d
{
    internal class OBJReader : ModelReader
    {
        private enum FaceType
        {
            Vertex,
            VertexTextureCoord,
            VertexNormal,
            VertexNormalNoTextureCoord,
            Invalid
        }
        
        private FaceType GetFaceType(string face)
        {
            string[] parts = face.Split('/');
            if (parts.Length == 1)
            {
                return FaceType.Vertex;
            }
            else if (parts.Length == 2)
            {
                return FaceType.VertexTextureCoord;
            }
            else if (parts.Length == 3)
            {
                return parts[1] == string.Empty ? FaceType.VertexNormalNoTextureCoord : FaceType.VertexNormal;
            }
            else
            {
                return FaceType.Invalid;
            }
        }

        private List<FVec3> ParseVertices(FileInfo file)
        {
            StreamReader reader = file.OpenText();
            List<FVec3> vertices = new();
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("v "))
                {
                    string[] coords = line[2..].Split(' ');
                    vertices.Add(new FVec3(
                       float.Parse(coords[0]),
                       float.Parse(coords[1]),
                       float.Parse(coords[2])));
                }
            }
            reader.Close();

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

            return vertices.Select(v => v - center).ToList();
        }

        private List<List<int>> ParseFaces(FileInfo file)
        {
            StreamReader reader = file.OpenText();
            List<List<int>> faceElements = new();
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("f "))
                {
                    string[] face = line[2..].Split(' ');
                    faceElements.Add(
                        face.Select(f =>
                        {
                            switch (GetFaceType(f))
                            {
                                case FaceType.Vertex:
                                    return int.Parse(f) - 1;
                                case FaceType.VertexTextureCoord:
                                    return int.Parse(f.Split('/')[0]) - 1;
                                case FaceType.VertexNormal:
                                    return int.Parse(f.Split('/')[0]) - 1;
                                case FaceType.VertexNormalNoTextureCoord:
                                    return int.Parse(f.Split('/')[0]) - 1;
                                default:
                                    return 0;
                            }
                        }).ToList());
                }
            }
            reader.Close();

            return faceElements;
        }

        private List<Triangle> MakeTriangles(List<List<FVec3>> faceElements)
        {
            List<Triangle> triangles = new();
            foreach (List<FVec3> face in faceElements)
            {
                int faceVertices = face.Count;
                for (int i = 2; i < faceVertices; i++)
                {
                    triangles.Add(new Triangle(face[0], face[i - 1], face[i]));
                }
            }

            return triangles;
        }

        public override Model Read(FileInfo file)
        {
            Console.WriteLine($"Loading object from file {file}");

            List<FVec3> vertices = ParseVertices(file);
            List<List<FVec3>> faceElements = ParseFaces(file)
                .Select(f => f.Select(i => vertices[i]).ToList())
                .ToList();
            List<Triangle> triangles = MakeTriangles(faceElements);

            Console.WriteLine($"Loaded {vertices.Count} vertices, {faceElements.Count} faces, and {triangles.Count} triangles.");

            return new Model(triangles.ToArray());
        }
    }
}
