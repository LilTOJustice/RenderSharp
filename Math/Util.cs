namespace RenderSharp.Math
{
    public class Util
    {
        public static int Mod(int x, int y)
        {
            return (x % y + y) % y;
        }
    }
}
