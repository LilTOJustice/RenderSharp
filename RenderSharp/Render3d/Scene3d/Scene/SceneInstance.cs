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

        /// <summary>
        /// Think function to be run for the next instance.
        /// </summary>
        public Scene.ThinkFunc Think { get; private set; }

        internal Dictionary<string, Actor> Actors { get; set; }

        internal SceneInstance(Scene scene)
        {
            Cameras = new Dictionary<string, Camera>(
                scene.Cameras.Select(pair => new KeyValuePair<string, Camera>(pair.Key, new Camera(pair.Value))));
            primaryCameraKey = Cameras.Keys.First();
            Camera = Cameras[primaryCameraKey];
            Actors = new Dictionary<string, Actor>(
                scene.Actors.Select(pair => new KeyValuePair<string, Actor>(pair.Key, pair.Value.Copy())));
            Time = 0;
            Index = 0;
            Think = scene.Think;
        }

        internal SceneInstance(SceneInstance scene, double time, int index)
        {
            Cameras = new Dictionary<string, Camera>(
                scene.Cameras.Select(pair => new KeyValuePair<string, Camera>(pair.Key, new Camera(pair.Value))));
            primaryCameraKey = Cameras.Keys.First();
            Camera = Cameras[primaryCameraKey];
            Actors = new Dictionary<string, Actor>(
                scene.Actors.Select(pair => new KeyValuePair<string, Actor>(pair.Key, pair.Value.Copy())));
            Time = time;
            Index = index;
            Think = scene.Think;
        }

        /// <summary>
        /// Removes the actor from all planes that have it in <see cref="Actors"/>.
        /// </summary>
        /// <param name="actorId">Id for looking up the actor.</param>
        /// <returns>Whether the actor could be removed.</returns>
        public bool RemoveActor(string actorId)
        {
            return Actors.Remove(actorId);
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
        public Actor this[string actorId]
        {
            get
            {
                return Actors[actorId];
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
        /// Add a new actor to the scene.
        /// </summary>
        /// <param name="actor">Actor to add (build with <see cref="ActorBuilder"/>).</param>
        /// <param name="actorId">Unique Id for the actor.</param>
        public void AddActor(ActorBuilder actor, string actorId)
        {
            Actors.Add(actorId, actor.Build());
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
    }
}
