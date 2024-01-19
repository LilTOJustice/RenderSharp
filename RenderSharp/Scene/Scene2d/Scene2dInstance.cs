using MathSharp;

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
        public Scene2d.Camera Camera { get; }

        /// <inheritdoc cref="Scene2d.Actors"/>
        public Dictionary<string, Scene2d.Actor> Actors { get; }

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
            Camera = new Scene2d.Camera(new Vec2(scene.SceneCamera.Center), scene.SceneCamera.Zoom, scene.SceneCamera.Rotation);
            Actors = new Dictionary<string, Scene2d.Actor>(scene.Actors.Select(keyValue =>
            new KeyValuePair<string, Scene2d.Actor>(keyValue.Key,
                new Scene2d.Actor(
                    keyValue.Value.Texture,
                    new FVec2(keyValue.Value.Position),
                    new FVec2(keyValue.Value.Size),
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
            Camera = new Scene2d.Camera(new Vec2(scene.Camera.Center), scene.Camera.Zoom, scene.Camera.Rotation);
            Actors = new Dictionary<string, Scene2d.Actor>(scene.Actors.Select(keyValue =>
            new KeyValuePair<string, Scene2d.Actor>(keyValue.Key,
                new Scene2d.Actor(
                    keyValue.Value.Texture,
                    new FVec2(keyValue.Value.Position),
                    new FVec2(keyValue.Value.Size),
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
