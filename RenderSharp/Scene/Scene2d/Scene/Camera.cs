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

        /// <summary>
        /// Constructs a camera centered at (0, 0) with zoom 1 and rotation 0.
        /// </summary>
        internal Camera()
        {
            Center = new FVec2(0, 0);
            Zoom = 1;
            Rotation = 0;
        }

        /// <summary>
        /// <c>Deep</c> copies a camera.
        /// </summary>
        /// <param name="other">Camera to copy from.</param>
        internal Camera(Camera other)
        {
            Center = new(other.Center);
            Zoom = other.Zoom;
            Rotation = other.Rotation;
        }

        /// <summary>
        /// Constructs a camera.
        /// </summary>
        /// <param name="center">Center of the camera in world space.</param>
        /// <param name="zoom">Zoom of the camera.</param>
        /// <param name="rotation">Rotation of the camera in radians.</param>
        internal Camera(FVec2 center, double zoom, double rotation)
        {
            Center = new(center);
            Zoom = zoom;
            Rotation = rotation;
        }
    }
}
