using MathSharp;
using System.ComponentModel;
using System.Numerics;

namespace RenderSharp.Render2d
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

        /// <inheritdoc cref="Scene2d.ActorIndex"/>
        public List<Dictionary<string, Actor2d>> ActorIndex { get; }

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
            ActorIndex = scene.ActorIndex.Select(plane => new Dictionary<string, Actor2d>(
                plane.Select(keyValue =>
                    new KeyValuePair<string, Actor2d>(keyValue.Key, keyValue.Value.Reconstruct())
                    )
                )).ToList();
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
            ActorIndex = scene.ActorIndex.Select(plane => new Dictionary<string, Actor2d>(
                plane.Select(keyValue =>
                    new KeyValuePair<string, Actor2d>(keyValue.Key, keyValue.Value.Reconstruct())
                    )
                )).ToList();
            Time = time;
            Index = index;
            ThinkFunc = scene.ThinkFunc;
        }

        /// <summary>
        /// Retrieves an actor from the scene.
        /// </summary>
        /// <param name="actorId">Id of the actor to retrieve.</param>
        /// <returns></returns>
        public Actor2d this[string actorId]
        {
            get
            {
                foreach (Dictionary<string, Actor2d> plane in ActorIndex)
                {
                    if (plane.ContainsKey(actorId))
                    {
                        return plane[actorId];
                    }
                }

                throw new KeyNotFoundException("Actor not found in index.");
            }
        }
    }
}
