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
        private Camera? camera;
        private RGBA? bgColor;
        private Texture? bgTexture;
        private FragShader bgShader = (in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; };
        private Scene.ThinkFunc think = (SceneInstance scene, double time, double dt) => { };
        private ActorIndex actorIndex = new ActorIndex();

        internal OptionalsStep(int framerate, double duration)
        {
            this.framerate = framerate;
            this.duration = duration;
        }

        /// <inheritdoc cref="Scene.Camera"/>
        public OptionalsStep WithCamera(FVec2 center, double zoom = 1, double rotation = 1)
        {
            camera = new Camera(center, zoom, rotation);
            return this;
        }

        /// <summary>
        /// The fill color of the scene's background if there is no <see cref="Scene.BgTexture"/>.
        /// </summary>
        /// <param name="bgColor"></param>
        /// <returns></returns>
        public OptionalsStep WithBgColor(RGBA bgColor)
        {
            this.bgColor = bgColor;
            return this;
        }

        /// <inheritdoc cref="Scene.BgTexture"/>
        public OptionalsStep WithBgTexture(Texture bgTexture)
        {
            this.bgTexture = bgTexture;
            return this;
        }

        /// <inheritdoc cref="Scene.BgShader"/>
        public OptionalsStep WithBgShader(FragShader bgShader)
        {
            this.bgShader = bgShader;
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
        /// <returns></returns>
        public OptionalsStep WithActor(ActorBuilder actorBuilder)
        {
            return this;
        }

        /// <summary>
        /// Create a <see cref="LineBuilder"/> and add properties to the line here.
        /// </summary>
        /// <param name="lineBuilder">Builder to modify and pass.</param>
        /// <returns></returns>
        public OptionalsStep WithActor(LineBuilder lineBuilder)
        {
            return this;
        }

        /// <summary>
        /// Fully construct the scene with options provided to the builder.
        /// </summary>
        /// <returns>A new <see cref="Scene"/> object.</returns>
        public Scene Build()
        {
            return new Scene(framerate, duration, camera, bgColor, bgTexture, bgShader, think, actorIndex);
        }
    }

    /// <summary>
    /// Builder for the <see cref="Scene"/> class. Use this to create a scene.
    /// </summary>
    public class SceneBuilder
    {
        /// <summary>
        /// Pick this if you want a dynamic scene (capable of video rendering).
        /// </summary>
        /// <returns></returns>
        public DynamicStep AsDynamic()
        {
            return new DynamicStep();
        }

        /// <summary>
        /// Pick this if you want a static scene (only renders an image).
        /// </summary>
        /// <returns></returns>
        public OptionalsStep AsStatic()
        {
            return new OptionalsStep(0, 0);
        }
    }
}
