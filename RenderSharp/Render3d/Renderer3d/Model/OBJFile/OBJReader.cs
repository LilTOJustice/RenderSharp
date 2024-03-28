using MathSharp;
using RenderSharp.Common;

namespace RenderSharp.Render3d
{
    internal class OBJReader : ModelReader
    {
        private List<FVec3> vertices = new();
        private List<FVec3> vertexNormals = new();
        private List<FVec2> textureVertices = new();
        private List<Face> faces = new();
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
            Console.WriteLine($"\tFound {requiredMaterials.Count} required materials. Loading...");
            MTLReader mtlReader = new(directory);
            foreach (string requiredMaterial in requiredMaterials)
            {
                FileInfo matFile = directory.EnumerateFiles().FirstOrDefault(f => f.Name == requiredMaterial)!;
                mtlReader.Read(matFile);
            }
            materials = mtlReader.MakeMaterials();
            Console.WriteLine($"\tLoaded {materials.Count} materials.");

            vertices = ParseVertices(file);
            Console.WriteLine($"\tLoaded {vertices.Count} vertices.");
            vertexNormals = ParseVertexNormals(file);
            Console.WriteLine($"\tLoaded {vertexNormals.Count} vertex normals.");
            textureVertices = ParseTextureVertices(file);
            Console.WriteLine($"\tLoaded {textureVertices.Count} texture vertices.");
            faces = ParseFaces(file);
            Console.WriteLine($"\tLoaded {faces.Count} faces.");

            return this;
        }

        public override Model MakeModel()
        {
            return new Model(faces.ToArray());
        }

        private void Parse(FileInfo file, Dictionary<string, Action<string>> targetLineActions)
        {
            string? line;
            StreamReader reader = file.OpenText();
            while ((line = reader.ReadLine()) != null)
            {
                if (targetLineActions.Any(kv => line.StartsWith(kv.Key)))
                {
                    targetLineActions.First(kv => line.StartsWith(kv.Key)).Value(line);
                }
            }
            reader.Close();
        }

        private void Parse(FileInfo file, Action<string> action, string targetLineStart)
        {
            Parse(
                file,
                new Dictionary<string, Action<string>>()
                {
                    { targetLineStart, action }
                });
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
            List<string> requiredMaterials = new();
            Parse(file, (string line) => requiredMaterials.Add(line[7..]), "mtllib ");
            return requiredMaterials;
        }

        private List<FVec3> ParseVertices(FileInfo file)
        {
            List<FVec3> vertices = new();
            Parse(file, (string line) =>
            {
                string[] coords = line[2..].Split(' ');
                vertices.Add(new FVec3(
                    float.Parse(coords[0]),
                    float.Parse(coords[1]),
                    float.Parse(coords[2])));
            }, "v ");

            // Center the vertices
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

            List<FVec3> centered = vertices.Select(v => v - center).ToList();
            
            // Normalize the vertices
            double max = centered.Max(v => Math.Max(Math.Max(Math.Abs(v.X), Math.Abs(v.Y)), Math.Abs(v.Z)));

            return centered.Select(v => v / max).ToList();
        }


        private List<FVec3> ParseVertexNormals(FileInfo file)
        {
            List<FVec3> vertexNormals = new();
            Parse(file, (string line) =>
            {
                string[] coords = line[3..].Split(' ');
                vertexNormals.Add(new FVec3(
                    float.Parse(coords[0]),
                    float.Parse(coords[1]),
                    float.Parse(coords[2])));
            }, "vn ");

            return vertexNormals;
        }

        private List<FVec2> ParseTextureVertices(FileInfo file)
        {
            List<FVec2> textureVertices = new();
            Parse(file, (string line) =>
            {
                string[] coords = line[3..].Split(' ');
                textureVertices.Add(new FVec2(
                    float.Parse(coords[0]),
                    coords.Length > 1 ? float.Parse(coords[1]) : 0));
            }, "vt ");

            return textureVertices;
        }

        private FVec2 GetTextureVertex(string face)
        {
            return GetFaceType(face) switch
                {
                    FaceType.VertexTextureCoord => textureVertices[int.Parse(face.Split('/')[1]) - 1],
                    FaceType.VertexNormal => textureVertices[int.Parse(face.Split('/')[1]) - 1],
                    _ => new FVec2()
                };
        }

        private List<FaceTriangle> MakeTriangles(List<string> faceVertexIndices)
        {
            List<FaceTriangle> triangles = new();
            int vertexCount = faceVertexIndices.Count;
            for (int i = 2; i < vertexCount; i++)
            {
                triangles.Add(
                    new FaceTriangle(
                        new Triangle(
                            vertices[int.Parse(faceVertexIndices[0].Split('/')[0]) - 1],
                            vertices[int.Parse(faceVertexIndices[i - 1].Split('/')[0]) - 1],
                            vertices[int.Parse(faceVertexIndices[i].Split('/')[0]) - 1]),
                        ( // Rotate the texture vertices by 1 to work with the renderer.
                            GetTextureVertex(faceVertexIndices[i]),
                            GetTextureVertex(faceVertexIndices[0]),
                            GetTextureVertex(faceVertexIndices[i - 1])
                        )));
            }

            return triangles;
        }

        private Face ParseFace(Material material, List<string> faceVertexIndices)
        {
            return new Face(
                material,
                MakeTriangles(faceVertexIndices).ToArray());
        }

        private List<Face> ParseFaces(FileInfo file)
        {
            List<Face> faces = new();
            Material? currentMaterial = null;
            Parse(file, new Dictionary<string, Action<string>>()
            {
                { "f ", (string line) =>
                    {
                        List<string> faceVertexIndices = line[2..].Split(' ').ToList();
                        faces.Add(
                            ParseFace(
                                currentMaterial ?? new Material(new Texture(1, 1, new RGBA())),
                                faceVertexIndices));
                    }
                },
                { "usemtl ", (string line) =>
                    {
                        string materialName = line[7..];
                        if (!materials.ContainsKey(materialName))
                        {
                            Console.WriteLine($"Warning: Referenced material \"{materialName}\" not found.");
                        }
                        else
                        {
                            currentMaterial = materials[materialName];
                        }
                    }
                }
            });

            return faces;
        }
    }
}
