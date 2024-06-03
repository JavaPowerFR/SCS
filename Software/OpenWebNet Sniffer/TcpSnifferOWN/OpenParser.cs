namespace TcpSnifferOWN
{
    public class OpenParser
    {
        public enum WHO
        {
            UNDEFINED = -1, // 0xFFFFFFFF
            SCENARIOS = 0,
            LIGHTING = 1,
            AUTOMATION_SYSTEMS = 2,
            APPLIANCES = 3,
            HEATING_ADJUSTMENT = 4,
            ALARMS = 5,
            VIDEO_DOOR_ENTRY_SYSTEM = 6,
            MULTIMEDIA = 7,
            VIDEO_DOOR_ENTRY_SYSTEM_OVER_IP = 8,
            AUXILIARIES = 9,
            NAVIGATION = 10, // 0x0000000A
            ENERGY_DISTRIBUTION = 11, // 0x0000000B
            EXTERNAL_INTERFACE_DEVICES = 13, // 0x0000000D
            SPECIAL_COMMANDS = 14, // 0x0000000E
            HOME_AUTOMATION_MAIN_UNIT_COMMAND = 15, // 0x0000000F
            SOUND_SYSTEM = 16, // 0x00000010
            HOME_AUTOMATION_MAIN_UNIT_MANAGEMENT = 17, // 0x00000011
            SERVICE_IDENTIFICATION = 99, // 0x00000063
            DIAGNOSTIC_OF_LIGHTING_SYSTEM = 1001, // 0x000003E9
            DIAGNOSTIC_OF_HEATING_SYSTEM = 1004, // 0x000003EC
            DIAGNOSTIC_OF_DEVICE_SYSTEM = 1013, // 0x000003F5
        }
    }
}
