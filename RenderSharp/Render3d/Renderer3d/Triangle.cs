﻿using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Represents a single triangle in 3D space.
    /// </summary>
    public struct Triangle
    {
        internal readonly FVec3 v0, v1, v2;
        FVec3 v01, v12, v20;
        FVec3 normal;
        FVec3 unitNorm;
        double d;

        // v0 , v1, v2 are the vertices of the triangle, ordered counter-clockwise
        /// <summary>
        /// Constructs a triangle with vertices v0, v1, and v2.
        /// <b>The vertices should be ordered counter-clockwise.</b>
        /// </summary>
        public Triangle(FVec3 v0, FVec3 v1, FVec3 v2)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
            v01 = v1 - v0;
            v12 = v2 - v1;
            v20 = v0 - v2;
            normal = (v2 - v0).Cross(v01);
            unitNorm = normal.Norm();
            d = -unitNorm.Dot(v0);
        }

        private bool Within(FVec3 test)
        {
            double dot = test.Dot(unitNorm);

            // Check if we are facing the triangle side-on
            if (dot == 0)
            {
                return false;
            }

            double t = - d / dot;

            // Check if the intersection is behind the near plane.
            if (t < 0)
            {
                return false;
            }
            
            FVec3 intersection = test * t;

            return v01.Cross(intersection - v0).Dot(unitNorm) <= 0 &&
                   v12.Cross(intersection - v1).Dot(unitNorm) <= 0 &&
                   v20.Cross(intersection - v2).Dot(unitNorm) <= 0;
        }

        internal RGBA Sample(in FVec3 worldVec)
        {
            return Within(worldVec) ? new RGBA(255, 255, 255, 255) : new RGBA();
        }
    }
}
