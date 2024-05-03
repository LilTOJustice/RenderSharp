using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// 3d scene for rendering. Created using a <see cref="SceneBuilder"/>.
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

        internal int Framerate { get { return (int)(1 / DeltaTime); } set { DeltaTime = 1d / value; } }

        internal Dictionary<string, Camera> Cameras { get; set; }

        internal List<double> TimeSeq { get; private set; }
        
        internal double Duration { get; private set; }
        
        internal double DeltaTime { get; private set; } 
        
        internal ThinkFunc Think { get; set; }

        internal Dictionary<string, Actor> Actors { get; set; }

        internal Dictionary<string, PointLight> Lights { get; set; }

        internal Scene(
            int framerate,
            double duration,
            Dictionary<string, Camera> cameras,
            ThinkFunc think,
            Dictionary<string, Actor> actors,
            Dictionary<string, PointLight> lights)
        {
            Framerate = framerate;
            Duration = duration;
            Cameras = cameras;
            Think = think;
            Actors = actors;
            Lights = lights;

            TimeSeq = new List<double>();
            for (int i = 0; i < framerate * duration; i++)
            {
                TimeSeq.Add(i * DeltaTime);
            }
        }

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