using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// A single instance of a <see cref="Scene"/>. It stores deep copies of
    /// all important aspects of the scene it was constructed from. So the
    /// original scene will not be modified from the simulation.
    /// </summary>
    public class SceneInstance
    {
        string primaryCameraKey;

        /// <summary>
        /// Current simulation time for this instance.
        /// </summary>
        public double Time { get; }

        /// <summary>
        /// Index into the scene this instance was constructed from.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Current primary camera for the scene. Set using <see cref="SetPrimaryCamera(string)"/>.
        /// </summary>
        public Camera Camera { get; private set; }

        /// <summary>
        /// All cameras in the scene.
        /// </summary>
        private Dictionary<string, Camera> Cameras { get; }

        /// <inheritdoc cref="Scene.BgTexture"/>
        public Texture BgTexture { get; set; }

        /// <inheritdoc cref="Scene.BgTextureWorldSize"/>
        public FVec2 BgTextureWorldSize { get; set; }
        
        /// <inheritdoc cref="Scene.BgFragShader"/>
        public FragShader BgFragShader { get; set; }

        /// <inheritdoc cref="Scene.BgCoordShader"/>
        public CoordShader BgCoordShader { get; set; }

        /// <summary>
        /// Think function to be run for the next instance.
        /// </summary>
        public Scene.ThinkFunc Think { get; private set; }

        internal ActorIndex ActorIndex { get; set; }

        internal SceneInstance(Scene scene)
        {
            Cameras = new Dictionary<string, Camera>(
                scene.Cameras.Select(pair => new KeyValuePair<string, Camera>(pair.Key, new Camera(pair.Value))));
            primaryCameraKey = Cameras.Keys.First();
            Camera = Cameras[primaryCameraKey];
            ActorIndex = new ActorIndex(scene.ActorIndex);
            Time = 0;
            Index = 0;
            Think = scene.Think;
            BgTexture = scene.BgTexture;
            BgTextureWorldSize = scene.BgTextureWorldSize;
            BgFragShader = scene.BgFragShader;
            BgCoordShader = scene.BgCoordShader;
        }

        internal SceneInstance(SceneInstance scene, double time, int index)
        {
            Cameras = new Dictionary<string, Camera>(
                scene.Cameras.Select(pair => new KeyValuePair<string, Camera>(pair.Key, new Camera(pair.Value))));
            primaryCameraKey = Cameras.Keys.First();
            Camera = Cameras[primaryCameraKey];
            ActorIndex = new ActorIndex(scene.ActorIndex);
            Time = time;
            Index = index;
            Think = scene.Think;
            BgTexture = scene.BgTexture;
            BgTextureWorldSize = scene.BgTextureWorldSize;
            BgFragShader = scene.BgFragShader;
            BgCoordShader = scene.BgCoordShader;
        }

        /// <summary>
        /// Removes the actor from all planes that have it in <see cref="ActorIndex"/>.
        /// </summary>
        /// <param name="actorId">Id for looking up the actor.</param>
        /// <returns>Whether the actor could be removed.</returns>
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
            BgFragShader = (FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; };
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
                return ActorIndex[actorId].Item2;
            }
        }

        /// <summary>
        /// Adds a camera to the scene.
        /// </summary>
        /// <param name="camera">Camera to add.</param>
        /// <param name="name">Name of the camera.</param>
        public void AddCamera(Camera camera, string name)
        {
            Cameras.Add(name, camera);
        }

        /// <summary>
        /// Removes a camera from the scene.
        /// </summary>
        /// <param name="name">Name of the camera.</param>
        public void RemoveCamera(string name)
        {
            Cameras.Remove(name);
        }

        /// <summary>
        /// Sets the primary camera for the scene.
        /// </summary>
        /// <param name="name">The name of the camera to set to.</param>
        public void SetPrimaryCamera(string name)
        {
            Camera = Cameras[name];
            primaryCameraKey = name;
        }

        /// <summary>
        /// Gets the plane that the actor is in.
        /// </summary>
        /// <param name="actorId">Id of the actor.</param>
        /// <returns>The plane the actor resides in, if it exists.</returns>
        public int GetActorPlane(string actorId)
        {
            return ActorIndex[actorId].Item1;
        }

        /// <summary>
        /// Move the actor from its current plane to another.
        /// </summary>
        /// <param name="actorId">Actor to move.</param>
        /// <param name="plane">Plane to move to.</param>
        public void MoveActor(string actorId, int plane)
        {
            (int oldPlane, Actor actor) = ActorIndex[actorId];
            ActorIndex[plane].Add(actorId, actor);
            ActorIndex[oldPlane].Remove(actorId);
        }

        /// <summary>
        /// Add a new actor to the scene.
        /// </summary>
        /// <param name="actor">Actor to add (build with <see cref="ActorBuilder"/>).</param>
        /// <param name="actorId">Unique Id for the actor.</param>
        /// <param name="plane">Plane for the actor to reside in.</param>
        public void AddActor(ActorBuilder actor, string actorId, int plane)
        {
            ActorIndex[plane].Add(actorId, actor.Build());
        }

        /// <summary>
        /// Gets an actor as <see cref="Actor"/>.
        /// </summary>
        /// <param name="actorId">Id of the actor.</param>
        /// <returns>An actor as <see cref="Actor"/> type.</returns>
        public Actor GetActor(string actorId)
        {
            return this[actorId];
        }

        /// <summary>
        /// Gets an actor as <see cref="Line"/>.
        /// </summary>
        /// <param name="actorId">Id of the actor.</param>
        /// <returns>An actor as <see cref="Line"/> type.</returns>
        public Line GetLine(string actorId)
        {
            return (Line)this[actorId];
        }
    }
}
