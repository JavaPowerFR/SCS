using static ScsDecoder.Utils;

namespace ScsDecoder
{
    internal class PacketDispatcher
    {
        public static void reciveA8A3_SIZE7(byte[] buffer, bool goodpacket)
        {
            ConsoleWrite(ConsoleColor.Yellow, "DATA ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[0].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[1].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[2].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[3].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"GoodParity={goodpacket}\n");

            Console.ForegroundColor = ConsoleColor.Gray;

            DataDecoder.reciveCommand(buffer, -1, goodpacket);
        }

        public static void reciveA8D0A3_SIZE11(byte[] buffer, bool goodpacket)
        {
            // [0]          [1]         [2]         [3]         [4]         [5]         [6]         [7]
            //              DEVICE_TYPE

            ConsoleWrite(ConsoleColor.Cyan, "CONFIG ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[0].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[1].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[2].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[3].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[4].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[5].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[6].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[7].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"GoodParity={goodpacket}\n");

            Console.ForegroundColor = ConsoleColor.Gray;

            DataDecoder.reciveConfig(buffer, goodpacket);
        }
        public static void reciveA8ECA3_SIZE11(byte[] buffer, bool goodpacket)
        {
            ConsoleWrite(ConsoleColor.Yellow, "EXT DATA ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[0].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[1].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[2].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[3].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[4].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[5].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[6].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"0x{buffer[7].ToString("X2")} ");
            ConsoleWrite(ConsoleColor.Green, $"GoodParity={goodpacket}\n");

            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine($"InterfaceID={buffer[1]}");

            byte[] subBuffer = new byte[4];
            Array.Copy(buffer, 4, subBuffer, 0, 4);

            DataDecoder.reciveCommand(subBuffer, buffer[1], goodpacket);
        }
    }
}
