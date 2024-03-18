using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Builder for the <see cref="TriangleActor"/> class.
    /// Used for <see cref="SceneBuilder.FinalStep.WithActor(string, ActorBuilder)"/> within <see cref="SceneBuilder"/>.
    /// </summary>
    public class TriangleActorBuilder : ActorBuilder
    {
        internal override Actor Build()
        {
            boundingBoxSize ??= new FVec3(1, 1, 1);
            rotation ??= new RVec3();
            texture ??= new Texture(1, 1, color);
            position ??= new FVec3();
            fragShader ??= ((FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
            return new TriangleActor((FVec3)boundingBoxSize, (RVec3)rotation, (FVec3)position, texture, fragShader);
        }
    }
}
