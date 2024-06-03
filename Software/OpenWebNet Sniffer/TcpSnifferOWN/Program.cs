using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpSnifferOWN
{
    public class Program
    {
        public static object MUTEX_CONSOLE = new object();
        public static TcpListener tcpServer;

        public static Thread consoleThread = new Thread(new ThreadStart(ConsoleThread));

        private static void ConsoleThread()
        {
            while (true)
            {
                ConsoleKey key = Console.ReadKey().Key;

                if (key == ConsoleKey.Delete)
                {
                    lock(MUTEX_CONSOLE)
                        Console.Clear();
                }
                else if (key == ConsoleKey.Insert)
                {
                    lock (MUTEX_CONSOLE)
                    {
                        ConsoleColor color = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("=".PadLeft(Console.WindowWidth, '='));
                        Console.ForegroundColor = color;
                    }
                }
            }
        }

        public static void Main(string[] args)
        {
            Console.Title = "Open Web Net Sniffer";
            /*string s = "*abc##*def##*ghi##";
            int startIndex = 0;
            while (true)
            {
                int endIndex = s.IndexOf("##", startIndex);
                if (endIndex == -1)
                    break;
                string substing = s.Substring(startIndex, endIndex+2 - startIndex);
                startIndex = endIndex + 2;
                Console.WriteLine(substing);

                if (endIndex > s.Length)
                    break;
            }
            return;*/
            /*DBManager.getInstance();

            DBManager.getInstance().showDescriptionForThisOPENCommand("*#1001*11*35#249#2*0##", Side.SERVER_TO_CLIENT);
            return;*/

            tcpServer = new TcpListener(IPAddress.Any, 20000);

            tcpServer.Start();
            Console.WriteLine("START SERVER");
            int i = 0;

            consoleThread.Start();

            while (true)
            {
                TcpClient serverClient = tcpServer.AcceptTcpClient();
                new ClientSession(serverClient, i++);

                Console.WriteLine("ACCEPT CLIENT");
            }
        }

        private static string GetSideName(Side s)
        {
            if (s == Side.CLIENT_TO_SERVER)
                return "C -> S";
            return "S -> C";
        }

        public static void ReciveData(int id, Side side, byte[] buffer, int size)
        {
            Console.Write($"#{id} [{GetSideName(side)}] BufferSize={size} Text=\"");
            for (int i = 0; i < size; i++)
                Console.Write((char)buffer[i]);
            Console.WriteLine("\"");

            string message = new string(Encoding.UTF8.GetChars(buffer, 0, size));
            if (message == null || message.Length < 3)
                return;

            int startIndex = 0;
            while (true)
            {
                int endIndex = message.IndexOf("##", startIndex);
                if (endIndex == -1)
                    break;
                string subMessage = message.Substring(startIndex, endIndex + 2 - startIndex);
                startIndex = endIndex + 2;

                if(subMessage.StartsWith('*') && subMessage.EndsWith("##"))
                {
                    Console.ForegroundColor = side == Side.CLIENT_TO_SERVER ? ConsoleColor.Green : ConsoleColor.Cyan;
                    if (subMessage == "*#*1##")
                    {
                        Console.WriteLine("ACK");
                    }
                    else if (subMessage == "*#*0##")
                    {
                        Console.WriteLine("NACK");
                    }
                    else
                    {
                        DBManager.getInstance().showDescriptionForThisOPENCommand(subMessage, side);
                    }

                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                if (endIndex > message.Length)
                    break;
            }
        }
    }
}
