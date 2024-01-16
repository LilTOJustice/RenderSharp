using RenderSharp.Math;

namespace RenderSharp.Scene
{
    public class Scene2dInstance
    {
        public Scene2d.Camera Camera { get; }

        public HashSet<Scene2d.Actor> Actors { get; }

        public double Time { get; }

        public int Index { get; }

        public Scene2dInstance(Scene2d scene, double time, int index)
        {
            Camera = scene.SceneCamera;
            Actors = scene.Actors.Select(actor =>
                new Scene2d.Actor(
                    actor.Texture,
                    new Vec2(actor.Position.Components),
                    new Vec2(actor.Size.Components),
                    actor.Rotation,
                    actor.Shader
                )
            ).ToHashSet();
            Time = time;
            Index = index;
        }
    }
}
