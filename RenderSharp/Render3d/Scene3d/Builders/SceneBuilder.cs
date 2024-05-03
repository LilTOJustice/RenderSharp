using MathSharp;

namespace RenderSharp.Render3d
{

    /// <summary>
    /// Builder for the <see cref="Scene"/> class. Use this to create a scene.
    /// </summary>
    public class SceneBuilder
    {
        /// <summary>
        /// Next, choose the <see cref="Scene.Duration"/> with <see cref="WithDuration(float)"/>
        /// </summary>
        public class FramerateStep
        {
            private int framerate;

            internal FramerateStep(int framerate)
            {
                this.framerate = framerate;
            }

            /// <inheritdoc cref="Scene.Duration"/>
            public FinalStep WithDuration(float duration)
            {
                return new FinalStep(framerate, duration);
            }
        }

        /// <summary>
        /// Next, choose the <see cref="Scene.Framerate"/> with <see cref="WithFramerate(int)"/>
        /// </summary>
        public class DynamicStep
        {
            internal DynamicStep() { }

            /// <inheritdoc cref="Scene.Framerate"/>
            public FramerateStep WithFramerate(int framerate)
            {
                return new FramerateStep(framerate);
            }
        }

        /// <summary>
        /// Now add any additional properties to the scene and <see cref="Build"/> when done.
        /// </summary>
        public class FinalStep
        {
            private int framerate;
            private double duration;
            private Dictionary<string, Camera> cameras;
            private Scene.ThinkFunc? think;
            private Texture? skyboxTexture;
            private Dictionary<string, Actor> actors;
            private Dictionary<string, PointLight> lights;

            internal FinalStep(int framerate, double duration)
            {
                this.framerate = framerate;
                this.duration = duration;
                actors = new Dictionary<string, Actor>();
                cameras = new Dictionary<string, Camera>();
                lights = new Dictionary<string, PointLight>();
            }

            /// <summary>
            /// Camera to add to the scene. The first to be added is the starting camera for the scene.
            /// If no cameras are added, a default camera will be created and named "main".
            /// </summary>
            /// <param name="center">Center of the camera in world space.</param>
            /// <param name="fov">Field of view of the camera.</param>
            /// <param name="focalLength">Focal length (distance from the near plane) in world space of the camera.
            /// If it is 0, orthographic projection will be used <see href="https://en.wikipedia.org/wiki/Orthographic_projection"/>.</param>
            /// <param name="rotation">Rotation of the camera in world space.</param>
            /// <param name="name">Name of the camera.</param>
            public FinalStep WithCamera(
                string name,
                in FVec3? center = null,
                in RVec3? rotation = null,
                in RVec2? fov = null,
                double focalLength = 1)
            {
                cameras.Add(name, new Camera(center ?? new FVec3(), focalLength, fov ?? new DVec2(90, 90), rotation ?? new RVec3()));
                return this;
            }

            /// <inheritdoc cref="Scene.Think"/>
            public FinalStep WithThink(Scene.ThinkFunc think)
            {
                this.think += think;
                return this;
            }

            /// <summary>
            /// Create an <see cref="ActorBuilder"/> and add properties to the actor here.
            /// </summary>
            /// <param name="actorBuilder">Builder to modify and pass.</param>
            /// <param name="actorId">Id of the actor for accessing it by <see cref="SceneInstance.this[string]"/>.</param>
            public FinalStep WithActor(string actorId, ActorBuilder actorBuilder)
            {
                Actor newActor = actorBuilder.Build();
                actors.Add(actorId, newActor);
                return this;
            }

            /// <summary>
            /// Adds a point light to the scene.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="position"></param>
            public FinalStep WithPointLight(string name, in FVec3 position)
            {
                lights.Add(name, new PointLight(position));
                return this;
            }

            /// <summary>
            /// Sets the skybox texture for the scene.
            /// </summary>
            /// <param name="texture">A spherical skybox texture.</param>
            public FinalStep WithSkyboxTexture(Texture texture)
            {
                skyboxTexture = texture;
                return this;
            }

            /// <summary>
            /// Builds the scene.
            /// </summary>
            /// <returns>A new <see cref="Scene"/>.</returns>
            public Scene Build()
            {
                if (cameras.Count == 0)
                {
                    cameras.Add("main", new Camera());
                }

                think ??= (SceneInstance scene, double time, double dt) => { };
                skyboxTexture ??= new Texture(1, 1);

                return new Scene(
                    framerate,
                    duration,
                    new Dictionary<string, Camera>(
                        cameras.Select(pair => new KeyValuePair<string, Camera>(pair.Key, new Camera(pair.Value)))),
                    think,
                    skyboxTexture,
                    new Dictionary<string, Actor>(
                        actors.Select(pair => new KeyValuePair<string, Actor>(pair.Key, pair.Value.Copy()))),
                    new Dictionary<string, PointLight>(
                        lights.Select(pair => new KeyValuePair<string, PointLight>(pair.Key, new PointLight(pair.Value.Position)))));
            }
        }

        /// <summary>
        /// Pick this if you want a dynamic scene (capable of video rendering).
        /// </summary>
        /// <returns></returns>
        static public DynamicStep MakeDynamic()
        {
            return new DynamicStep();
        }

        /// <summary>
        /// Pick this if you want a static scene (only renders an image).
        /// </summary>
        /// <returns></returns>
        static public FinalStep MakeStatic()
        {
            return new FinalStep(0, 0);
        }
    }
}
