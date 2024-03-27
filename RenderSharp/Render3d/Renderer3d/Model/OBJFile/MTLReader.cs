using RenderSharp.Common;

namespace RenderSharp.Render3d
{
    internal class MTLReader
    {
        Dictionary<string, Material> materials = new();
        DirectoryInfo directory;

        public MTLReader(DirectoryInfo directory)
        {
            this.directory = directory;
        }

        private Material ParseMaterial(StreamReader reader)
        {
            string? line;
            Texture diffuse = new(1, 1);
            while (reader.Peek() != 'n' && (line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(' ');
                switch (parts[0])
                {
                    case "Kd":
                        diffuse = new Texture(
                            1,
                            1,
                            new FRGBA(
                                float.Parse(parts[1]),
                                float.Parse(parts[2]),
                                float.Parse(parts[3]),
                            1));
                        break;
                    case "map_Kd":
                        diffuse = new Texture(directory.FullName + '/' + parts[1]);
                        break;
                }
            }

            return new Material(diffuse);
        }

        public MTLReader Read(FileInfo file)
        {
            if (!file.Exists)
            {
                Console.WriteLine($"Warning: File \"{file}\" does not exist. Skipping...");
                return this;
            }

            Console.WriteLine($"Loading material from file {file}");

            StreamReader reader = file.OpenText();

            Dictionary<string, Material> newMaterials = new();

            string currentMaterial;
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("newmtl "))
                {
                    currentMaterial = line[7..];
                    Console.WriteLine($"Found new material, {currentMaterial}.");
                    Material newMaterial = ParseMaterial(reader);
                    newMaterials[currentMaterial] = newMaterial;
                    Console.WriteLine('\t' + newMaterial.ToString().Replace("\n", "\n\t"));
                }
            }
            reader.Close();

            materials = materials
                .Concat(newMaterials)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            Console.WriteLine($"Loaded {newMaterials.Count} new materials.");
            Console.WriteLine($"Total: {materials.Count} materials.");

            return this;
        }

        public Dictionary<string, Material> MakeMaterials()
        {
            return materials;
        }
    }
}
