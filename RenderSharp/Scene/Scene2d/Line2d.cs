using MathSharp;
using RenderSharp.Common;

namespace RenderSharp.Scene
{
    /// <summary>
    /// A 2d line.
    /// </summary>
    public class Line2d : Actor2d
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
                return ((Actor2d)this).Position;
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
            get { return ((Actor2d)this).Size; }
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
            get { return ((Actor2d)this).Rotation; }
            set
            {
                ((Actor2d)this).Rotation = value;
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
        public Line2d(double thickness, FVec2 start, FVec2 end, RGBA? color = null, FragShader? shader = null) : base(new FVec2())
        {
            Texture = new Texture(1, 1, color);
            ((Actor2d)this).Size = new FVec2(0, thickness);
            _start = start;
            _end = end;
            Shader = shader ?? ((in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
            Recompute();
        }

        /// <summary>
        /// Constructs a 2d line based on length, position and rotation.
        /// </summary>
        /// <param name="thickness">Thickness of the line in world space.</param>
        /// <param name="length">Length of the line in world space.</param>
        /// <param name="position">Location of the center of the line in world space.</param>
        /// <param name="rotation">Rotation (radians) of the line in world space.</param>
        /// <param name="color">Color of the line's texture.</param>
        /// <param name="shader">Shader run on the single pixel representing the line.</param>
        public Line2d(double thickness, int length, FVec2 position, double rotation = 0, RGBA? color = null, FragShader? shader = null) : base(new FVec2())
        {
            Texture = new Texture(1, 1, color);
            ((Actor2d)this).Size = new FVec2(0, thickness);
            Length = length;
            ((Actor2d)this).Rotation = rotation;
            Position = position;
            Shader = shader ?? ((in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
            _end ??= new FVec2();
            _start ??= new FVec2();
        }

        private void Recompute()
        {
            FVec2 disp = _end - _start;
            Size.X = disp.Length();
            ((Actor2d)this).Position = _start + (disp / 2);
            ((Actor2d)this).Rotation = Math.Atan2(disp.Y, disp.X);
        }

        internal override Actor2d Reconstruct()
        {
            return new Line2d(Thickness, _start, _end, Texture[0, 0], Shader);
        }
    }
}
