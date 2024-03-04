using MathSharp;

namespace RenderSharp
{
    /// <summary>
    /// Contains Renderer-related utility classes and functions.
    /// </summary>
    internal class Util
    {
        /// <summary>
        /// Contains various vector transformation functions.
        /// </summary>
        public class Transforms
        {
            /// <summary>
            /// Transforms screen-space coordinates to 2d world-space coordinates.
            /// </summary>
            /// <param name="screenSize">Size of the screen.</param>
            /// <param name="screenCoords">Position within the screen.</param>
            /// <param name="cameraCenter">Center of the camera in the world.</param>
            /// <param name="aspectRatio">Aspect ratio for proper screen scaling.</param>
            /// <param name="cameraZoom">Zoom of the camera.</param>
            /// <param name="cameraRotation">Rotation of the camera.</param>
            /// <returns>The world coordinates corresponding to the screen coordinates.</returns>
            public static FVec2 ScreenToWorld2(Vec2 screenSize, Vec2 screenCoords, FVec2 cameraCenter, double aspectRatio, double cameraZoom, double cameraRotation)
            {
                FVec2 result = (new FVec2(screenCoords.X - (screenSize.X / 2),
                         (screenSize.Y / 2) - screenCoords.Y) / screenSize / cameraZoom + cameraCenter).Rotate(cameraRotation);
                result.X *= aspectRatio;
                return result;
            }

            /// <summary>
            /// Transforms world-space coordinates to edge-wrapped background texture space.
            /// </summary>
            /// <param name="worldCoord">Position within the world.</param>
            /// <param name="bgTextureSize">Size of the background texture.</param>
            /// <returns></returns>
            public static Vec2 WorldToBgTexture2(FVec2 worldCoord, Vec2 bgTextureSize)
            {
                Vec2 ind = (Vec2)worldCoord + new Vec2(bgTextureSize.X / 2, bgTextureSize.Y / 2);
                ind = new Vec2(Mod(ind.X, bgTextureSize.X), bgTextureSize.Y - Mod(ind.Y, bgTextureSize.Y) - 1);
                return ind;
            }

            /// <summary>
            /// Transforms world-space coordinates to 2d actor-space coordinates.
            /// </summary>
            /// <param name="worldCoord">Position within the world.</param>
            /// <param name="actorPosition">Actor's position within the world.</param>
            /// <param name="actorSize">Size of the actor.</param>
            /// <param name="actorRotation">Rotation of the actor.</param>
            /// <returns>The actor coordinates corresponding to the world coordinates, or null if the actor is not intersected.</returns>
            public static FVec2? WorldToActor2(FVec2 worldCoord, FVec2 actorPosition, FVec2 actorSize, double actorRotation)
            {
                FVec2 result = (worldCoord - actorPosition).Rotate(actorRotation);
                return (result.X < -actorSize.X / 2
                    || result.Y < -actorSize.Y / 2
                    || result.X > actorSize.X / 2
                    || result.Y > actorSize.Y / 2)
                    ? null : result;
            }

            /// <summary>
            /// Transforms 2d actor-space coordinates to texture-space coordinates.
            /// </summary>
            /// <param name="actorCoords">Actor's position within the world.</param>
            /// <param name="actorSize">Size of the actor.</param>
            /// <param name="textureSize">Size of the actor's texture.</param>
            /// <returns>The texture coordinates corresponding to the actor coordinates, or null if the coordinates are not within the texture.</returns>
            public static Vec2? ActorToTexture2(FVec2 actorCoords, FVec2 actorSize, Vec2 textureSize)
            {
                FVec2 fromTl = new FVec2(
                    actorCoords.X + actorSize.X / 2,
                    actorSize.Y - (actorCoords.Y + actorSize.Y / 2)
                    );
                Vec2 result = (Vec2)(fromTl / actorSize * textureSize);
                return (result.X < 0
                    || result.Y < 0
                    || result.X >= textureSize.X
                    || result.Y >= textureSize.Y)
                    ? null : result;
            }
        }

        /// <summary>
        /// Computes the true modulus. Returns a positive value when modulo is positive.
        /// </summary>
        /// <param name="input">The number to modulus by.</param>
        /// <param name="modulo">The modulo.</param>
        /// <returns></returns>
        public static int Mod(int input, int modulo)
        {
            return (input % modulo + modulo) % modulo;
        }
    }
}
