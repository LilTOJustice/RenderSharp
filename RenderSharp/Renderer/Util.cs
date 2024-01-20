﻿using MathSharp;

namespace RenderSharp.Renderer
{
    /// <summary>
    /// Contains math-related utility classes and functions.
    /// </summary>
    public class Util
    {
        /// <summary>
        /// 
        /// </summary>
        public class Constants
        {
            /// <summary>
            /// Constant 180 / PI.
            /// </summary>
            public const double DEGPERPI = 180 / Math.PI;
        }

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
            /// <param name="cameraZoom">Zoom of the camera.</param>
            /// <param name="cameraRotation">Rotation of the camera.</param>
            /// <returns>The world coordinates corresponding to the screen coordinates.</returns>
            public static FVec2 ScreenToWorld2(Vec2 screenSize, Vec2 screenCoords, Vec2 cameraCenter, double cameraZoom, double cameraRotation)
            {
                FVec2 coords = (new Vec2(screenCoords.X - (screenSize.X / 2),
                         (screenSize.Y / 2) - screenCoords.Y) / cameraZoom + cameraCenter).Rotate(cameraRotation);
                return coords;
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
                FVec2 fromTl = actorCoords + new FVec2(actorSize.X / 2, actorSize.Y / 2);
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