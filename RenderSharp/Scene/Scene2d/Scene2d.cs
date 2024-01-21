using MathSharp;

namespace RenderSharp.Render2d
{
    /// <summary>
    /// 2d scene for rendering.
    /// </summary>
    public class Scene2d
    {
        /// <summary>
        /// Think function delegate run each frame on the scene.
        /// </summary>
        /// <param name="scene">Instance of the scene that can be manipulated within the frame.</param>
        /// <param name="time">Current time within the simulation.</param>
        /// <param name="dt">Time between each frame of the simulation.</param>
        public delegate void Scene2dThinkFunc(Scene2dInstance scene, double time, double dt);


        /// <summary>
        /// Framerate of the scene.
        /// </summary>
        public int Framerate { get { return (int)(1 / DeltaTime); } private set { DeltaTime = 1d / value; } }

        /// <summary>
        /// Camera within the scene.
        /// </summary>
        public Camera2d Camera { get; set; }

        /// <summary>
        /// List of times that each frame of simulation occurs at.
        /// </summary>
        public List<double> TimeSeq { get; private set; }
        
        /// <summary>
        /// Total duration of the simulation for the scene in seconds. 0 when the scene is static.
        /// </summary>
        public double Duration { get; private set; }
        
        /// <summary>
        /// Amount of time between each frame of the simulation.
        /// </summary>
        public double DeltaTime { get; private set; } 
        
        /// <summary>
        /// Collection of planes containing dictionaries of actors indexed by their actorId.
        /// </summary>
        public List<Dictionary<string, Actor2d>> ActorIndex { get; set; }
        
        /// <summary>
        /// Background texture to use if an actor is not intersected by the renderer.
        /// </summary>
        public Texture BgTexture { get; set; }
        
        /// <summary>
        /// Shader to be run each frame on the background if no actor is intersected by the renderer.
        /// </summary>
        public FragShader BgShader { get; set; }
        
        /// <summary>
        /// Think function to be run each frame of the simulation of a dynamic scene.
        /// </summary>
        public Scene2dThinkFunc ThinkFunc { get; set; }

        /// <summary>
        /// Constructs a scene.
        /// </summary>
        /// <param name="framerate">The framerate of the simulation. If 0 or negative, the scene will be considered static.</param>
        /// <param name="duration">The duration of the simulation in seconds. If 0 or negative, the scene will be considered static.</param>
        /// <param name="bgColor">The background color to be used if no actor is interesected
        /// by the renderer and no background texture is provided.</param>
        /// <param name="bgTexture">The background texture to be used if no actor is intersected.</param>
        public Scene2d(int framerate = 0, double duration = 0, RGB? bgColor = null, Texture? bgTexture = null) 
        {
            Framerate = framerate;
            BgTexture = bgTexture ?? new Texture(1, 1, bgColor ?? new RGB());
            Duration = duration;
            TimeSeq = new List<double>();
            for (int i = 0; i < framerate * duration; i++)
            {
                TimeSeq.Add(i * DeltaTime);
            }

            ActorIndex = new List<Dictionary<string, Actor2d>>();
            Camera = new Camera2d(new Vec2(0, 0), 1, 0);
            BgShader = (in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; };
            ThinkFunc = (Scene2dInstance scene, double time, double dt) => { };
        }

        /// <summary>
        /// Registers a new actor in the dictionary with the given actorId.
        /// </summary>
        /// <param name="actor">Actor object to store.</param>
        /// <param name="actorId">Lookup id for the actor into <see cref="ActorIndex"/>.</param>
        /// <param name="plane">Virtual plane for the actor to reside in. Lower is closer to camera.</param>
        public void AddActor(Actor2d actor, string actorId, int plane = 0)
        {
            if (plane >= ActorIndex.Count)
            {
                int size = ActorIndex.Count;
                for (int i = 0; i <= plane - size; i++)
                {
                    ActorIndex.Add(new Dictionary<string, Actor2d>());
                }
            }

            ActorIndex[plane].Add(actorId, actor);
        }

        /// <summary>
        /// Removes the actor from all planes that have it in <see cref="ActorIndex"/>.
        /// </summary>
        /// <param name="actorId">Id for looking up the actor.</param>
        /// <returns></returns>
        public bool RemoveActor(string actorId)
        {
            foreach (Dictionary<string, Actor2d> plane in ActorIndex)
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
            ThinkFunc = (Scene2dInstance scene, double time, double dt) => { };
        }

        /// <summary>
        /// Simulates the scene and returns all instances at each time up to the given index.
        /// </summary>
        /// <param name="simulateToIndex">Index into the scene to simulate up to.
        /// If no argument is provided, the simulation will run fully.</param>
        /// <returns></returns>
        public List<Scene2dInstance> Simulate(int? simulateToIndex = null)
        {
            simulateToIndex ??= TimeSeq.Count - 1;
            List<Scene2dInstance> instances = new List<Scene2dInstance>((int)simulateToIndex! + 1);
            Scene2dInstance current = new Scene2dInstance(this);
            instances.Add(current);
            for (int i = 1; i <= simulateToIndex; i++)
            {
                current = new Scene2dInstance(current, TimeSeq[i], i);
                current.ThinkFunc(current, TimeSeq[i], DeltaTime);
                instances.Add(current);
            }

            return instances;
        }
    }
}