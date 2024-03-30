using MathSharp;

namespace RenderSharp.Render3d.Renderer3d.Model
{
    internal class BVHOctree
    {
        private (
            BVHOctree o1, BVHOctree o2, BVHOctree o3, BVHOctree o4,
            BVHOctree o5, BVHOctree o6, BVHOctree o7, BVHOctree o8) children;

        private BoundingBox boundingBox;

        private Face[] faces;

        private bool isLeaf;

        private static bool Within(FVec3 test, FVec3 min, FVec3 max)
        {
            return test.X >= min.X &&
                test.Y >= min.Y &&
                test.Z >= min.Z &&
                test.X <= max.X &&
                test.Y <= max.Y &&
                test.Z <= max.Z;  
        }

        public BVHOctree(BoundingBox boundingBox, Face[] faces)
        {
            this.boundingBox = boundingBox;
            this.faces = faces;
            isLeaf = false;

            if (faces.Length <= 1)
            {
                isLeaf = true;
                return;
            }

            FVec3 min = boundingBox.min;
            FVec3 max = boundingBox.max;
            FVec3 mid = (max - min) / 2 + min;

            BoundingBox bbo1 = new BoundingBox(min, mid);
            BoundingBox bbo2 = new BoundingBox(new FVec3(mid.X, min.Y, min.Z), new FVec3(max.X, mid.Y, mid.Z));
            BoundingBox bbo3 = new BoundingBox(new FVec3(mid.X, min.Y, mid.Z), new FVec3(max.X, mid.Y, max.Z));
            BoundingBox bbo4 = new BoundingBox(new FVec3(min.X, min.Y, mid.Z), new FVec3(mid.X, mid.Y, max.Z));
            BoundingBox bbo5 = new BoundingBox(new FVec3(min.X, mid.Y, min.Z), new FVec3(mid.X, max.Y, mid.Z));
            BoundingBox bbo6 = new BoundingBox(new FVec3(mid.X, mid.Y, min.Z), new FVec3(max.X, max.Y, mid.Z));
            BoundingBox bbo7 = new BoundingBox(mid, max);
            BoundingBox bbo8 = new BoundingBox(new FVec3(min.X, mid.Y, mid.Z), new FVec3(mid.X, max.Y, max.Z));

            Face[] facesO1 = faces.Where(f => f.triangles.Any(t =>
                Within(t.triangle.v0, bbo1.min, bbo1.max) ||
                Within(t.triangle.v1, bbo1.min, bbo1.max) ||
                Within(t.triangle.v2, bbo1.min, bbo1.max))).ToArray();

            Face[] facesO2 = faces.Where(f => f.triangles.Any(t =>
                Within(t.triangle.v0, bbo2.min, bbo2.max) ||
                Within(t.triangle.v1, bbo2.min, bbo2.max) ||
                Within(t.triangle.v2, bbo2.min, bbo2.max))).ToArray();

            Face[] facesO3 = faces.Where(f => f.triangles.Any(t =>
                Within(t.triangle.v0, bbo3.min, bbo3.max) ||
                Within(t.triangle.v1, bbo3.min, bbo3.max) ||
                Within(t.triangle.v2, bbo3.min, bbo3.max))).ToArray();

            Face[] facesO4 = faces.Where(f => f.triangles.Any(t =>
                Within(t.triangle.v0, bbo4.min, bbo4.max) ||
                Within(t.triangle.v1, bbo4.min, bbo4.max) ||
                Within(t.triangle.v2, bbo4.min, bbo4.max))).ToArray();

            Face[] facesO5 = faces.Where(f => f.triangles.Any(t =>
                Within(t.triangle.v0, bbo5.min, bbo5.max) ||
                Within(t.triangle.v1, bbo5.min, bbo5.max) ||
                Within(t.triangle.v2, bbo5.min, bbo5.max))).ToArray();

            Face[] facesO6 = faces.Where(f => f.triangles.Any(t =>
                Within(t.triangle.v0, bbo6.min, bbo6.max) ||
                Within(t.triangle.v1, bbo6.min, bbo6.max) ||
                Within(t.triangle.v2, bbo6.min, bbo6.max))).ToArray();

            Face[] facesO7 = faces.Where(f => f.triangles.Any(t =>
                Within(t.triangle.v0, bbo7.min, bbo7.max) ||
                Within(t.triangle.v1, bbo7.min, bbo7.max) ||
                Within(t.triangle.v2, bbo7.min, bbo7.max))).ToArray();

            Face[] facesO8 = faces.Where(f => f.triangles.Any(t =>
                Within(t.triangle.v0, bbo8.min, bbo8.max) ||
                Within(t.triangle.v1, bbo8.min, bbo8.max) ||
                Within(t.triangle.v2, bbo8.min, bbo8.max))).ToArray();

            children = (
                new BVHOctree(bbo1, facesO1),
                new BVHOctree(bbo2, facesO2),
                new BVHOctree(bbo3, facesO3),
                new BVHOctree(bbo4, facesO4),
                new BVHOctree(bbo5, facesO5),
                new BVHOctree(bbo6, facesO6),
                new BVHOctree(bbo7, facesO7),
                new BVHOctree(bbo8, facesO8));
        }
    }
}
