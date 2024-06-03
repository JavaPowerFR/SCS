using System.Reflection.Emit;

namespace ScsDecoder
{
    public class PacketSCS
    {
        PacketPatternArg[] pattern;
        string string_pattern;

        string description;
        int weight = 0;

        public PacketSCS(string _pattern, string _description)
        {
            string_pattern = _pattern;
            description = _description;

            string[] args = _pattern.Split(":");
            pattern = new PacketPatternArg[args.Length];
            
            int i = 0;
            foreach (string arg in args)
            {
                bool isDontCare = false;
                int indexCompareStart = arg.IndexOf('[');
                int indexCompareEnd = arg.IndexOf(']');
                string arg2 = arg;

                string varName = null;
                PacketPatternArg.ComparisonType comparisonType = PacketPatternArg.ComparisonType.DontCare;


                if (indexCompareStart != -1 && indexCompareEnd != -1)
                {
                    varName = arg2.Substring(indexCompareStart + 1, indexCompareEnd - 1 - indexCompareStart);
                    arg2 = arg2.Remove(indexCompareStart, indexCompareEnd + 1 - indexCompareStart);
                }

                if(arg2.Equals("*"))
                {
                    pattern[i] = new PacketPatternArg(PacketPatternArg.ComparisonType.DontCare, 0);
                    isDontCare = true;
                }
                else if (arg2.StartsWith("!"))
                {
                    comparisonType = PacketPatternArg.ComparisonType.NotEqual;
                    arg2 = arg2.Substring(1);
                }
                else if (arg2.StartsWith("<"))
                {
                    comparisonType = PacketPatternArg.ComparisonType.LowerThan;
                    arg2 = arg2.Substring(1);
                }
                else if (arg2.StartsWith(">"))
                {
                    comparisonType = PacketPatternArg.ComparisonType.GreaterThan;
                    arg2 = arg2.Substring(1);
                }
                else if (arg2.StartsWith("H="))
                {
                    comparisonType = PacketPatternArg.ComparisonType.HighEqual;
                    arg2 = arg2.Substring(2);
                }
                else if (arg2.StartsWith("L="))
                {
                    comparisonType = PacketPatternArg.ComparisonType.LowEqual;
                    arg2 = arg2.Substring(2);
                }
                else if (arg2.StartsWith("H!="))
                {
                    comparisonType = PacketPatternArg.ComparisonType.HighNotEqual;
                    arg2 = arg2.Substring(3);
                }
                else if (arg2.StartsWith("L!="))
                {
                    comparisonType = PacketPatternArg.ComparisonType.LowNotEqual;
                    arg2 = arg2.Substring(3);
                }
                else if (arg2.StartsWith("H>"))
                {
                    comparisonType = PacketPatternArg.ComparisonType.HighGreaterThan;
                    arg2 = arg2.Substring(2);
                }
                else if (arg2.StartsWith("L>"))
                {
                    comparisonType = PacketPatternArg.ComparisonType.LowGreaterThan;
                    arg2 = arg2.Substring(2);
                }
                else if (arg2.StartsWith("H<"))
                {
                    comparisonType = PacketPatternArg.ComparisonType.HighLowerThan;
                    arg2 = arg2.Substring(2);
                }
                else if (arg2.StartsWith("L<"))
                {
                    comparisonType = PacketPatternArg.ComparisonType.LowLowerThan;
                    arg2 = arg2.Substring(2);
                }

                List<byte> values = new List<byte>();

                if (arg2.Length > 0)
                {
                    string[] argsValue = arg2.Split(',');

                    foreach (string valueString in argsValue)
                    {
                        if(valueString.StartsWith('(') && valueString.Contains('-') && valueString.EndsWith(')'))
                        {
                            string[] minMax = valueString.Substring(1, valueString.Length - 2).Split("-");
                            if (minMax.Length != 2)
                                throw new Exception("Packet format error");

                            values.Add(ConvertString(minMax[0]));
                            values.Add(ConvertString(minMax[1]));

                            //Range
                            if (comparisonType == PacketPatternArg.ComparisonType.DontCare)
                                comparisonType = PacketPatternArg.ComparisonType.InRange;

                            else if (comparisonType == PacketPatternArg.ComparisonType.LowEqual)
                                comparisonType = PacketPatternArg.ComparisonType.LowInRange;
                            
                            else if (comparisonType == PacketPatternArg.ComparisonType.HighEqual)
                                comparisonType = PacketPatternArg.ComparisonType.HighInRange;
                        }
                        else
                        {
                            if(valueString.Equals("*"))
                                values.Add(0);
                            else
                                values.Add(ConvertString(valueString));
                        }
                    }

                    if (!isDontCare && varName == null && comparisonType == PacketPatternArg.ComparisonType.DontCare)
                        comparisonType = PacketPatternArg.ComparisonType.Equal;
                }

                pattern[i] = new PacketPatternArg(comparisonType, varName, values.ToArray());

                i++;
            }

            foreach(var ptrn in pattern)
                weight += ptrn.Weight();
        }

