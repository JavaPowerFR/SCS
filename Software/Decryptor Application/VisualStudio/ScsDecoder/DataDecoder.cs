using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ScsDecoder
{
    internal class DataDecoder
    {
        static string scsCmdToString(byte b)
        {
            switch (b)
            {
                case 0: return "ON";
                case 1: return "OFF";
                case 3: return "DIM+";
                case 4: return "DIM-";
                case 8: return "UP";
                case 9: return "DOWN";
                case 10: return "STOP";
                default:
                    if ((b & 0x0D) != 0)
                        return $"DIM{(((b & 0xF0) >> 4) + 1) * 10}%";

                    return "UNKOW";
            }
        }

        static string scsCfgDeviceTypeToString(byte b)
        {
            switch(b)
            {
                case 1: return "AUTOMATION";
                case 2: return "ENERGY";
                case 3: return "TEMP";
                default:
                    return "UNKOW";
            }
        }

        static string scsCfgEndTypeToString(byte b)
        {
            switch (b)
            {
                case 0: return "SEND_CONFIG";
                case 1: return "SCAN";

                default:
                    return "UNKOW";
            }
        }

        public static void reciveCommand(byte[] buffer, int scsInterface, bool goodParity)
        {
            if (buffer[0] <= 0xA9)
            {
                //[0]     [1]     [2]     [3]
                //A/PL    0x00    0x12    CMD

                byte scs_A = (byte)(buffer[0] >> 4);
                byte scs_PL = (byte)(buffer[0] & 0x0F);
                byte scs_cmd = buffer[3];

                Console.WriteLine($"CMD A={scs_A} PL={scs_PL} CMD={scsCmdToString(scs_cmd)}");
            }
            else if (buffer[0] == 0xB1)
            {
                //[0]     [1]     [2]     [3]
                //0xB3    0x00    0x12    CMD

                byte scs_cmd = buffer[3];
                Console.WriteLine($"GENERAL CMD={scsCmdToString(scs_cmd)}");
            }
            else if (buffer[0] == 0xB5)
            {
                //[0]     [1]     [2]     [3]
                //0xB5    PL      0x12    CMD

                byte scs_PL = (byte)(buffer[1] & 0x0F);
                byte scs_cmd = buffer[3];

                Console.WriteLine($"GR PL={scs_PL} CMD={scsCmdToString(scs_cmd)}");
            }
            else if (buffer[0] == 0xB3)
            {
                //[0]     [1]     [2]     [3]
                //0xB3    PL      0x12    CMD

                byte scs_PL = (byte)(buffer[1] & 0x0F);
                byte scs_cmd = buffer[3];

                Console.WriteLine($"AMB PL={scs_PL} CMD={scsCmdToString(scs_cmd)}");
            }
            else if (buffer[0] == 0xB8)
            {
                //[0]     [1]     [2]     [3]
                //0xB8    A/PL    0x12    CMD

                byte scs_A = (byte)(buffer[1] >> 4);
                byte scs_PL = (byte)(buffer[1] & 0x0F);
                byte scs_cmd = buffer[3];

                Console.WriteLine($"REPLY A={scs_A} PL={scs_PL} CMD={scsCmdToString(scs_cmd)}");
            }
        }

        public static void reciveConfig(byte[] buffer, bool goodpacket)
        {
            // [0]              [1]             [2]             [3]             [4]             [5]             [6]             [7]
            //                                  DEVICE_TYPE     CMD
        }
    }
}
