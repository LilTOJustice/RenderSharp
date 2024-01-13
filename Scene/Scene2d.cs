using RenderSharp.RendererCommon;
using RenderSharp.Math;

namespace RenderSharp.Scene
{
    public class Scene2d
    {
        public class Camera
        {
            public Vec2 Center { get; set; }

            public double Zoom { get; set; }

            public double Rotation { get; set; }

            public Camera(Vec2 center, double zoom, double rotation)
            {
                Center = center;
                Zoom = zoom;
                Rotation = rotation;
            }
            public void Translate(Vec2 change)
            {
                Center += change;
            }
            public void ScaleZoom(double zoom)
            {
                Zoom += zoom;
            }
            public void Rotate(double radChange) 
            {
                Rotation = radChange;
            }
        }

        public class Actor
        {
            public int Width { get { return Size.X; } set { Size.X = value; } }
            public int Height { get { return Size.Y; } set { Size.Y = value; } }
            public Vec2 Position { get; set; }
            public Vec2 Size { get; set; }
            public double Rotation { get; set; }
            public Texture Texture { get; set; }
            public FragShader Shader { get; set; }
            public Scene2dThinkFunc ThinkFunc { get; set; }

            public Actor(Texture texture, Vec2? position = null, Vec2? size = null, double rotation = 0)
            {
                Texture = texture;
                Position = position ?? new Vec2();
                Size = size ?? new Vec2();
                Rotation = rotation;
                Shader = (FragShaderArgs) => { };
                ThinkFunc = (Scene2dThinkFuncArgs) => { };
            }

            public void Scale(FVec2 scale)
            {
                Size = (Vec2) ((FVec2) Size * scale);
            }

            public void Scale(double scale)
            {
                Size = (Vec2) ((FVec2) Size * scale);
            }

            public void ClearShaders()
            {
                Shader = (FragShaderArgs) => { }; 
            }

            public void ClearThinkFunc()
            {
                ThinkFunc = (Scene2dThinkFuncArgs) => { };
            }
        }

        public int Framerate { get { return (int)(1 / DeltaTime); } private set { DeltaTime = 1d / value; } }

        public Camera SceneCamera { get; set; }

        public List<double> TimeSeq { get; private set; }
        
        public double Duration { get; private set; }
        
        public double DeltaTime { get; private set; } 
        
        public HashSet<Actor> Actors { get; set; }
        
        public RGB BgColor { get; set; }
        
        public Texture BgTexture { get; set; }
        
        public FragShader Shader { get; set; }
        
        public Scene2dThinkFunc ThinkFunc { get; set; } //Everthing I do with Shader I've gotta do with this

        public Scene2d(int framerate, double duration, RGB? bgcolor = null, Texture? texture = null) 
        {
            Framerate = framerate;
            BgColor = bgcolor ?? new RGB();
            BgTexture = texture ?? new Texture(new Vec2(1, 1));
            Duration = duration;
            TimeSeq = new List<double>();
            for (int i = 0; i < framerate * duration; i++)
            {
                TimeSeq.Add(i*DeltaTime);
            }
            Actors = new HashSet<Actor>();
            SceneCamera = new Camera(new Vec2(0, 0), 1, 0);
            Shader = (FragShaderArgs) => { };
            ThinkFunc = (Scene2dThinkFuncArgs) => { };
        }

        public Actor AddActor(Texture texture, Vec2 position, Vec2 size, double rotation)
        {
            Actor holderActor = new Actor(texture, position, size, rotation);
            Actors.Add(holderActor);
            return holderActor;
        }

        public bool RemoveActor(Actor actor) 
        {
            return Actors.Remove(actor);
        }

        public void ClearShaders()
        {
            Shader = (FragShaderArgs) => { };
        }

        public void ClearThinkFunc()
        {
            ThinkFunc = (Scene2dThinkFuncArgs) => { };
        }

        public Vec2 ScreenToWorld(Vec2 screenSize, Vec2 screenCoords)
        {
            return (Vec2)(new Vec2(screenCoords.X - (screenSize.X / 2),
                     screenCoords.Y - (screenSize.Y / 2)) / SceneCamera.Zoom + SceneCamera.Center).Rotate(SceneCamera.Rotation);
        }

        public static Vec2 WorldToActor(Actor actor, Vec2 worldCoord)
        {
            return (Vec2)((FVec2)(worldCoord - actor.Position)).Rotate(-actor.Rotation);
        }

        public Vec2 ScreenToActor(Vec2 screenSize, Actor actor, Vec2 screenCoord)
        {
            return WorldToActor(actor, ScreenToWorld(screenSize, screenCoord));
        }
    }
}