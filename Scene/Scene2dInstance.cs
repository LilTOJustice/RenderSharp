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
                    actor.Rotation
                )
            ).ToHashSet();
            Time = time;
            Index = index;
        }

        public Vec2 ScreenToWorld(Vec2 screenSize, Vec2 screenCoords)
        {
            return (Vec2)(new Vec2(screenCoords.X - (screenSize.X / 2),
                     screenCoords.Y - (screenSize.Y / 2)) / Camera.Zoom + Camera.Center).Rotate(Camera.Rotation);
        }

        public static Vec2 WorldToActor(Scene2d.Actor actor, Vec2 worldCoord)
        {
            return (Vec2)((FVec2)(worldCoord - actor.Position)).Rotate(-actor.Rotation);
        }

        public Vec2 ScreenToActor(Vec2 screenSize, Scene2d.Actor actor, Vec2 screenCoord)
        {
            return WorldToActor(actor, ScreenToWorld(screenSize, screenCoord));
        }
    }
}
