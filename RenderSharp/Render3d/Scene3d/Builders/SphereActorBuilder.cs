using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Builder for the <see cref="SphereActor"/> class.
    /// Used for <see cref="SceneBuilder.FinalStep.WithActor(string, ActorBuilder)"/> within <see cref="SceneBuilder"/>.
    /// </summary>
    public class SphereActorBuilder : ActorBuilder
    {
        internal override Actor Build()
        {
            color ??= new RGBA(255, 255, 255, 255);
            size ??= new FVec3(1, 1, 1);
            rotation ??= new RVec3();
            texture ??= new Texture(1, 1, color);
            position ??= new FVec3();
            fragShader ??= ((FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
            return new SphereActor((FVec3)position, (FVec3)size, (RVec3)rotation, texture, fragShader);
        }
    }
}