        public int GetWeight()
        {
            return weight;
        }

        public string GetDescription()
        {
            return description;
        }

        public string GetPatternString()
        {
            return string_pattern;
        }

        public bool ShowIfIsCompatible(byte[] buffer)
        {
            Dictionary<string, byte> datas = new Dictionary<string, byte>();

            int size = Math.Min(buffer.Length, pattern.Length);
            for(int i = 0; i < size; ++i)
            {
                byte b = buffer[i];
                if(pattern[i].Equal(b))
                {
                    if(pattern[i].GetVarName() != null)
                        datas.Add(pattern[i].GetVarName(), b);
                }
                else
                    return false;
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{description}:");
            Console.ForegroundColor= ConsoleColor.Gray;

            foreach (var kv in datas)
            {
                Console.WriteLine($"[{kv.Key}]=0x{kv.Value.ToString("X2")}");
            }

            return true;
        }

        public bool GetIfIsCompatible(byte[] buffer, Dictionary<string, byte> datas)
        {
            int size = Math.Min(buffer.Length, pattern.Length);
            for (int i = 0; i < size; ++i)
            {
                byte b = buffer[i];
                if (pattern[i].Equal(b))
                {
                    if (pattern[i].GetVarName() != null)
                        datas.Add(pattern[i].GetVarName(), b);
                }
                else
                    return false;
            }

            return true;
        }

        internal byte ConvertString(string valueString)
        {
            if (valueString.StartsWith("0x"))
                return Convert.ToByte(valueString.Substring(2), 16);
            else
                return Convert.ToByte(valueString);
        }
    }

    public class PacketTransformSCS : PacketSCS
    {
        string string_patternOut;

        byte[] pattern_passThrough_flag, pattern_passThrough_value;

        public PacketTransformSCS(string _patternIn, string _patternOutPatch, string _description) : base(_patternIn, _description)
        {
            string_patternOut = _patternOutPatch;

            string[] args = string_patternOut.Split(":");

            pattern_passThrough_flag = new byte[args.Length];
            pattern_passThrough_value = new byte[args.Length];

            int i = 0;
            foreach (string arg in args)
            {
                string arg2 = arg;

                if (arg2.StartsWith('&')) // and
                {
                    pattern_passThrough_flag[i] = 1;
                    arg2 = arg2.Substring(1);
                }
                else if(arg2.StartsWith('|')) // or
                {
                    pattern_passThrough_flag[i] = 2;
                    arg2 = arg2.Substring(1);
                }
                else if (arg2.StartsWith('^')) // xor
                {
                    pattern_passThrough_flag[i] = 3;
                    arg2 = arg2.Substring(1);
                }
                else if (arg2.StartsWith('!')) // not
                {
                    pattern_passThrough_flag[i] = 4;
                    arg2 = arg2.Substring(1);
                }
                else if (arg2.StartsWith('+')) // add
                {
                    pattern_passThrough_flag[i] = 5;
                    arg2 = arg2.Substring(1);
                }
                else if (arg2.StartsWith('-')) // sub
                {
                    pattern_passThrough_flag[i] = 6;
                    arg2 = arg2.Substring(1);
                }
                else if (arg2.StartsWith('/')) // div
                {
                    pattern_passThrough_flag[i] = 7;
                    arg2 = arg2.Substring(1);
                }
                else if (arg2.StartsWith('x')) // multiply
                {
                    pattern_passThrough_flag[i] = 8;
                    arg2 = arg2.Substring(1);
                }
                else if (arg2.StartsWith('%')) // modulo
                {
                    pattern_passThrough_flag[i] = 9;
                    arg2 = arg2.Substring(1);
                }

                if(arg2.Equals("*"))
                {
                    pattern_passThrough_flag[i] = 0xFF;
                }
                else if(arg2.Length > 0)
                {
                    pattern_passThrough_value[i] = ConvertString(arg2);
                }

                ++i;
            }
        }

