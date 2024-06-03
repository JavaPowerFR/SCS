using System.IO.Ports;

namespace ScsDecoder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "SCS Decoder";
            DataDecoder.Init();
            SerialInterface.Run();
            byte value = 0;
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
                else if (key == ConsoleKey.NumPad9)
                {
                    //A/PL    0x00    0x12    CMD
                    // 0xD2:[WHERE]:[TYPE]:0x34:0x87:[BITMASK_DIA_B_3]:[BITMASK_DIA_B_2]:[BITMASK_DIA_B_1]

                    //0xD2:[WHERE]:[TYPE]:0x34:0x82:[VERSION]:[V_RELEASE]:[V_BUILD]
                    //0xD2:[WHERE]:[TYPE]:0x34:0x85:[VERSION]:[V_RELEASE]:[V_BUILD]
                    //0xD2:[WHERE]:[TYPE]:0x34:0x8C:0x00:0x00:0xAC
                    //0xD2:[WHERE]:[TYPE]:0x34:0x8B:0xFF:0x04:[SLOT_STATE]

                    //SerialInterface.SendData(SerialInterface.WrapPacketA8A3(new byte[] { 0xD2, 0x00, 0x01, 0x33, 0x8D, 0x00, 0x00, 0x02}), 1);
                    SerialInterface.SendData(new byte[] {0xA5}, 1);
                }
                else if (key == ConsoleKey.Add)
                {
                    //A/PL    0x00    0x12    CMD
                    //SerialInterface.SendData(SerialInterface.WrapPacketA8A3(new byte[] { 0x11, 0x00, 0x12, 0x00 }), 1);
                    //SerialInterface.SendData(SerialInterface.WrapPacketA8A3(new byte[] { 0xD2, 0x00, 0x01, 0x34, 0x85, 0x01, 0x01, 0x01 }), 1);
                    ++value;
                    Console.WriteLine("new value=0x" + value.ToString("X2"));
                }
                else if (key == ConsoleKey.NumPad7)
                {
                    //A/PL    0x00    0x12    CMD
                    //SerialInterface.SendData(SerialInterface.WrapPacketA8A3(new byte[] { 0x11, 0x00, 0x12, 0x01 }), 1);
                    //SerialInterface.SendData(new byte[] { 0xA6 }, 1);
                }
            }
        }
    }
}