using RenderSharp.Math;

namespace RenderSharp.Scene
{
    public class Scene2dInstance
    {
        public Scene2d.Camera Camera { get; }

        public HashSet<Scene2d.Actor> Actors { get; }

        public double Time { get; }

        public int Index { get; }

        public Scene2dThinkFunc ThinkFunc { get; private set; }

        public Scene2dInstance(Scene2d scene, double time, int index)
        {
            Camera = new Scene2d.Camera(new Vec2(scene.SceneCamera.Center.Components), scene.SceneCamera.Zoom, scene.SceneCamera.Rotation);
            Actors = scene.Actors.Select(actor =>
                new Scene2d.Actor(
                    actor.Texture,
                    new FVec2(actor.Position.Components),
                    new FVec2(actor.Size.Components),
                    actor.Rotation,
                    actor.Shader
                )
            ).ToHashSet();
            Time = time;
            Index = index;
            ThinkFunc = scene.ThinkFunc;
        }

        public Scene2dInstance(Scene2dInstance scene, double time, int index)
        {
            Camera = new Scene2d.Camera(new Vec2(scene.Camera.Center.Components), scene.Camera.Zoom, scene.Camera.Rotation);
            Actors = scene.Actors.Select(actor =>
                new Scene2d.Actor(
                    actor.Texture,
                    new FVec2(actor.Position.Components),
                    new FVec2(actor.Size.Components),
                    actor.Rotation,
                    actor.Shader
                )
            ).ToHashSet();
            Time = time;
            Index = index;
            ThinkFunc = scene.ThinkFunc;
        }
    }
}
