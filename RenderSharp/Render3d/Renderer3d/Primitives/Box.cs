using MathSharp;

namespace RenderSharp.Render3d
{
    internal struct Box
    {
        private FVec3 position;
        private FVec3 size;

        public Box(FVec3 position, FVec3 size)
        {
            this.position = position;
            this.size = size;
        }

        private bool TestX(in FVec3 test, out double result)
        {
            result = 0;
            double a = test.X * test.X;
            double b = -2 * test.X * position.X;
            double c = position.X * position.X - size.X * size.X;
            double sqrt = b * b - 4 * a * c;
            if (sqrt < 0)
            {
                return false;
            }

            double sqrtResult = Math.Sqrt(sqrt);
            double plust = (-b + sqrtResult) / (2 * a);
            double minust = (-b - sqrtResult) / (2 * a);
            if (plust < 0 && minust < 0)
            {
                return false;
            }
            else if (plust < 0)
            {
                result = minust;
                return true;
            }
            else if (minust < 0)
            {
                result = plust;
                return true;
            }

            result = Math.Min(plust, minust);
            return true;
        }

        private bool TestY(in FVec3 test, out double result)
        {
            result = 0;
            double a = test.Y * test.Y;
            double b = -2 * test.Y * position.Y;
            double c = position.Y * position.Y - size.Y * size.Y;
            double sqrt = b * b - 4 * a * c;
            if (sqrt < 0)
            {
                return false;
            }

            double sqrtResult = Math.Sqrt(sqrt);
            double plust = (-b + sqrtResult) / (2 * a);
            double minust = (-b - sqrtResult) / (2 * a);
            if (plust < 0 && minust < 0)
            {
                return false;
            }
            else if (plust < 0)
            {
                result = minust;
                return true;
            }
            else if (minust < 0)
            {
                result = plust;
                return true;
            }

            result = Math.Min(plust, minust);
            return true;
        }

        private bool TestZ(in FVec3 test, out double result)
        {
            result = 0;
            double a = test.Z * test.Z;
            double b = -2 * test.Z * position.Z;
            double c = position.Z * position.Z - size.Z * size.Z;
            double sqrt = b * b - 4 * a * c;
            if (sqrt < 0)
            {
                return false;
            }

            double sqrtResult = Math.Sqrt(sqrt);
            double plust = (-b + sqrtResult) / (2 * a);
            double minust = (-b - sqrtResult) / (2 * a);
            if (plust < 0 && minust < 0)
            {
                return false;
            }
            else if (plust < 0)
            {
                result = minust;
                return true;
            }
            else if (minust < 0)
            {
                result = plust;
                return true;
            }

            result = Math.Min(plust, minust);
            return true;
        }

        private bool Intersects(in FVec3 test)
        {
            double t;
            if (TestX(test, out t))
            {
                double resultX = Math.Abs(test.X * t - position.X) / size.X;
                if (resultX > Math.Abs(test.Y * t - position.Y) / size.Y
                    && resultX > Math.Abs(test.Z * t - position.Z) / size.Z)
                {
                    return true;
                }
            }

            if (TestY(test, out t))
            {
                double resultY = Math.Abs(test.Y * t - position.Y) / size.Y;
                if (resultY > Math.Abs(test.X * t - position.X) / size.X
                    && resultY > Math.Abs(test.Z * t - position.Z) / size.Z)
                {
                    return true;
                }
            }
            
            if (TestZ(test, out t))
            {
                double resultZ = Math.Abs(test.Z * t - position.Z) / size.Z;
                if (resultZ > Math.Abs(test.X * t - position.X) / size.X
                    && resultZ > Math.Abs(test.Y * t - position.Y) / size.Y)
                {
                    return true;
                }
            }

            return false;
        }

        public RGBA Sample(in FVec3 worldVec)
        {
            return Intersects(worldVec) ? new RGBA(255, 255, 255, 255) : new RGBA();
        }
    }
}
