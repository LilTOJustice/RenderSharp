using RendererCommon;
using RenderSharp.RendererCommon;
using System.Runtime.Versioning;

namespace RenderSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Texture test = new Texture("C:\\Users\\Owner\\Pictures\\Camera Roll\\lock.png");
            Frame f = new Frame(test.Width, test.Height);
            Movie m = new Movie(test.Width, test.Height, 5, 15);

            for (int i = 0; i < m.NumFrames; i++) 
            {
                m.WriteFrame(f, i);
            }
            m.Output("wook");
        }
    }
}