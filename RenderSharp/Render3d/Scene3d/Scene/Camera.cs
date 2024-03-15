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
        public FVec3 Center { get; set; }

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
            Center = new FVec3();
            Fov = new FVec2();
            Rotation = new AVec3();
        }

        internal Camera(Camera camera)
        {
            Center = camera.Center;
            Fov = camera.Fov;
            Rotation = camera.Rotation;
        }

        internal Camera(in FVec3 center, in FVec2 fov, in AVec3 rotation)
        {
            Center = center;
            Fov = fov;
            Rotation = rotation;
        }
    }
}
