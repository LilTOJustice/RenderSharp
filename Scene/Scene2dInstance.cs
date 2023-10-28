using RenderSharp.Math;
using RenderSharp.RendererCommon;

namespace RenderSharp.Scene
{
    internal class Scene2dInstance
    {
        public Scene2d.Camera Camera { get; set; } 
        public HashSet<Scene2d.Actor> Actors { get; set; }
        public RGB BgColor { get; set; } 
        public Texture BgTexture { get; set; }
        public FragShader Shader { get; set; }

        public Scene2dInstance(Scene2d scene)
        {
            Camera = scene.SceneCamera;
            Actors = scene.Actors;
            BgColor = scene.BgColor;
            BgTexture = scene.BgTexture;
            Shader = scene.Shader;
        }

        public void ClearShaders()
        {
            Shader = (FragShaderArgs) => { };
        }

        public Vec2 ScreenToWorld(Vec2 screenSize, Vec2 screenCoords)
        {
            return (Vec2) (new Vec2 (screenCoords.X - (screenSize.X / 2),
                     screenCoords.Y - (screenSize.Y / 2)) / Camera.Zoom + Camera.Center).Rotate(Camera.Rotation);
        }

        public static Vec2 WorldToActor(Scene2d.Actor actor, Vec2 worldCoord)
        {
            return (Vec2) ((FVec2) (worldCoord - actor.Position)).Rotate(-actor.Rotation);
        }

        public Vec2 ScreenToActor(Vec2 screenSize, Scene2d.Actor actor, Vec2 screenCoord)
        {
            return WorldToActor(actor, ScreenToWorld(screenSize, screenCoord));
        }
    }
}
