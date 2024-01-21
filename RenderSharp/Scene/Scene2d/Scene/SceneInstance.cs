using MathSharp;

namespace RenderSharp.Render2d
{
    /// <summary>
    /// A single instance of a <see cref="Scene"/>. It stores deep copies of
    /// all important aspects of the scene it was constructed from. So the
    /// original scene will not be modified from the simulation.
    /// </summary>
    public class SceneInstance
    {

        /// <summary>
        /// Current simulation time for this instance.
        /// </summary>
        public double Time { get; }

        /// <summary>
        /// Index into the scene this instance was constructed from.
        /// </summary>
        public int Index { get; }

        /// <inheritdoc cref="Scene.Camera"/>
        public Camera Camera { get; }

        /// <inheritdoc cref="Scene.BgTexture"/>
        public Texture BgTexture { get; set; }
        
        /// <inheritdoc cref="Scene.BgShader"/>
        public FragShader BgShader { get; set; }

        /// <summary>
        /// Think function to be run for the next instance.
        /// </summary>
        public Scene.ThinkFunc Think { get; private set; }

        internal ActorIndex ActorIndex { get; set; }

        /// <summary>
        /// Constructs a scene for the start of the simulation.
        /// </summary>
        /// <param name="scene">Scene to copy and simulate off of.</param>
        internal SceneInstance(Scene scene)
        {
            Camera = new Camera(scene.Camera.Center, scene.Camera.Zoom, scene.Camera.Rotation);
            ActorIndex = new ActorIndex(scene.ActorIndex);
            Time = 0;
            Index = 0;
            Think = scene.Think;
            BgTexture = scene.BgTexture;
            BgShader = scene.BgShader;
        }

        /// <summary>
        /// Constructs a scene for intermediate frames of the simulation.
        /// </summary>
        /// <param name="scene">Scene instance to copy and simulate off of.</param>
        /// <param name="time">Simulation time for this instance.</param>
        /// <param name="index">Index into the scene this instance was constructed from.</param>
        internal SceneInstance(SceneInstance scene, double time, int index)
        {
            Camera = new Camera(new FVec2(scene.Camera.Center), scene.Camera.Zoom, scene.Camera.Rotation);
            ActorIndex = new ActorIndex(scene.ActorIndex);
            Time = time;
            Index = index;
            Think = scene.Think;
            BgTexture = scene.BgTexture;
            BgShader = scene.BgShader;
        }

        /// <summary>
        /// Removes the actor from all planes that have it in <see cref="ActorIndex"/>.
        /// </summary>
        /// <param name="actorId">Id for looking up the actor.</param>
        /// <returns></returns>
        public bool RemoveActor(string actorId)
        {
            foreach (Dictionary<string, Actor> plane in ActorIndex)
            {
                if (plane.ContainsKey(actorId))
                {
                    plane.Remove(actorId);
                }
            }

            return false;
        }

        /// <summary>
        /// Clears all active shaders on the scene background.
        /// </summary>
        public void ClearShaders()
        {
            BgShader = (in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; };
        }

        /// <summary>
        /// Clears all think functions on the scene.
        /// </summary>
        public void ClearThinkFunc()
        {
            Think = (SceneInstance scene, double time, double dt) => { };
        }

        /// <summary>
        /// Retrieves an actor from the scene.
        /// </summary>
        /// <param name="actorId">Id of the actor to retrieve.</param>
        /// <returns></returns>
        public Actor this[string actorId]
        {
            get
            {
                return ActorIndex[actorId];
            }
        }
    }
}
