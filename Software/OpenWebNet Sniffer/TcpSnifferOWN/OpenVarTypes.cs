namespace TcpSnifferOWN
{
    public class OpenVarTypes
    {
        private static OpenVarTypes? instance;

        public static OpenVarTypes GetInstance()
        {
            if(instance == null)
                instance = new OpenVarTypes();

            return instance;
        }
        public readonly OpenVarType 
            VARTYPE_WHO,
            VARTYPE_WHERE,
            VARTYPE_OBJECT_MODEL,
            VARTYPE_N_CONF,
            VARTYPE_BRAND,
            VARTYPE_LINE,
            VARTYPE_ID,
            VARTYPE_FW_VERSION,
            VARTYPE_C1,
            VARTYPE_C2,
            VARTYPE_C3,
            VARTYPE_C4,
            VARTYPE_C5,
            VARTYPE_C6,
            VARTYPE_C7,
            VARTYPE_C8,
            VARTYPE_C9,
            VARTYPE_C10,
            VARTYPE_C11,
            VARTYPE_C12,
            VARTYPE_MICRO_VERSION,
            VARTYPE_Version,
            VARTYPE_Release,
            VARTYPE_Build,
            VARTYPE_BITMASK_A,
            VARTYPE_BITMASK_B,
            VARTYPE_SLOT,
            VARTYPE_KEYO,
            VARTYPE_STATE,
            VARTYPE_ERROR,
            VARTYPE_SYS,
            VARTYPE_ADDR,
            VARTYPE_VAL_PAR,
            VARTYPE_INDEX,
            VARTYPE_LIVELLOLUX,
            VARTYPE_MAC,
            VARTYPE_HW_VERSION,
            VARTYPE_BIT,
            VARTYPE_OPERATIONS,
            VARTYPE_RESULT,
            VARTYPE_SCE,
            VARTYPE_STATE_SCE,
            VARTYPE_MAC1,
            VARTYPE_MAC2,
            VARTYPE_MAC3,
            VARTYPE_MAC4,
            VARTYPE_MAC5,
            VARTYPE_MAC6,
            VARTYPE_SCAN_TYPE;

        public List<OpenVarType> LIST_VARTYPE;
        public OpenVarTypes()
        {
            LIST_VARTYPE = new List<OpenVarType>();

            VARTYPE_WHO =           new("WHO",              OpenVarInputFormat.RANGE,       "who",                                                              (input) => (OpenParser.WHO)Convert.ToUInt32(input));
            VARTYPE_WHERE =         new("WHERE",            OpenVarInputFormat.RANGE,       "where",                                                            (input) => Convert.ToUInt32(input));
            VARTYPE_OBJECT_MODEL =  new("OBJECT_MODEL",     OpenVarInputFormat.RANGE,       "device object model",                                              (input) => Convert.ToUInt32(input));
            VARTYPE_N_CONF =        new("N_CONF",           OpenVarInputFormat.RANGE,       "Configurator number",                                              (input) => Convert.ToUInt32(input));
            VARTYPE_BRAND =         new("BRAND",            OpenVarInputFormat.RANGE,       "device brand",                                                     (input) => Convert.ToUInt32(input));
            VARTYPE_LINE =          new("LINE",             OpenVarInputFormat.RANGE,       "device aestethic",                                                 (input) => Convert.ToUInt32(input));
            VARTYPE_ID =            new("ID",               OpenVarInputFormat.RANGE,       "device id",                                                        (input) => Convert.ToUInt32(input));
            VARTYPE_FW_VERSION =    new("FW_VERSION",       OpenVarInputFormat.UNDEFINED,   "device firmware version",                                          (input) => input, "[Version]*[Release]*[Build]");
            VARTYPE_C1 =            new("C1",               OpenVarInputFormat.RANGE,       "Configurator value",                                               (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_C2 =            new("C2",               OpenVarInputFormat.RANGE,       "Configurator value",                                               (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_C3 =            new("C3",               OpenVarInputFormat.RANGE,       "Configurator value",                                               (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_C4 =            new("C4",               OpenVarInputFormat.RANGE,       "Configurator value",                                               (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_C5 =            new("C5",               OpenVarInputFormat.RANGE,       "Configurator value",                                               (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_C6 =            new("C6",               OpenVarInputFormat.RANGE,       "Configurator value",                                               (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_C7 =            new("C7",               OpenVarInputFormat.RANGE,       "Configurator value",                                               (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_C8 =            new("C8",               OpenVarInputFormat.RANGE,       "Configurator value",                                               (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_C9 =            new("C9",               OpenVarInputFormat.RANGE,       "Configurator value",                                               (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_C10 =           new("C10",              OpenVarInputFormat.RANGE,       "Configurator value",                                               (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_C11 =           new("C11",              OpenVarInputFormat.RANGE,       "Configurator value",                                               (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_C12 =           new("C12",              OpenVarInputFormat.RANGE,       "Configurator value",                                               (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_MICRO_VERSION = new("MICRO_VERSION",    OpenVarInputFormat.UNDEFINED,   "device micro version",                                             (input) => input, "[Version]*[Release]*[Build]");
            VARTYPE_Version =       new("Version",          OpenVarInputFormat.RANGE,       "Version",                                                          (input) => Math.Clamp(Convert.ToUInt32(input), 0, 99));
            VARTYPE_Release =       new("Release",          OpenVarInputFormat.RANGE,       "Release",                                                          (input) => Math.Clamp(Convert.ToUInt32(input), 0, 99));
            VARTYPE_Build =         new("Build",            OpenVarInputFormat.RANGE,       "Build",                                                            (input) => Math.Clamp(Convert.ToUInt32(input), 0, 99));
            VARTYPE_BITMASK_A =     new("BITMASK_DIA_A",    OpenVarInputFormat.BITMASK,     "bit of Diagnostic A",                                              (input) => Convert.ToUInt32(input,2));
            VARTYPE_BITMASK_B =     new("BITMASK_DIA_B",    OpenVarInputFormat.BITMASK,     "bit of Diagnostic B",                                              (input) => Convert.ToUInt32(input,2));
            VARTYPE_SLOT =          new("SLOT",             OpenVarInputFormat.RANGE,       "ko slot",                                                          (input) => Math.Clamp(Convert.ToUInt32(input), 1, 255));
            VARTYPE_KEYO =          new("KEYO",             OpenVarInputFormat.RANGE,       "device object model",                                              (input) => Math.Clamp(Convert.ToUInt32(input), 1, 65535));
            VARTYPE_STATE =         new("STATE",            OpenVarInputFormat.BOOLEAN,     "configured or not configured",                                     (input) => Convert.ToUInt32(input) > 0);
            VARTYPE_ERROR =         new("ERROR",            OpenVarInputFormat.BOOLEAN,     "error",                                                            (input) => Convert.ToUInt32(input) > 0);
            VARTYPE_SYS =           new("SYS",              OpenVarInputFormat.RANGE,       "KeyObject system",                                                 (input) => Math.Clamp(Convert.ToUInt32(input), 1, 255));
            VARTYPE_ADDR =          new("ADDR",             OpenVarInputFormat.RANGE,       "KeyObject address",                                                (input) => Math.Clamp(Convert.ToUInt32(input), 0, 65535));
            VARTYPE_VAL_PAR =       new("VAL_PAR",          OpenVarInputFormat.RANGE,       "Parameter value",                                                  (input) => Math.Clamp(Convert.ToUInt32(input), 0, 65535));
            VARTYPE_INDEX =         new("INDEX",            OpenVarInputFormat.RANGE,       "Parameter number (even kown as kconf index)",                      (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_LIVELLOLUX =    new("LIVELLOLUX",       OpenVarInputFormat.RANGE,       "LUX value",                                                        (input) => Math.Clamp(Convert.ToUInt32(input), 0, 65535));
            VARTYPE_MAC =           new("MAC",              OpenVarInputFormat.RANGE,       "Mac address",                                                      (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_HW_VERSION =    new("HW_VERSION",       OpenVarInputFormat.UNDEFINED,   "device hardware version",                                          (input) => input, "[Version]*[Release]*[Build]");
            VARTYPE_BIT =           new("BIT",              OpenVarInputFormat.BITMASK,     "bit of Diagnostic/Autodiagnostic",                                 (input) => Math.Clamp(Convert.ToUInt32(input,2), 0, 24));
            VARTYPE_OPERATIONS =    new("OPERATIONS",       OpenVarInputFormat.RANGE,       "the sequence of logic operations to be applied to the password",   (input) => Math.Clamp(Convert.ToUInt32(input), 0, 999999999));
            VARTYPE_RESULT =        new("RESULT",           OpenVarInputFormat.RANGE,       "Result of password",                                               (input) => Convert.ToUInt32(input));
            VARTYPE_SCE =           new("SCE",              OpenVarInputFormat.RANGE,       "scenario PPT",                                                     (input) => Math.Clamp(Convert.ToUInt32(input), 1, 255));
            VARTYPE_STATE_SCE =     new("STATE_SCE",        OpenVarInputFormat.RANGE,       "Scenarious state",                                                 (input) => Math.Clamp(Convert.ToUInt32(input), 1, 101));
            VARTYPE_MAC1 =          new("MAC1",             OpenVarInputFormat.RANGE,       "Mac address 1",                                                    (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_MAC2 =          new("MAC2",             OpenVarInputFormat.RANGE,       "Mac address 2",                                                    (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_MAC3 =          new("MAC3",             OpenVarInputFormat.RANGE,       "Mac address 3",                                                    (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_MAC4 =          new("MAC4",             OpenVarInputFormat.RANGE,       "Mac address 4",                                                    (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_MAC5 =          new("MAC5",             OpenVarInputFormat.RANGE,       "Mac address 5",                                                    (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_MAC6 =          new("MAC6",             OpenVarInputFormat.RANGE,       "Mac address 6",                                                    (input) => Math.Clamp(Convert.ToUInt32(input), 0, 255));
            VARTYPE_SCAN_TYPE =     new("SCAN_TYPE",        OpenVarInputFormat.ENUMERATE,   "Type of plant scanning by ID",                                     (input) => Convert.ToUInt32(input));

            LIST_VARTYPE.Add(VARTYPE_WHO);
            LIST_VARTYPE.Add(VARTYPE_WHERE);
            LIST_VARTYPE.Add(VARTYPE_OBJECT_MODEL);
            LIST_VARTYPE.Add(VARTYPE_N_CONF);
            LIST_VARTYPE.Add(VARTYPE_BRAND);
            LIST_VARTYPE.Add(VARTYPE_LINE);
            LIST_VARTYPE.Add(VARTYPE_ID);
            LIST_VARTYPE.Add(VARTYPE_FW_VERSION);
            LIST_VARTYPE.Add(VARTYPE_C1);
            LIST_VARTYPE.Add(VARTYPE_C2);
            LIST_VARTYPE.Add(VARTYPE_C3);
            LIST_VARTYPE.Add(VARTYPE_C4);
            LIST_VARTYPE.Add(VARTYPE_C5);
            LIST_VARTYPE.Add(VARTYPE_C6);
            LIST_VARTYPE.Add(VARTYPE_C7);
            LIST_VARTYPE.Add(VARTYPE_C8);
            LIST_VARTYPE.Add(VARTYPE_C9);
            LIST_VARTYPE.Add(VARTYPE_C10);
            LIST_VARTYPE.Add(VARTYPE_C11);
            LIST_VARTYPE.Add(VARTYPE_C12);
            LIST_VARTYPE.Add(VARTYPE_MICRO_VERSION);
            LIST_VARTYPE.Add(VARTYPE_Version);
            LIST_VARTYPE.Add(VARTYPE_Release);
            LIST_VARTYPE.Add(VARTYPE_Build);
            LIST_VARTYPE.Add(VARTYPE_BITMASK_A);
            LIST_VARTYPE.Add(VARTYPE_BITMASK_B);
            LIST_VARTYPE.Add(VARTYPE_SLOT);
            LIST_VARTYPE.Add(VARTYPE_KEYO);
            LIST_VARTYPE.Add(VARTYPE_STATE);
            LIST_VARTYPE.Add(VARTYPE_ERROR);
            LIST_VARTYPE.Add(VARTYPE_SYS);
            LIST_VARTYPE.Add(VARTYPE_ADDR);
            LIST_VARTYPE.Add(VARTYPE_VAL_PAR);
            LIST_VARTYPE.Add(VARTYPE_INDEX);
            LIST_VARTYPE.Add(VARTYPE_LIVELLOLUX);
            LIST_VARTYPE.Add(VARTYPE_MAC);
            LIST_VARTYPE.Add(VARTYPE_HW_VERSION);
            LIST_VARTYPE.Add(VARTYPE_BIT);
            LIST_VARTYPE.Add(VARTYPE_OPERATIONS);
            LIST_VARTYPE.Add(VARTYPE_RESULT);
            LIST_VARTYPE.Add(VARTYPE_SCE);
            LIST_VARTYPE.Add(VARTYPE_STATE_SCE);
            LIST_VARTYPE.Add(VARTYPE_MAC1);
            LIST_VARTYPE.Add(VARTYPE_MAC2);
            LIST_VARTYPE.Add(VARTYPE_MAC3);
            LIST_VARTYPE.Add(VARTYPE_MAC4);
            LIST_VARTYPE.Add(VARTYPE_MAC5);
            LIST_VARTYPE.Add(VARTYPE_MAC6);
            LIST_VARTYPE.Add(VARTYPE_SCAN_TYPE);
        }

        public OpenVarType GetOpenVarTypeByName(string name)
        {
            foreach(OpenVarType openVarType in LIST_VARTYPE)
            {
                if(openVarType.GetName().Equals(name))
                    return openVarType;
            }
            return null;
        }

    }

    public enum OpenVarInputFormat
    {
        RANGE,
        ENUMERATE,
        BITMASK,
        BOOLEAN,
        UNDEFINED
    }

    public class OpenVarType
    {
        readonly string name;
        readonly bool isExtended;
        readonly string extendedString;
        readonly OpenVarInputFormat format;
        readonly string descr;
        Func<string, object> converter;

        public OpenVarType(string _name, OpenVarInputFormat _format, string _descr, Func<string, object> _converter, string _extendedString = null)
        {
            name = _name;
            format = _format;
            descr = _descr;
            converter = _converter;

            if(_extendedString == null)
            {
                isExtended = false;
            }
            else
            {
                extendedString = _extendedString;
                isExtended = true;
            }
        }

        public string GetName()
        {
            return name;
        }

        public bool IsExtended()
        {
            return isExtended;
        }

        public string GetExtendedString()
        {
            return extendedString;
        }

        public OpenVarInputFormat GetOpenVarInputFormat()
        {
            return format;
        }

        public string GetDescription()
        {
            return descr;
        }

        public object GetValue(string input)
        {
            return converter(input);
        }
    }
}
