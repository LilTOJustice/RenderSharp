using RenderSharp.RendererCommon;
using RenderSharp.Math;

namespace RenderSharp.Scene
{
    public class Scene2d
    {
        public delegate void Scene2dThinkFunc(Scene2dInstance scene, double time, double dt);

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
        }

        public class Actor
        {
            public double Width { get { return Size.X; } set { Size.X = value; } }

            public double Height { get { return Size.Y; } set { Size.Y = value; } }
            
            public FVec2 Position { get; set; }
            
            public FVec2 Size { get; set; }
            
            public double Rotation { get; set; }
            
            public Texture Texture { get; set; }
            
            public FragShader Shader { get; set; }

            public Actor(Texture texture, FVec2? position = null, FVec2? size = null, double rotation = 0, FragShader? shader = null)
            {
                Texture = texture;
                Position = position ?? new FVec2();
                Size = size ?? new FVec2(Texture.Size.X, Texture.Size.Y);
                Rotation = rotation;
                Shader = shader ?? ((in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
            }

            public void ClearShaders()
            {
                Shader = (in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; }; 
            }
        }

        public int Framerate { get { return (int)(1 / DeltaTime); } private set { DeltaTime = 1d / value; } }

        public Camera SceneCamera { get; set; }

        public List<double> TimeSeq { get; private set; }
        
        public double Duration { get; private set; }
        
        public double DeltaTime { get; private set; } 
        
        public HashSet<Actor> Actors { get; set; }
        
        public RGB? BgColor { get; set; }
        
        public Texture? BgTexture { get; set; }
        
        public FragShader Shader { get; set; }
        
        public Scene2dThinkFunc ThinkFunc { get; set; }

        public Scene2d(int framerate = 0, double duration = 0, RGB? bgcolor = null, Texture? bgTexture = null, FragShader? shader = null) 
        {
            Framerate = framerate;
            BgColor = bgcolor;
            BgTexture = bgTexture;
            Duration = duration;
            TimeSeq = new List<double>();
            for (int i = 0; i < framerate * duration; i++)
            {
                TimeSeq.Add(i * DeltaTime);
            }
            Actors = new HashSet<Actor>();
            SceneCamera = new Camera(new Vec2(0, 0), 1, 0);
            Shader = shader ?? ((in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
            ThinkFunc = (Scene2dInstance scene, double time, double dt) => { };
        }

        public void AddActor(Actor actor)
        {
            Actors.Add(actor);
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
            Shader = (in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; };
        }

        public void ClearThinkFunc()
        {
            ThinkFunc = (Scene2dInstance scene, double time, double dt) => { };
        }

        public List<Scene2dInstance> Simulate(int? simulateToIndex = null)
        {
            simulateToIndex ??= TimeSeq.Count - 1;
            List<Scene2dInstance> instances = new List<Scene2dInstance>((int)simulateToIndex! + 1);
            Scene2dInstance current = new Scene2dInstance(this, TimeSeq[0], 0);
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