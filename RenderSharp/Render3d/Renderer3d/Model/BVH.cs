using MathSharp;

namespace RenderSharp.Render3d
{
    internal class BVH
    {
        public enum Dim
        {
            X,
            Y,
            Z
        }

        private FaceTriangle[] allTriangles;

        private BVH? left, right;

        private BoundingBox boundingBox;

        private FaceTriangle? triangle;

        public BVH(FaceTriangle[] triangles, Dim sortDim = Dim.X)
            : this(
                  triangles,
                  triangles.OrderBy(t => t.triangle.centroid.X).Select(t => t.GetHashCode()).ToList(),
                  triangles.OrderBy(t => t.triangle.centroid.Y).Select(t => t.GetHashCode()).ToList(),
                  triangles.OrderBy(t => t.triangle.centroid.Z).Select(t => t.GetHashCode()).ToList(),
                  sortDim)
        { }

        public BVH(FaceTriangle[] triangles, List<int> sortedX, List<int> sortedY, List<int> sortedZ, Dim splitDim)
        {
            allTriangles = triangles;
            boundingBox = GetBoundingBox(sortedX);

            int sortedXCount = sortedX.Count();
            int sortedYCount = sortedY.Count();
            int sortedZCount = sortedZ.Count();

            if (sortedXCount == 1)
            {
                triangle = triangles[sortedX.First()];
                return;
            }

            switch (splitDim)
            {
                case Dim.X:
                    List<int> leftTrianglesX = sortedX.Take(sortedXCount / 2).ToList();
                    List<int> rightTrianglesX = sortedX.TakeLast(sortedXCount / 2 + sortedXCount % 2).ToList();

                    left = new BVH(
                        triangles,
                        leftTrianglesX,
                        sortedY.Intersect(leftTrianglesX).ToList(),
                        sortedZ.Intersect(leftTrianglesX).ToList(),
                        Dim.Y);
                    right = new BVH(
                        triangles,
                        rightTrianglesX,
                        sortedY.Intersect(rightTrianglesX).ToList(),
                        sortedZ.Intersect(rightTrianglesX).ToList(),
                        Dim.Y);
                    break;
                case Dim.Y:
                    List<int> leftTrianglesY = sortedY.Take(sortedYCount / 2).ToList();
                    List<int> rightTrianglesY = sortedY.TakeLast(sortedYCount / 2 + sortedYCount % 2).ToList();

                    left = new BVH(
                        triangles,
                        sortedX.Intersect(leftTrianglesY).ToList(),
                        leftTrianglesY,
                        sortedZ.Intersect(leftTrianglesY).ToList(),
                        Dim.Z);
                    right = new BVH(
                        triangles,
                        sortedX.Intersect(rightTrianglesY).ToList(),
                        rightTrianglesY,
                        sortedZ.Intersect(rightTrianglesY).ToList(),
                        Dim.Z);
                    break;
                case Dim.Z:
                    List<int> leftTrianglesZ = sortedZ.Take(sortedZCount / 2).ToList();
                    List<int> rightTrianglesZ = sortedZ.TakeLast(sortedZCount / 2 + sortedZCount % 2).ToList();

                    left = new BVH(
                        triangles,
                        sortedX.Intersect(leftTrianglesZ).ToList(),
                        sortedY.Intersect(leftTrianglesZ).ToList(),
                        leftTrianglesZ,
                        Dim.X);
                    right = new BVH(
                        triangles,
                        sortedX.Intersect(rightTrianglesZ).ToList(),
                        sortedY.Intersect(rightTrianglesZ).ToList(),
                        rightTrianglesZ,
                        Dim.X);
                    break;
            }
        }

        public HashSet<FaceTriangle> GetPotentialIntersectingTriangles(in Ray ray)
        {
            if (!boundingBox.Intersects(ray))
            {
                return new();
            }

            if (triangle != null)
            {
                return new() { (FaceTriangle)triangle };
            }

            HashSet<FaceTriangle> triangles = new();
            triangles.UnionWith(left!.GetPotentialIntersectingTriangles(ray));
            triangles.UnionWith(right!.GetPotentialIntersectingTriangles(ray));
            return triangles;
        }

        private BoundingBox GetBoundingBox(List<int> ids)
        {
            List<FaceTriangle> triangles = ids.Select(id => allTriangles[id]).ToList();
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
