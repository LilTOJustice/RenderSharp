using MathSharp;

namespace RenderSharp.Render2d
{
    /// <summary>
    /// 2d scene for rendering. Created using a <see cref="SceneBuilder"/>.
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// Think function delegate run each frame on the scene.
        /// </summary>
        /// <param name="scene">Instance of the scene that can be manipulated within the frame.</param>
        /// <param name="time">Current time within the simulation.</param>
        /// <param name="dt">Time between each frame of the simulation.</param>
        public delegate void ThinkFunc(SceneInstance scene, double time, double dt);

        /// <summary>
        /// Framerate of the scene.
        /// </summary>
        internal int Framerate { get { return (int)(1 / DeltaTime); } set { DeltaTime = 1d / value; } }

        /// <summary>
        /// All cameras within the scene. First added is the starting primary.
        /// </summary>
        internal Dictionary<string, Camera> Cameras { get; set; }

        /// <summary>
        /// List of times that each frame of simulation occurs at.
        /// </summary>
        internal List<double> TimeSeq { get; private set; }
        
        /// <summary>
        /// Total duration of the simulation for the scene in seconds. 0 when the scene is static.
        /// </summary>
        internal double Duration { get; private set; }
        
        /// <summary>
        /// Amount of time between each frame of the simulation.
        /// </summary>
        internal double DeltaTime { get; private set; } 
        
        /// <summary>
        /// Background texture to use if an actor is not intersected by the renderer.
        /// </summary>
        internal Texture BgTexture { get; set; }
        
        /// <summary>
        /// Shader to be run each frame on the background if no actor is intersected by the renderer.
        /// </summary>
        internal FragShader BgShader { get; set; }
        
        /// <summary>
        /// Think function to be run each frame of the simulation of a dynamic scene.
        /// </summary>
        internal ThinkFunc Think { get; set; }

        /// <summary>
        /// Collection of planes containing dictionaries of actors indexed by their actorId.
        /// </summary>
        internal ActorIndex ActorIndex { get; set; }

        /// <summary>
        /// Constructs a scene.
        /// </summary>
        /// <param name="framerate">The framerate of the simulation. If 0 or negative, the scene will be considered static.</param>
        /// <param name="duration">The duration of the simulation in seconds. If 0 or negative, the scene will be considered static.</param>
        /// <param name="cameras">List of cameras in the scene.</param>
        /// <param name="bgTexture">The background texture to be used if no actor is intersected.</param>
        /// <param name="bgShader">The shader run on each pixel of the background texture.</param>
        /// <param name="think">Function that runs on instances of the scene during simulation.</param>
        /// <param name="actorIndex">Lookup for actors in the scene.</param>
        internal Scene(
            int framerate,
            double duration,
            Dictionary<string, Camera> cameras,
            Texture bgTexture,
            FragShader bgShader,
            ThinkFunc think,
            ActorIndex actorIndex)
        {
            Framerate = framerate;
            Duration = duration;
            Cameras = cameras;
            BgTexture = bgTexture;
            BgShader = bgShader;
            Think = think;
            ActorIndex = actorIndex;

            TimeSeq = new List<double>();
            for (int i = 0; i < framerate * duration; i++)
            {
                TimeSeq.Add(i * DeltaTime);
            }
        }

        /// <summary>
        /// Simulates the scene and returns all instances at each time up to the given index.
        /// </summary>
        /// <param name="simulateToIndex">Index into the scene to simulate up to.
        /// If no argument is provided, the simulation will run fully.</param>
        /// <returns></returns>
        internal List<SceneInstance> Simulate(int? simulateToIndex = null)
        {
            simulateToIndex ??= TimeSeq.Count - 1;
            List<SceneInstance> instances = new List<SceneInstance>((int)simulateToIndex! + 1);
            SceneInstance current = new SceneInstance(this);
            instances.Add(current);
            for (int i = 1; i <= simulateToIndex; i++)
            {
                current = new SceneInstance(current, TimeSeq[i], i);
                current.Think(current, TimeSeq[i], DeltaTime);
                instances.Add(current);
            }

            return instances;
        }
    }
}