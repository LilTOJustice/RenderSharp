using MathSharp;

namespace RenderSharp.Render2d
{
    /// <summary>
    /// Builder for the <see cref="Line"/> class.
    /// Used for <see cref="OptionalsStep.WithActor(LineBuilder, string, int)"/> within <see cref="SceneBuilder"/>.
    /// </summary>
    public class LineBuilder
    {
        private double thickness;
        private FVec2? start;
        private FVec2? end;
        private RGBA? color;
        private FragShader? shader;

        /// <inheritdoc cref="Line.Thickness"/>
        public LineBuilder WithThickness(double thickness)
        {
            this.thickness = thickness;
            return this;
        }

        /// <inheritdoc cref="Line.Start"/>
        public LineBuilder WithStart(FVec2 start)
        {
            this.start = new FVec2(start);
            return this;
        }

        /// <inheritdoc cref="Line.End"/>
        public LineBuilder WithEnd(FVec2 end)
        {
            this.end = new FVec2(end);
            return this;
        }

        /// <summary>
        /// Color of the line.
        /// </summary>
        /// <param name="color"></param>
        public LineBuilder WithColor(RGBA color)
        {
            this.color = new RGBA(color);
            return this;
        }
        
        /// <inheritdoc cref="Actor.FragShader"/>
        public LineBuilder WithShader(FragShader shader)
        {
            this.shader += shader;
            return this;
        }

        internal Line Build()
        {
            start ??= new FVec2();
            end ??= new FVec2();
            color ??= new RGBA();
            shader ??= ((in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
            return new Line(thickness, start, end, color, shader);
        }
    }
}
