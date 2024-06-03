namespace ScsDecoder
{
    public class DataDecoder
    {
        public static List<PacketSCS> packets_config_scs = new List<PacketSCS>();
        public static List<PacketTransformSCS> packets_config_tansforms_scs = new List<PacketTransformSCS>();

        public static void Init()
        {
            packets_config_tansforms_scs.Add(new("0xD2:*:[INTERFACE]:0x44:*:*:*:*", "*:*:*:0x34:*:*:*:*",                     "cmd over interface"));
            packets_config_tansforms_scs.Add(new("0xD0:*:[INTERFACE]:0x44:*:*:*:*", "*:*:*:0x04:*:*:*:*",                     "cmd over interface"));
            //-*-*-*-*-*-*

            packets_config_scs.Add(new("0xD0:0x00:[TYPE]:0xF6:[DEVICE_ID_3]:[DEVICE_ID_2]:[DEVICE_ID_1]:[DEVICE_ID_0]",       "programmer start configuration session with ID"));
            packets_config_scs.Add(new("0xD2:0x00:[TYPE]:0x32:0x8C:0x00:0x00:0x00",                                           "programmer start configuration session"));
            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x32:0x8C:0x00:0x00:0x00",                                        "programmer start configuration session with address"));


            packets_config_scs.Add(new("0xD1:[WHERE]:[INTERFACE_TYPE]:0x42:0x8C:0x00:0x01:0x00",                              "programmer start diagnostic session with address"));//Address = WHERE

            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x80:[N_CONF]:[BRAND_LINE]:[OBJECT_MODEL]",                  "device answer its object model and number of physical configurator"));//-
            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x81:[VERSION]:[V_RELEASE]:[V_BUILD]",                       "device answer its firmware version"));
            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x82:[VERSION]:[V_RELEASE]:[V_BUILD]",                       "device answer its hardware version"));
            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x83:[C1_C2]:[C3_C4]:[C5_C6]",                               "device answer its configurator from 1 to 6"));
            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x84:[C7_C8]:[C9_C10]:[C11_C12]",                            "device answer its configurator from 7 to 12"));
            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x85:[VERSION]:[V_RELEASE]:[V_BUILD]",                       "device answer its micro version"));
            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x86:[BITMASK_DIA_A_3]:[BITMASK_DIA_A_2]:[BITMASK_DIA_A_1]", "device answer its diagnostic bitmask A"));
            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x87:[BITMASK_DIA_B_3]:[BITMASK_DIA_B_2]:[BITMASK_DIA_B_1]", "device answer its diagnostic bitmask B"));
            packets_config_scs.Add(new("0xD0:[WHERE]:[TYPE]:0x04:[DEVICE_ID_3]:[DEVICE_ID_2]:[DEVICE_ID_1]:[DEVICE_ID_0]",    "device answer its ID"));
            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x8B:[KEYO_2]:[KEYO_1]:[SLOT]",                              "device answer its key_object, value and the state"));
            packets_config_scs.Add(new("0xD2:0x00:[TYPE]:0x32:0x8B:0x00:0x00:0x80",                                           "programmer reset all KO of device"));
            packets_config_scs.Add(new("0xD2:0x00:[TYPE]:0x32:0x8B:0x00:0x00:[SLOT]>0x80",                                    "programmer reset a specific KO of device"));
            packets_config_scs.Add(new("0xD2:0x00:[TYPE]:0x32:0x8B:[KEYO_2]:[KEYO_1]:[SLOT]",                                 "programmer send the configuration of key_object, value"));
            packets_config_scs.Add(new("0xD2:0x00:[TYPE]:0x32:0x8D:[VAL_PAR]:[INDEX]:[SLOT]",                                 "programmer send the configuration of the parameters of ko"));
            packets_config_scs.Add(new("0xD2:0x00:[TYPE]:0x32:0x83:[C1_C2]:[C3_C4]:[C5_C6]",                                  "programmer send the configuration of the configurators from 1 to 6"));
            packets_config_scs.Add(new("0xD2:0x00:[TYPE]:0x32:0x84:[C7_C8]:[C9_C10]:[C11_C12]",                               "programmer send the configuration of the configurators from 7 to 12"));

            packets_config_scs.Add(new("0xD2:0x00:[TYPE]:0x32:0x8C:0x00:0x00:0xFF",                                           "programmer send end of configuration"));
            packets_config_scs.Add(new("0xD2:0x00:[TYPE]:0x32:0x8C:0x00:0x01:0xFF",                                           "programmer abort diagnostic"));
            packets_config_scs.Add(new("0xD2:0x00:[TYPE]:0x32:0x8C:0x00:[STATUS]:0xFA",                                       "programmer abort configuration"));
            packets_config_scs.Add(new("0xD2:0x00:[TYPE]:0x34:0x8C:0x00:[STATUS]:0xFA",                                       "device abort configuration"));
            packets_config_scs.Add(new("0xD2:0x00:[TYPE]:0x32:0x8C:0x00:0x01:0xF0",                                           "programmer delete the configuration stored in the device memory"));
            
            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x8C:0x00:0x00:0xAB",                                        "device answer configuration ok"));
            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x8B:0xFF:0x00:[SLOT_STATE]",                                "device answer KO not implemented"));
            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x8B:0xFF:0x01:[SLOT_STATE]",                                "device answer KO busy"));
            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x8B:0xFF:0x02:[SLOT_STATE]",                                "device answer KO already configured"));
            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x8B:0xFF:0x03:[SLOT_STATE]",                                "device answer there are not enough free KO"));
            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x8B:0xFF:0x04:[SLOT_STATE]",                                "device answer not implemented this KO"));

            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x8C:0x00:0x00:0xAA",                                        "device answer end of transmission"));
            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x8C:0x00:0x00:0xEE",                                        "device answer for wrong configuration"));

            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x8C:0x00:0x00:0xAC",                                        "device answer end of KO transmission"));// Suposition !
            packets_config_scs.Add(new("0xD2:0x00:[TYPE]:0x32:0x8C:0x00:0x00:0xAA",                                           "programmer answer end of transmission"));
            packets_config_scs.Add(new("0xD0:0x00:[TYPE]:0xFB:0x00:0x00:0x00:0x00",                                           "programmer delete from memory the previous scans"));
            packets_config_scs.Add(new("0xD0:0x00:[TYPE]:0xF3:0x00:0x00:0x00:0x00",                                           "programmer start scan for all devices with ID"));
            packets_config_scs.Add(new("0xD0:0x00:[TYPE]:0xF9:0x00:0x00:0x00:0x00",                                           "programmer start scan for only devices configured with ID"));
            packets_config_scs.Add(new("0xD0:0x00:[TYPE]:0xF8:0x00:0x00:0x00:0x00",                                           "programmer start scan for only devices not configured with ID"));


            packets_config_scs.Add(new("0xD0:0x00:[TYPE]:0xFA:[DEVICE_ID_3]:[DEVICE_ID_2]:[DEVICE_ID_1]:[DEVICE_ID_0]",       "programmer send flag to all the devices found"));

            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x8D:[VAL_PAR]:[INDEX]:[SLOT]",                              "device answer the parameters of ko"));
            packets_config_scs.Add(new("0xD2:[WHERE]:[TYPE]:0x34:0x8D:[SYS]:[ADDR]:[SLOT]>0x7F",                              "device answer its key_object, system and address"));//slot - 0x80 SYS HIGH
            packets_config_scs.Add(new("0xD2:0x00:[TYPE]:0x42:0x8D:[SYS]:[ADDR]:[SLOT]>0x7F",                                 "programmer send the configuration of key_object, system and address"));//slot - 0x80 SYS HIGH

            packets_config_scs.Add(new("0xD2:0x00:[TYPE]:0x33:0x8D:0x06:0x00:0x00",                                           "reset keyo on all slot"));
            packets_config_scs.Add(new("0xD0:0x00:[TYPE]:0xF7:[DEVICE_ID_3]:[DEVICE_ID_2]:[DEVICE_ID_1]:[DEVICE_ID_0]",       "programmer start diagnostic session with ID"));
            packets_config_scs.Add(new("0xD5:[WHERE_IID_A]:[TYPE]:0x43:0x80:0x00:0x00:0x00",                                  "programmer start scan for room"));// WHERE H=InterfaceID L=AERA TYPE=0x10+TYPE
            
            packets_config_scs.Add(new("0xD2:0x00:[TYPE]:0x32:0x8C:0x00:0x01:0x00",                                           "programmer start diagnostic session"));
            
            /*
            //Thermoregulation // NOT IMPLEMENTED NOW !
            packets_config_scs.Add(new("0xD5:[ZA_ZB]:0x03:0x33:0x80:0x00:0x00:0x00",                                          "programmer start scan of termoregulation"));
            packets_config_scs.Add(new("0xD1:[ZA_ZB]:0x03:0x32:0x8C:0x00:0x01:0x00",                                          "programmer start diagnostic session with address [Sonde (maître)]"));//ok
            packets_config_scs.Add(new("0xD1:[ZA_ZB]:[SLA]L=0x03:0x32:0x8C:0x00:0x01:0x00",                                   "programmer start diagnostic session with address [Sonde (esclave)]"));//ok
            packets_config_scs.Add(new("0xD1:0x00:[PL_N]L=0x03:0x32:0x8C:0x00:0x01:0x00",                                     "programmer start diagnostic session with address [Sonde (externe)]"));//ok
            //packets_config_scs.Add(new("0xD1:[ZA_ZB]>0x7F:0x03:0x32:0x8C:0x00:0x01:0x00",                                     "programmer start diagnostic session with address [Unité de controle 4 zones]")); //??
            packets_config_scs.Add(new("0xD1:[ZA_ZB]>0x7F:[N]L=0x03:0x32:0x8C:0x00:0x01:0x00",                                "programmer start diagnostic session with address [Geteway ou Action]"));
            */

            packets_config_scs.Sort((a, b) =>
            {
                if (a.GetWeight() > b.GetWeight())
                    return -1;
                if (a.GetWeight() < b.GetWeight())
                    return 1;
                return 0;
            });

            packets_config_tansforms_scs.Sort((a, b) =>
            {
                if (a.GetWeight() > b.GetWeight())
                    return -1;
                if (a.GetWeight() < b.GetWeight())
                    return 1;
                return 0;
            });

            /*foreach (var packet in packets_config_scs)
                Console.WriteLine(packet.GetPatternString()+"  "+packet.GetWeight()+"  "+packet.GetDescription());*/
        }

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
            if(goodpacket)
            {
                byte[] bufferOut = buffer;
                foreach (PacketTransformSCS transform in packets_config_tansforms_scs)
                {
                    if (transform.ShowAndApplyPathIfIsCompatible(buffer, out bufferOut))
                        break;
                }

                foreach(PacketSCS packet in packets_config_scs)
                {
                    if (packet.ShowIfIsCompatible(bufferOut))
                        return;
                }

                Utils.ConsoleWrite(ConsoleColor.Red, "UNIMPLEMENTED !\n");
            }
        }
    }
}
