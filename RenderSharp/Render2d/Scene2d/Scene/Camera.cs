using MathSharp;

namespace RenderSharp.Render2d
{
    /// <summary>
    /// Virtual camera for viewing the scene.
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// World location of the camera.
        /// </summary>
        public FVec2 Center { get; set; }

        /// <summary>
        /// Zoom of the camera. Represents the world space vertical length of the screen space.
        /// </summary>
        public double Zoom { get; set; }

        /// <summary>
        /// Rotation of the camera in radians.
        /// </summary>
        public double Rotation { get; set; }

        internal Camera()
        {
            Center = new FVec2(0, 0);
            Zoom = 1;
            Rotation = 0;
        }

        internal Camera(Camera camera)
        {
            Center = camera.Center;
            Zoom = camera.Zoom;
            Rotation = camera.Rotation;
        }

        internal Camera(in FVec2 center, double zoom, double rotation)
        {
            Center = center;
            Zoom = zoom;
            Rotation = rotation;
        }
    }
}
