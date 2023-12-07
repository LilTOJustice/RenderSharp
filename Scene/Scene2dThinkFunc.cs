namespace RenderSharp.Scene
{
    public class Scene2dThinkFuncArgs
    {
        public double Time;
        public double DT;

        public Scene2dThinkFuncArgs(double time, double dt)
        {
            this.Time = time;
            this.DT = dt;
        }
    }

    public delegate void Scene2dThinkFunc(Scene2dThinkFuncArgs args);
}