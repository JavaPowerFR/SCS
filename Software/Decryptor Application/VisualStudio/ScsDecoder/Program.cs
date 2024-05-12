using System.IO.Ports;

namespace ScsDecoder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SerialInterface.Run();
            while (true)
            {
                ConsoleKey key = Console.ReadKey().Key;

                if (key == ConsoleKey.Delete)
                {
                    Console.Clear();
                }
                else if(key == ConsoleKey.Insert)
                {
                    ConsoleColor color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("=".PadLeft(Console.WindowWidth, '='));
                    Console.ForegroundColor = color;
                }
            }
        }
    }
}