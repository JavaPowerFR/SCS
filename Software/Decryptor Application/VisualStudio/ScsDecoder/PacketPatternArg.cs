namespace ScsDecoder
{
    public class PacketPatternArg
    {
        public enum ComparisonType
        {
            DontCare = 0,
            Equal = 1,
            NotEqual = 2,
            GreaterThan = 3,
            LowerThan = 4,
            InRange = 5,
            LowEqual = 6,
            HighEqual = 7,
            LowNotEqual = 8,
            HighNotEqual = 9,
            LowGreaterThan = 10,
            HighGreaterThan = 11,
            LowLowerThan = 12,
            HighLowerThan = 13,
            LowInRange = 14,
            HighInRange = 15,
        }
        private readonly byte[] values;
        private readonly ComparisonType comparisonType;
        private readonly string varName;
        public PacketPatternArg(ComparisonType _comparisonType, params byte[] vals) : this(_comparisonType, null, vals)
        {

        }

        public PacketPatternArg(ComparisonType _comparisonType, string _varName, params byte[] vals)
        {
            comparisonType = _comparisonType;
            values = vals;
            varName = _varName;

            if (comparisonType == ComparisonType.InRange || comparisonType == ComparisonType.LowInRange || comparisonType == ComparisonType.HighInRange)
            {
                if (values.Length % 2 > 0)
                    throw new Exception($"PacketPattern {comparisonType} need a multiple of 2 arguments (min,max)");
                for (int i = 0; i < values.Length; i += 2)
                {
                    if (values[i] > values[i + 1])
                        throw new Exception($"PacketPattern {comparisonType} nedd (min,max) and not (max,min)");
                }
            }
        }

        public int Weight()
        {
            if (varName != null)
            {
                switch (comparisonType)
                {
                    case ComparisonType.DontCare:
                        return 0;

                    case ComparisonType.Equal:
                    case ComparisonType.NotEqual:
                        return 4;

                    default:
                        return 2;
                }
            }
            else
            {
                switch (comparisonType)
                {
                    case ComparisonType.DontCare:
                        return 6;

                    case ComparisonType.Equal:
                    case ComparisonType.NotEqual:
                        return 10;

                    default:
                        return 8;
                }
            }
        }

        public bool Equal(byte byteIn)
        {
            switch(comparisonType)
            {
                case ComparisonType.DontCare:
                    return true;

                case ComparisonType.Equal:
                    foreach (byte b in values)
                    {
                        if (byteIn == b)
                            return true;
                    }
                    return false;

                case ComparisonType.NotEqual:
                    foreach (byte b in values)
                    {
                        if (byteIn == b)
                            return false;
                    }
                    return true;

                case ComparisonType.GreaterThan:
                    foreach (byte b in values)
                    {
                        if (byteIn > b)
                            return true;
                    }
                    return false;

                case ComparisonType.LowerThan:
                    foreach (byte b in values)
                    {
                        if (byteIn < b)
                            return true;
                    }
                    return false;

                case ComparisonType.InRange:
                    for(int i = 0; i < values.Length; i += 2)
                    {
                        if (byteIn >= values[i] && byteIn <= values[i + 1])
                            return true;
                    }
                    return false;

                case ComparisonType.LowEqual:
                    foreach (byte b in values)
                    {
                        if ((byteIn & 0x0F) == b)
                            return true;
                    }
                    return false;

                case ComparisonType.HighEqual:
                    foreach (byte b in values)
                    {
                        if ((byteIn & 0xF0) == b)
                            return true;
                    }
                    return false;

                case ComparisonType.LowNotEqual:
                    foreach (byte b in values)
                    {
                        if ((byteIn & 0x0F) == b)
                            return false;
                    }
                    return true;

                case ComparisonType.HighNotEqual:
                    foreach (byte b in values)
                    {
                        if ((byteIn & 0xF0) == b)
                            return false;
                    }
                    return true;

                case ComparisonType.LowGreaterThan:
                    foreach (byte b in values)
                    {
                        if ((byteIn & 0x0F) > b)
                            return true;
                    }
                    return false;

                case ComparisonType.HighGreaterThan:
                    foreach (byte b in values)
                    {
                        if ((byteIn & 0xF0) > b)
                            return true;
                    }
                    return false;

                case ComparisonType.LowLowerThan:
                    foreach (byte b in values)
                    {
                        if ((byteIn & 0x0F) < b)
                            return true;
                    }
                    return false;

                case ComparisonType.HighLowerThan:
                    foreach (byte b in values)
                    {
                        if ((byteIn & 0xF0) < b)
                            return true;
                    }
                    return false;

                case ComparisonType.LowInRange:
                    for (int i = 0; i < values.Length; i += 2)
                    {
                        if ((byteIn & 0x0F) >= values[i] && (byteIn & 0x0F) <= values[i + 1])
                            return true;
                    }
                    return false;

                case ComparisonType.HighInRange:
                    for (int i = 0; i < values.Length; i += 2)
                    {
                        if ((byteIn & 0xF0) >= values[i] && (byteIn & 0xF0) <= values[i + 1])
                            return true;
                    }
                    return false;

                default:
                    return false;
            }
        }

        public string GetVarName()
        {
            return varName;
        }
    }
}