        public bool ShowAndApplyPathIfIsCompatible(byte[] buffer, out byte[] bufferOut)
        {
            bufferOut = new byte[buffer.Length];
            for(int i = 0; i < buffer.Length; ++i)
                bufferOut[i] = buffer[i];

            if(ShowIfIsCompatible(buffer))
            {
                for (int i = 0; i < buffer.Length; ++i)
                {
                    if (pattern_passThrough_flag[i] != 0xFF)
                    {
                        if (pattern_passThrough_flag[i] == 0)
                            bufferOut[i] = pattern_passThrough_value[i];

                        else if (pattern_passThrough_flag[i] == 1)
                            bufferOut[i] &= pattern_passThrough_value[i];

                        else if (pattern_passThrough_flag[i] == 2)
                            bufferOut[i] |= pattern_passThrough_value[i];

                        else if (pattern_passThrough_flag[i] == 3)
                            bufferOut[i] ^= pattern_passThrough_value[i];

                        else if (pattern_passThrough_flag[i] == 4)
                            bufferOut[i] ^= 0xFF;

                        else if (pattern_passThrough_flag[i] == 5)
                            bufferOut[i] += pattern_passThrough_value[i];

                        else if (pattern_passThrough_flag[i] == 6)
                            bufferOut[i] -= pattern_passThrough_value[i];

                        else if (pattern_passThrough_flag[i] == 7)
                            bufferOut[i] /= pattern_passThrough_value[i];

                        else if (pattern_passThrough_flag[i] == 8)
                            bufferOut[i] *= pattern_passThrough_value[i];

                        else if (pattern_passThrough_flag[i] == 9)
                            bufferOut[i] %= pattern_passThrough_value[i];

                    }
                }

                return true;
            }

            return false;
        }

        public bool ApplyPathIfIsCompatible(byte[] buffer, out byte[] bufferOut, Dictionary<string, byte> datas)
        {
            bufferOut = new byte[buffer.Length];
            for (int i = 0; i < buffer.Length; ++i)
                bufferOut[i] = buffer[i];

            if (GetIfIsCompatible(buffer, datas))
            {
                for (int i = 0; i < buffer.Length; ++i)
                {
                    if (pattern_passThrough_flag[i] != 0xFF)
                    {
                        if (pattern_passThrough_flag[i] == 0)
                            bufferOut[i] = pattern_passThrough_value[i];

                        else if (pattern_passThrough_flag[i] == 1)
                            bufferOut[i] &= pattern_passThrough_value[i];

                        else if (pattern_passThrough_flag[i] == 2)
                            bufferOut[i] |= pattern_passThrough_value[i];

                        else if (pattern_passThrough_flag[i] == 3)
                            bufferOut[i] ^= pattern_passThrough_value[i];

                        else if (pattern_passThrough_flag[i] == 4)
                            bufferOut[i] ^= 0xFF;

                        else if (pattern_passThrough_flag[i] == 5)
                            bufferOut[i] += pattern_passThrough_value[i];

                        else if (pattern_passThrough_flag[i] == 6)
                            bufferOut[i] -= pattern_passThrough_value[i];

                        else if (pattern_passThrough_flag[i] == 7)
                            bufferOut[i] /= pattern_passThrough_value[i];

                        else if (pattern_passThrough_flag[i] == 8)
                            bufferOut[i] *= pattern_passThrough_value[i];

                        else if (pattern_passThrough_flag[i] == 9)
                            bufferOut[i] %= pattern_passThrough_value[i];

                    }
                }

                return true;
            }

            return false;
        }
    }
}
