﻿using MathSharp;
using RenderSharp;
using RenderSharp.Render3d;

namespace RenderSharpExample
{
    internal class Program
    {
        static readonly int framerate = 60;
        static readonly int duration = 5;
        static readonly int resX = 1280;
        static readonly int resY = 720;

        static void Main()
        {
            // Create scene
            Scene scene = SceneBuilder
                .MakeDynamic()
                .WithFramerate(framerate)
                .WithDuration(duration)
                /*.WithActor("car", new ModelActorBuilder()
                    .WithModel(Model.FromFile("../../../assets/car_bebo.obj"))
                    .WithSize(new FVec3(1, 1, 1))
                    .WithPosition(new FVec3(0, -0.5, 5))
                )*/
                .WithActor("sphere", new SphereActorBuilder()
                    .WithSize(new FVec3(1, 1, 1))
                    .WithPosition(new FVec3(0, -0.5, 5))
                    .WithColor(new RGB(0, 0, 255))
                )
                /*.WithActor("cube", new CubeActorBuilder()
                    .WithTexture(new Texture("../../../assets/gordon.jpg"))
                    .WithSize(new FVec3(1, 1, 1))
                    .WithPosition(new FVec3(0, -0.5, 5))
                )*/
                .WithActor("platform", new CubeActorBuilder()
                    .WithColor(new RGB(128, 128, 128))
                    .WithSize(new FVec3(10, 1, 10))
                    .WithPosition(new FVec3(0, -2.5, 5))
                )
                .WithPointLight("light1", new FVec3(2, -0.5, 6))
                .WithPointLight("light2", new FVec3(-2, -0.5, 6))
                .WithThink((SceneInstance scene, double time, double dt) =>
                {
                    scene.GetLight("light1").Position += new FVec3(0, 0, -2) * dt;
                    scene.GetLight("light2").Position += new FVec3(0, 0, -2) * dt;
                })
                .Build();

            // Create renderer
            Renderer renderer = new(resX, resY, scene);

            // Finally render and output the video
            renderer.RenderMovie(showDepth: false).Output("test");
        }
    }
}