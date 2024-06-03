namespace TcpSnifferOWN
{

    public class OpenCmd
    {
        public readonly int id_open;
        public readonly string open_label;
        public readonly string descr;
        public readonly string open_string;
        public readonly bool has_address;
        public readonly bool has_param;
        public readonly OPEN_TYPE open_type;
        public readonly bool diag_open;
        public readonly int error_open;
        public readonly bool open4keyo;

        public OpenCmd(int id_open, string open_label, string descr, string open_string, bool has_address, bool has_param, int open_type, bool diag_open, int error_open, bool open4keyo)
        {
            this.id_open = id_open;
            this.open_label = open_label;
            this.descr = descr;
            this.open_string = open_string;
            this.has_address = has_address;
            this.has_param = has_param;
            this.open_type = (OPEN_TYPE)open_type;
            this.diag_open = diag_open;
            this.error_open = error_open;
            this.open4keyo = open4keyo;
        }

        public string getDescription()
        {
            return descr;
        }


        public bool isCompatible(string open, out Dictionary<OpenVarType, object> map_values)
        {
            map_values = new Dictionary<OpenVarType, object>();
            if (open.StartsWith("*") && open.EndsWith("##"))
            {
                bool isCompatible = true;

                int this_index = 0;
                int param_index = 0;
                int flagMarkEnd = 0;

                while(true)
                {
                    if (this_index >= open_string.Length || param_index >= open.Length)
                    {
                        if(flagMarkEnd != 2)
                            isCompatible = false;
                        break;
                    }
                    char this_char = open_string[this_index];
                    char param_char = open[param_index];
                    //Console.WriteLine(this_char+"="+param_char);
                    if (this_char.Equals('*'))
                    {
                        if (param_char.Equals('*'))
                        {
                            flagMarkEnd = 0;
                            ++this_index;
                            ++param_index;
                        }
                        else
                        {
                            isCompatible = false;
                            break;
                        }
                    }
                    else if (this_char.Equals('#'))
                    {
                        if (param_char.Equals('#'))
                        {
                            ++flagMarkEnd;
                            ++this_index;
                            ++param_index;
                        }
                        else
                        {
                            isCompatible = false;
                            break;
                        }
                    }
                    else if(this_char.Equals('['))
                    {
                        flagMarkEnd = 0;
                        int var_end_address = open_string.IndexOf(']', this_index+1);
                        string var_name = open_string.Substring(this_index+1, var_end_address - this_index-1);

                        int var_value_end_address = open.IndexOfAny(new char[] { '*','#' }, param_index);
                        string var_vlaue = open.Substring(param_index, var_value_end_address - param_index);

                        OpenVarType openVarType = OpenVarTypes.GetInstance().GetOpenVarTypeByName(var_name);
                        if (openVarType != null)
                        {
                            if (openVarType.IsExtended())
                            {
                                string[] extVars = openVarType.GetExtendedString().Split('*');
                                if (extVars.Length > 0)
                                {
                                    var_name = extVars[0].Substring(1, extVars[0].Length - 2);
                                    openVarType = OpenVarTypes.GetInstance().GetOpenVarTypeByName(var_name);
                                    if (openVarType != null)
                                    {
                                        try
                                        {
                                            map_values.Add(openVarType, openVarType.GetValue(var_vlaue));
                                        }
                                        catch { }

                                    }
                                    param_index = var_value_end_address+1;

                                    for (int i = 1; i < extVars.Length; i++)
                                    {
                                        var_value_end_address = open.IndexOfAny(new char[] { '*', '#' }, param_index);
                                        var_vlaue = open.Substring(param_index, var_value_end_address - param_index);

                                        var_name = extVars[i].Substring(1, extVars[i].Length - 2);
                                        openVarType = OpenVarTypes.GetInstance().GetOpenVarTypeByName(var_name);

                                        if (openVarType != null)
                                        {
                                            try
                                            {
                                                map_values.Add(openVarType, openVarType.GetValue(var_vlaue));
                                            }
                                            catch { }

                                        }

                                        param_index = var_value_end_address+1;
                                    }
                                }
                            }
                            else
                            { 
                                try
                                {
                                    map_values.Add(openVarType, openVarType.GetValue(var_vlaue));
                                }
                                catch { }
                            }
                        }
                        //Console.WriteLine(var_name+" - "+var_vlaue);

                        this_index = var_end_address + 1;
                        param_index = var_value_end_address;
                        //break;
                    }
                    else if("0123456789".IndexOf(this_char) != -1)
                    {
                        flagMarkEnd = 0;
                        if (open_string[this_index] == open[param_index])
                        {
                            ++this_index;
                            ++param_index;
                        }
                        else
                        {
                            isCompatible = false;
                            break;
                        }
                    }
                }

                if(isCompatible)
                {
                    OpenParser.WHO how = OpenParser.WHO.UNDEFINED;
                    foreach(var L_value in map_values)
                    {
                        if(L_value.Key.GetName().Equals("WHO"))
                        {
                            how = (OpenParser.WHO)L_value.Value;
                        }
                    }

                    if((int)how > 1000)
                    {
                        if(!diag_open)
                        {
                            map_values.Clear();
                            return false;
                        }
                    }
                    else
                    {
                        if (diag_open)
                        {
                            map_values.Clear();
                            return false;
                        }
                    }
                }
                return isCompatible;
            }
            return false;
        }
    }

    public enum OPEN_TYPE
    {
        DEVICE_TO_PROGRAMMER,
        PROGRAMMER_TO_DEVICE
    }
}
