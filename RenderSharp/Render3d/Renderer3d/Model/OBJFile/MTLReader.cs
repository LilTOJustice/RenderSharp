using RenderSharp.Common;

namespace RenderSharp.Render3d
{
    internal class MTLReader
    {
        Dictionary<string, Material> materials = new();

        private Material ParseMaterial(StreamReader reader)
        {
            string? line;
            RGBA color = new();
            while (reader.Peek() != 'n' && (line = reader.ReadLine()) != null)
            {
                string start = line.Split(' ')[0];
                switch (start)
                {
                    case "Kd":
                        string[] parts = line.Split(' ');
                        color = new FRGBA(
                            float.Parse(parts[1]),
                            float.Parse(parts[2]),
                            float.Parse(parts[3]),
                            1);
                        break;
                }
            }

            return new Material(new Texture(1, 1, color));
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
