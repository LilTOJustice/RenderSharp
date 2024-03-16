using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Virtual camera for viewing the scene.
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// World location of the camera.
        /// </summary>
        public FVec3 Position { get; set; }

        /// <summary>
        /// Focal length of the camera.
        /// </summary>
        public double FocalLength { get; set; }

        /// <summary>
        /// Fov of the camera.
        /// </summary>
        public FVec2 Fov { get; set; }

        /// <summary>
        /// Rotation of the camera in radians.
        /// </summary>
        public AVec3 Rotation { get; set; }

        internal Camera()
        {
            Position = new FVec3();
            FocalLength = 1;
            Fov = new FVec2(Math.PI / 2, Math.PI / 2);
            Rotation = new AVec3();
        }

        internal Camera(Camera camera)
        {
            Position = camera.Position;
            FocalLength = camera.FocalLength;
            Fov = camera.Fov;
            Rotation = camera.Rotation;
        }

        internal Camera(in FVec3 center, double focalLength, in FVec2 fov, in AVec3 rotation)
        {
            Position = center;
            FocalLength = focalLength;
            Fov = fov;
            Rotation = rotation;
        }
    }
}
