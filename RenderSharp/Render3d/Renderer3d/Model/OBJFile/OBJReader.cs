using MathSharp;
using RenderSharp.Common;

namespace RenderSharp.Render3d
{
    internal class OBJReader : ModelReader
    {
        private List<FVec3> vertices = new();
        private List<Face> faceElements = new();
        private Dictionary<string, Material> materials = new();

        private enum FaceType
        {
            Vertex,
            VertexTextureCoord,
            VertexNormal,
            VertexNormalNoTextureCoord,
            Invalid
        }

        public override OBJReader Read(FileInfo file)
        {
            if (!file.Exists)
            {
                Console.WriteLine($"Warning: File \"{file}\" does not exist. Skipping...");
                return this;
            }

            Console.WriteLine($"Loading object from file {file}");
            DirectoryInfo directory = file.Directory!;

            List<string> requiredMaterials = ParseRequiredMaterials(file);
            Console.WriteLine($"Found {requiredMaterials.Count} required materials. Loading...");
            MTLReader mtlReader = new();
            foreach (string requiredMaterial in requiredMaterials)
            {
                FileInfo matFile = directory.EnumerateFiles().FirstOrDefault(f => f.Name == requiredMaterial)!;
                mtlReader.Read(matFile);
            }

            Dictionary<string, Material> newMaterials = mtlReader.MakeMaterials();
            List<FVec3> newVertices = ParseVertices(file);
            List<Face> newFaceElements = ParseFaces(file, newVertices, newMaterials);

            materials = materials.Concat(newMaterials)
                .ToDictionary(kv => kv.Key, kv => kv.Value);
            vertices.AddRange(newVertices);
            faceElements.AddRange(newFaceElements);

            Console.WriteLine($"Loaded {newMaterials.Count} new materials, " +
                $"{newVertices.Count} new vertices, " +
                $"{newFaceElements.Count} new faces, " +
                $"and {newFaceElements.Sum(f => f.Triangles.Count())} new triangles.");
            Console.WriteLine($"Total: {materials.Count} materials, " +
                $"{vertices.Count} vertices, " +
                $"{faceElements.Count} faces, " +
                $"and {faceElements.Sum(f => f.Triangles.Count())} triangles.");

            return this;
        }

        public override Model MakeModel()
        {
            return new Model(faceElements.ToArray());
        }
        
        private FaceType GetFaceType(string face)
        {
            string[] parts = face.Split('/');
            switch (parts.Length)
            {
                case 1:
                    return FaceType.Vertex;
                case 2:
                    return FaceType.VertexTextureCoord;
                case 3:
                    return parts[1] == string.Empty ? FaceType.VertexNormalNoTextureCoord : FaceType.VertexNormal;
                default:
                    return FaceType.Invalid;
            }
        }

        private List<string> ParseRequiredMaterials(FileInfo file)
        {
            StreamReader reader = file.OpenText();
            List<string> requiredMaterials = new();
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("mtllib "))
                {
                    requiredMaterials.Add(line[7..]);
                }
            }
            reader.Close();

            return requiredMaterials;
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

        private List<Face> ParseFaces(FileInfo file, List<FVec3> newVertices, Dictionary<string, Material> newMaterials)
        {
            StreamReader reader = file.OpenText();
            List<Face> faceElements = new();
            string? line;
            Material? currentMaterial = null;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("f "))
                {
                    string[] face = line[2..].Split(' ');
                    faceElements.Add(new Face()
                    {
                        Material = currentMaterial ?? new Material(new Texture(1, 1, new RGBA())),
                        Triangles = MakeTriangles(face.Select(f =>
                        {
                            switch (GetFaceType(f))
                            {
                                case FaceType.Vertex:
                                    return newVertices[int.Parse(f) - 1];
                                case FaceType.VertexTextureCoord:
                                    return newVertices[int.Parse(f.Split('/')[0]) - 1];
                                case FaceType.VertexNormal:
                                    return newVertices[int.Parse(f.Split('/')[0]) - 1];
                                case FaceType.VertexNormalNoTextureCoord:
                                    return newVertices[int.Parse(f.Split('/')[0]) - 1];
                                default:
                                    throw new Exception("Invalid face type");
                            }
                        }).ToList()).ToArray()
                    });
                }
                else if (line.StartsWith("usemtl "))
                {
                    string materialName = line[7..];
                    if (!newMaterials.ContainsKey(materialName))
                    {
                        Console.WriteLine($"Warning: Referenced material \"{materialName}\" not found.");
                    }
                    else
                    {
                        currentMaterial = newMaterials[materialName];
                    }
                }
            }
            reader.Close();

            return faceElements;
        }

        private List<Triangle> MakeTriangles(List<FVec3> vertices)
        {
            List<Triangle> triangles = new();
            int vertexCount = vertices.Count;
            for (int i = 2; i < vertexCount; i++)
            {
                triangles.Add(new Triangle(vertices[0], vertices[i - 1], vertices[i]));
            }

            return triangles;
        }
    }
}
