using MathSharp;

namespace RenderSharp.Render2d
{
    /// <summary>
    /// A 2d line.
    /// </summary>
    public class Line : Actor
    {
        private FVec2 _start;
        private FVec2 _end;

        /// <summary>
        /// The start point of the line in world space.
        /// </summary>
        public FVec2 Start
        {
            get { return _start; }
            set
            {
                _start = value;
                Recompute();
            }
        }

        /// <summary>
        /// The end point of the line in world space.
        /// </summary>
        public FVec2 End
        {
            get { return _end; }
            set
            {
                _end = value;
                Recompute();
            }
        }

        /// <summary>
        /// Position of the center of the line in world space.
        /// </summary>
        public new FVec2 Position
        {
            get
            {
                return ((Actor)this).Position;
            }
            set
            {
                FVec2 disp = value - Position;
                _start += disp;
                _end += disp;
            }
        }

        /// <summary>
        /// Length of the line in world space.
        /// </summary>
        public double Length
        {
            get { return (_end - _start).Length(); }
            set
            {
                FVec2 disp = _end - _start;
                disp = (disp / disp.Length()) * value / 2;
                _end = disp + Position;
                _start = (disp * -1) + Position;
                Recompute();
            }
        }

        /// <summary>
        /// Size of the line as represented in the world. Cannot be mutated. See <see cref="Length"/> and <see cref="Thickness"/> for changing the length and thickness.
        /// </summary>
        public new FVec2 Size
        {
            get { return ((Actor)this).Size; }
            set { }
        }

        /// <summary>
        /// The thickness of the line in world space.
        /// </summary>
        public double Thickness { get { return Size.Y; } set { Size.Y = value; } }

        /// <summary>
        /// The rotation of the line in world space.
        /// </summary>
        public new double Rotation
        {
            get { return ((Actor)this).Rotation; }
            set
            {
                ((Actor)this).Rotation = value;
                _start = new FVec2(Math.Cos(value) * Length / 2, Math.Sin(value) * Length / 2);
                _end = _start * -1;
                _start += Position;
                _end += Position;
            }
        }

        /// <summary>
        /// Constructs a 2d line based on start and end points.
        /// </summary>
        /// <param name="thickness">Thickness of the line in world space.</param>
        /// <param name="start">Location of the start of the line in world space.</param>
        /// <param name="end">Location of the end of the line in world space.</param>
        /// <param name="color">Color of the line.</param>
        /// <param name="shader">Shader run on the single pixel representing the line.</param>
        internal Line(
            double thickness,
            FVec2 start,
            FVec2 end,
            RGBA color,
            FragShader shader) : base(new FVec2(), 0, new FVec2(), new(0, 0), (in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; })
        {
            _start = start;
            _end = end;
            Texture = new Texture(1, 1, color);
            Shader = shader;
            ((Actor)this).Size = new FVec2(0, thickness);
            Recompute();
        }

        private void Recompute()
        {
            FVec2 disp = _end - _start;
            Size.X = disp.Length();
            ((Actor)this).Position = _start + (disp / 2);
            ((Actor)this).Rotation = Math.Atan2(disp.Y, disp.X);
        }

        /// <inheritdoc cref="Actor.Copy"/>
        public override Line Copy()
        {
            return new Line(Thickness, _start, _end, Texture[0, 0], Shader);
        }
    }
}
