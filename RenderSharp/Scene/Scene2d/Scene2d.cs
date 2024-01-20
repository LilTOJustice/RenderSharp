using MathSharp;
using RenderSharp.Common;

namespace RenderSharp.Scene
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
        /// Virtual camera for viewing the scene.
        /// </summary>
        public class Camera
        {
            /// <summary>
            /// World location of the camera.
            /// </summary>
            public Vec2 Center { get; set; }

            /// <summary>
            /// Zoom of the camera.
            /// </summary>
            public double Zoom { get; set; }

            /// <summary>
            /// Rotation of the camera in radians.
            /// </summary>
            public double Rotation { get; set; }

            /// <summary>
            /// Constructs a camera.
            /// </summary>
            /// <param name="center">Center of the camera in world space.</param>
            /// <param name="zoom">Zoom of the camera.</param>
            /// <param name="rotation">Rotation of the camera in radians.</param>
            public Camera(Vec2 center, double zoom, double rotation)
            {
                Center = center;
                Zoom = zoom;
                Rotation = rotation;
            }
        }

        /// <summary>
        /// An object to be rendered from within a scene.
        /// </summary>
        public class Actor
        {
            /// <summary>
            /// World space size of the actor.
            /// </summary>
            public FVec2 Size { get; set; }
            
            /// <summary>
            /// World space width of the actor.
            /// </summary>
            public double Width { get { return Size.X; } set { Size.X = value; } }

            /// <summary>
            /// World space height of the actor.
            /// </summary>
            public double Height { get { return Size.Y; } set { Size.Y = value; } }
            
            /// <summary>
            /// World space position of the actor.
            /// </summary>
            public FVec2 Position { get; set; }
            
            /// <summary>
            /// Rotation of the actor about its center.
            /// </summary>
            public double Rotation { get; set; }
            
            /// <summary>
            /// Texture to render on the actor.
            /// </summary>
            public Texture Texture { get; set; }
            
            /// <summary>
            /// Shader to be applied to the actor's texture.
            /// </summary>
            public FragShader Shader { get; set; }

            /// <summary>
            /// Constructs an actor.
            /// </summary>
            /// <param name="texture">Texture to be rendered on the actor.</param>
            /// <param name="position">World space position of the actor.</param>
            /// <param name="size">World space size of the actor.</param>
            /// <param name="rotation">Rotation of the actor about its center.</param>
            /// <param name="shader">Shader to be applied to the actor's texture.</param>
            public Actor(FVec2 size, FVec2? position = null, Texture? texture = null, double rotation = 0, FragShader? shader = null)
            {
                Texture = texture ?? new Texture((Vec2)size);
                Position = position ?? new FVec2();
                Size = size;
                Rotation = rotation;
                Shader = shader ?? ((in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
            }

            /// <summary>
            /// Clears any active shaders on the actor.
            /// </summary>
            public void ClearShaders()
            {
                Shader = (in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; }; 
            }
        }

        /// <summary>
        /// Framerate of the scene.
        /// </summary>
        public int Framerate { get { return (int)(1 / DeltaTime); } private set { DeltaTime = 1d / value; } }

        /// <summary>
        /// Camera within the scene.
        /// </summary>
        public Camera SceneCamera { get; set; }

        /// <summary>
        /// List of times that each frame of simulation occurs at.
        /// </summary>
        public List<double> TimeSeq { get; private set; }
        
        /// <summary>
        /// Total duration of the simulation for the scene. 0 when the scene is static.
        /// </summary>
        public double Duration { get; private set; }
        
        /// <summary>
        /// Amount of time between each frame of the simulation.
        /// </summary>
        public double DeltaTime { get; private set; } 
        
        /// <summary>
        /// Dictionary of actors indexed by their actorId.
        /// </summary>
        public Dictionary<string, Actor> Actors { get; set; }
        
        /// <summary>
        /// Background texture to use if an actor is not intersected by the renderer.
        /// </summary>
        public Texture BgTexture { get; set; }
        
        /// <summary>
        /// Shader to be run each frame on the background if no actor is intersected by the renderer.
        /// </summary>
        public FragShader Shader { get; set; }
        
        /// <summary>
        /// Think function to be run each frame of the simulation of a dynamic scene.
        /// </summary>
        public Scene2dThinkFunc ThinkFunc { get; set; }

        /// <summary>
        /// Constructs a scene.
        /// </summary>
        /// <param name="framerate">The framerate of the simulation. If 0 or negative, the scene will be considered static.</param>
        /// <param name="duration">The duration of the simulation. If 0 or negative, the scene will be considered static.</param>
        /// <param name="bgColor">The background color to be used if no actor is interesected
        /// by the renderer and no background texture is provided.</param>
        /// <param name="bgTexture">The background texture to be used if no actor is intersected.</param>
        public Scene2d(int framerate = 0, double duration = 0, Texture? bgTexture = null, RGB? bgColor = null) 
        {
            Framerate = framerate;
            BgTexture = bgTexture ?? new Texture(1, 1, bgColor ?? new RGB());
            Duration = duration;
            TimeSeq = new List<double>();
            for (int i = 0; i < framerate * duration; i++)
            {
                TimeSeq.Add(i * DeltaTime);
            }
            Actors = new Dictionary<string, Actor>();
            SceneCamera = new Camera(new Vec2(0, 0), 1, 0);
            Shader = (in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; };
            ThinkFunc = (Scene2dInstance scene, double time, double dt) => { };
        }

        /// <summary>
        /// Registers a new actor in the dictionary with the given actorId.
        /// </summary>
        /// <param name="actor">Actor object to store.</param>
        /// <param name="actorId">Lookup id for the actor into <see cref="Actors"/>.</param>
        public void AddActor(Actor actor, string actorId)
        {
            Actors.Add(actorId, actor);
        }

        /// <summary>
        /// Removes the actor from <see cref="Actors"/>.
        /// </summary>
        /// <param name="actorId">Id for looking up the actor.</param>
        /// <returns></returns>
        public bool RemoveActor(string actorId)
        {
            return Actors.Remove(actorId);
        }

        /// <summary>
        /// Clears all active shaders on the scene background.
        /// </summary>
        public void ClearShaders()
        {
            Shader = (in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; };
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