using MathSharp;

namespace RenderSharp.Render2d
{
    /// <summary>
    /// Virtual camera for viewing the scene.
    /// </summary>
    public class Camera2d
    {
        /// <summary>
        /// World location of the camera.
        /// </summary>
        public Vec2 Center { get; set; }

        /// <summary>
        /// Zoom of the camera.
        /// </summary>
        public double Zoom { get; set; }

        /// <summary>
        /// Rotation of the camera in radians.
        /// </summary>
        public double Rotation { get; set; }

        /// <summary>
        /// Constructs a camera.
        /// </summary>
        /// <param name="center">Center of the camera in world space.</param>
        /// <param name="zoom">Zoom of the camera.</param>
        /// <param name="rotation">Rotation of the camera in radians.</param>
        public Camera2d(Vec2 center, double zoom, double rotation)
        {
            Center = center;
            Zoom = zoom;
            Rotation = rotation;
        }
    }
}
