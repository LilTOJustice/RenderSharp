﻿using MathSharp;

namespace RenderSharp.Scene
{
    /// <summary>
    /// A single instance of a <see cref="Scene2d"/>. It stores deep copies of
    /// all important aspects of the scene it was constructed from. So the
    /// original scene will not be modified from the simulation.
    /// </summary>
    public class Scene2dInstance
    {
        /// <inheritdoc cref="Scene2d.Camera"/>
        public Camera2d Camera { get; }

        /// <inheritdoc cref="Scene2d.Actors"/>
        public Dictionary<string, Actor2d> Actors { get; }

        /// <summary>
        /// Current simulation time for this instance.
        /// </summary>
        public double Time { get; }

        /// <summary>
        /// Index into the scene this instance was constructed from.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Think function to be run for the next instance.
        /// </summary>
        public Scene2d.Scene2dThinkFunc ThinkFunc { get; private set; }

        /// <summary>
        /// Constructs a scene for the start of the simulation.
        /// </summary>
        /// <param name="scene">Scene to copy and simulate off of.</param>
        public Scene2dInstance(Scene2d scene)
        {
            Camera = new Camera2d(new Vec2(scene.Camera.Center), scene.Camera.Zoom, scene.Camera.Rotation);
            Actors = new Dictionary<string, Actor2d>(scene.Actors.Select(keyValue =>
            new KeyValuePair<string, Actor2d>(keyValue.Key,
                new Actor2d(
                    new FVec2(keyValue.Value.Size),
                    new FVec2(keyValue.Value.Position),
                    keyValue.Value.Texture,
                    keyValue.Value.Rotation,
                    keyValue.Value.Shader
                )
            )));
            Time = 0;
            Index = 0;
            ThinkFunc = scene.ThinkFunc;
        }

        /// <summary>
        /// Constructs a scene for intermediate frames of the simulation.
        /// </summary>
        /// <param name="scene">Scene instance to copy and simulate off of.</param>
        /// <param name="time">Simulation time for this instance.</param>
        /// <param name="index">Index into the scene this instance was constructed from.</param>
        public Scene2dInstance(Scene2dInstance scene, double time, int index)
        {
            Camera = new Camera2d(new Vec2(scene.Camera.Center), scene.Camera.Zoom, scene.Camera.Rotation);
            Actors = new Dictionary<string, Actor2d>(scene.Actors.Select(keyValue =>
            new KeyValuePair<string, Actor2d>(keyValue.Key,
                new Actor2d(
                    new FVec2(keyValue.Value.Size),
                    new FVec2(keyValue.Value.Position),
                    keyValue.Value.Texture,
                    keyValue.Value.Rotation,
                    keyValue.Value.Shader
                )
            )));
            Time = time;
            Index = index;
            ThinkFunc = scene.ThinkFunc;
        }
    }
}
