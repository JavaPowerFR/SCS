namespace ScsDecoder
{
    public class Utils
    {
        public static void ConsoleWrite(ConsoleColor color, string msg)
        {
            ConsoleColor old = Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ForegroundColor = old;
        }
    }
}
