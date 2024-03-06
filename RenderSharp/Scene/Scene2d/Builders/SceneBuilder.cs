using MathSharp;

namespace RenderSharp.Render2d
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
        public OptionalsStep WithDuration(float duration)
        {
            return new OptionalsStep(framerate, duration);
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
    public class OptionalsStep
    {
        private int framerate;
        private double duration;
        private Dictionary<string, Camera> cameras;
        private RGBA? bgColor;
        private Texture? bgTexture;
        private FVec2? bgTextureWorldSize;
        private FragShader? bgFragShader;
        private CoordShader? bgCoordShader;
        private Scene.ThinkFunc? think;
        private ActorIndex actorIndex;

        internal OptionalsStep(int framerate, double duration)
        {
            this.framerate = framerate;
            this.duration = duration;
            actorIndex = new ActorIndex();
            cameras = new Dictionary<string, Camera>();
        }

        /// <summary>
        /// Camera to add to the scene. The first to be added is the starting camera for the scene.
        /// If no cameras are added, a default camera will be created and named "main".
        /// </summary>
        /// <param name="center">Center of the camera in world space.</param>
        /// <param name="zoom">Zoom of the camera.</param>
        /// <param name="rotation">Rotation of the camera in world space.</param>
        /// <param name="name">Name of the camera.</param>
        public OptionalsStep WithCamera(string name, FVec2 center, double zoom = 1, double rotation = 0)
        {
            cameras.Add(name, new Camera(new FVec2(center), zoom, rotation));
            return this;
        }

        /// <summary>
        /// The fill color of the scene's background if there is no <see cref="Scene.BgTexture"/>.
        /// </summary>
        /// <param name="bgColor">Background color.</param>
        public OptionalsStep WithBgColor(RGBA bgColor)
        {
            this.bgColor = new RGBA(bgColor);
            return this;
        }

        /// <inheritdoc cref="Scene.BgTexture"/>
        /// <param name="bgTexture"></param>
        public OptionalsStep WithBgTexture(Texture bgTexture)
        {
            this.bgTexture = bgTexture;
            return this;
        }

        /// <inheritdoc cref="Scene.BgTextureWorldSize"/>
        /// <param name="bgTextureWorldSize">Size of the texture in world space.
        /// If excluded or either component is 0, the texture will fill the screen.</param>
        /// <returns></returns>
        public OptionalsStep WithBgTextureWorldSize(FVec2 bgTextureWorldSize)
        {
            this.bgTextureWorldSize = new FVec2(bgTextureWorldSize);
            return this;
        }

        /// <inheritdoc cref="Scene.BgFragShader"/>
        public OptionalsStep WithBgShader(FragShader bgShader)
        {
            bgFragShader += bgShader;
            return this;
        }

        /// <inheritdoc cref="Scene.BgCoordShader"/>
        public OptionalsStep WithBgShader(CoordShader bgShader)
        {
            bgCoordShader += bgShader;
            return this;
        }

        /// <inheritdoc cref="Scene.Think"/>
        public OptionalsStep WithThink(Scene.ThinkFunc think)
        {
            this.think += think;
            return this;
        }

        /// <summary>
        /// Create an <see cref="ActorBuilder"/> and add properties to the actor here.
        /// </summary>
        /// <param name="actorBuilder">Builder to modify and pass.</param>
        /// <param name="actorId">Id of the actor for accessing it by <see cref="SceneInstance.this[string]"/>.</param>
        /// <param name="plane">Plane in the scene to place the actor in.</param>
        public OptionalsStep WithActor(ActorBuilder actorBuilder, string actorId, int plane = 0)
        {
            actorIndex.EnsurePlaneExists(plane);
            actorIndex[plane].Add(actorId, actorBuilder.Build());
            return this;
        }

        /// <summary>
        /// Create a <see cref="LineBuilder"/> and add properties to the line here.
        /// </summary>
        /// <param name="lineBuilder">Builder to modify and pass.</param>
        /// <param name="actorId">Id of the actor for accessing it by <see cref="SceneInstance.this[string]"/>.</param>
        /// <param name="plane">Plane in the scene to place the line in.</param>
        public OptionalsStep WithActor(LineBuilder lineBuilder, string actorId, int plane = 0)
        {
            actorIndex.EnsurePlaneExists(plane);
            actorIndex[plane].Add(actorId, lineBuilder.Build());
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

            bgTexture ??= new Texture(1, 1, bgColor);
            bgTextureWorldSize ??= new FVec2();
            bgFragShader ??= (in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; };
            bgCoordShader ??= (in Vec2 vertIn, out Vec2 vertOut, Vec2 size, double time) => { vertOut = vertIn; };
            think ??= (SceneInstance scene, double time, double dt) => { };

            return new Scene(
                framerate,
                duration,
                new Dictionary<string, Camera>(
                    cameras.Select(pair => new KeyValuePair<string, Camera>(pair.Key, new Camera(pair.Value)))),
                bgTexture,
                bgTextureWorldSize,
                bgFragShader,
                bgCoordShader,
                think,
                new ActorIndex(actorIndex));
        }
    }

    /// <summary>
    /// Builder for the <see cref="Scene"/> class. Use this to create a scene.
    /// </summary>
    public abstract class SceneBuilder
    {
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
        static public OptionalsStep MakeStatic()
        {
            return new OptionalsStep(0, 0);
        }
    }
}
