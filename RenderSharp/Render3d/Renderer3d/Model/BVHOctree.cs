using MathSharp;

namespace RenderSharp.Render3d
{
    internal class BVHOctree
    {
        public enum Dim
        {
            X,
            Y,
            Z
        }

        private BVHOctree? left, right;

        private BoundingBox boundingBox;

        private FaceTriangle? triangle;

        public BVHOctree(FaceTriangle[] triangles, Dim sortDim = Dim.X)
        {
            boundingBox = GetBoundingBox(triangles);

            if (triangles.Length == 1)
            {
                triangle = triangles[0];
                return;
            }

            switch (sortDim)
            {
                case Dim.X:
                    List<FaceTriangle> toSortX = triangles.ToList();
                    toSortX.Sort((a, b) => a.triangle.centroid.X.CompareTo(b.triangle.centroid.X));
                    triangles = toSortX.ToArray();

                    double centerX = triangles[triangles.Length / 2].triangle.centroid.X;

                    FaceTriangle[] leftTrianglesX = triangles.Take(triangles.Length / 2).ToArray();

                    FaceTriangle[] rightTrianglesX = triangles.TakeLast(triangles.Length / 2 + triangles.Length % 2).ToArray();

                    left = new BVHOctree(leftTrianglesX, Dim.Y);
                    right = new BVHOctree(rightTrianglesX, Dim.Y);
                    break;
                case Dim.Y:
                    List<FaceTriangle> toSortY = triangles.ToList();
                    toSortY.Sort((a, b) => a.triangle.centroid.Y.CompareTo(b.triangle.centroid.Y));
                    triangles = toSortY.ToArray();

                    double centerY = triangles[triangles.Length / 2].triangle.centroid.Y;

                    FaceTriangle[] leftTrianglesY = triangles.Take(triangles.Length / 2).ToArray();

                    FaceTriangle[] rightTrianglesY = triangles.TakeLast(triangles.Length / 2 + triangles.Length % 2).ToArray();

                    left = new BVHOctree(leftTrianglesY, Dim.Z);
                    right = new BVHOctree(rightTrianglesY, Dim.Z);
                    break;
                case Dim.Z:
                    List<FaceTriangle> toSortZ = triangles.ToList();
                    toSortZ.Sort((a, b) => a.triangle.centroid.Z.CompareTo(b.triangle.centroid.Z));
                    triangles = toSortZ.ToArray();

                    double centerZ = triangles[triangles.Length / 2].triangle.centroid.Z;

                    FaceTriangle[] leftTrianglesZ = triangles.Take(triangles.Length / 2).ToArray();

                    FaceTriangle[] rightTrianglesZ = triangles.TakeLast(triangles.Length / 2 + triangles.Length % 2).ToArray();

                    left = new BVHOctree(leftTrianglesZ, Dim.X);
                    right = new BVHOctree(rightTrianglesZ, Dim.X);
                    break;
            }
        }

        public HashSet<FaceTriangle> GetPotentialIntersectingTriangles(in FVec3 worldVec)
        {
            if (!boundingBox.Intersects(worldVec))
            {
                return new();
            }

            if (triangle != null)
            {
                return new() { (FaceTriangle)triangle };
            }

            HashSet<FaceTriangle> triangles = new();
            triangles.UnionWith(left!.GetPotentialIntersectingTriangles(worldVec));
            triangles.UnionWith(right!.GetPotentialIntersectingTriangles(worldVec));
            return triangles;
        }

        private static BoundingBox GetBoundingBox(FaceTriangle[] triangles)
        {
            return new BoundingBox(
                new FVec3(
                    triangles.Min(t => Math.Min(Math.Min(t.triangle.v0.X, t.triangle.v1.X), t.triangle.v2.X)),
                    triangles.Min(t => Math.Min(Math.Min(t.triangle.v0.Y, t.triangle.v1.Y), t.triangle.v2.Y)),
                    triangles.Min(t => Math.Min(Math.Min(t.triangle.v0.Z, t.triangle.v1.Z), t.triangle.v2.Z))
                    ),
                new FVec3(
                    triangles.Max(t => Math.Max(Math.Max(t.triangle.v0.X, t.triangle.v1.X), t.triangle.v2.X)),
                    triangles.Max(t => Math.Max(Math.Max(t.triangle.v0.Y, t.triangle.v1.Y), t.triangle.v2.Y)),
                    triangles.Max(t => Math.Max(Math.Max(t.triangle.v0.Z, t.triangle.v1.Z), t.triangle.v2.Z))
                    ));
        }
    }
}
