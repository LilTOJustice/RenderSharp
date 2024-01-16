using RenderSharp.Math;

namespace RenderSharp.Scene
{
    public class Scene2dInstance
    {
        public Scene2d.Camera Camera { get; }

        public Dictionary<string, Scene2d.Actor> Actors { get; }

        public double Time { get; }

        public int Index { get; }

        public Scene2d.Scene2dThinkFunc ThinkFunc { get; private set; }

        public Scene2dInstance(Scene2d scene, double time, int index)
        {
            Camera = new Scene2d.Camera(new Vec2(scene.SceneCamera.Center), scene.SceneCamera.Zoom, scene.SceneCamera.Rotation);
            Actors = new Dictionary<string, Scene2d.Actor>(scene.Actors.Select(keyValue =>
            new KeyValuePair<string, Scene2d.Actor>(keyValue.Key,
                new Scene2d.Actor(
                    keyValue.Value.Texture,
                    new FVec2(keyValue.Value.Position),
                    new FVec2(keyValue.Value.Size),
                    keyValue.Value.Rotation,
                    keyValue.Value.Shader
                )
            )));
            Time = time;
            Index = index;
            ThinkFunc = scene.ThinkFunc;
        }

        public Scene2dInstance(Scene2dInstance scene, double time, int index)
        {
            Camera = new Scene2d.Camera(new Vec2(scene.Camera.Center), scene.Camera.Zoom, scene.Camera.Rotation);
            Actors = new Dictionary<string, Scene2d.Actor>(scene.Actors.Select(keyValue =>
            new KeyValuePair<string, Scene2d.Actor>(keyValue.Key,
                new Scene2d.Actor(
                    keyValue.Value.Texture,
                    new FVec2(keyValue.Value.Position),
                    new FVec2(keyValue.Value.Size),
                    keyValue.Value.Rotation,
                    keyValue.Value.Shader
                )
            )));
            Time = time;
            Index = index;
            ThinkFunc = scene.ThinkFunc;
        }
    }
}
