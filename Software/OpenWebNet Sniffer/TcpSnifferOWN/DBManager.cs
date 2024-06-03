using System.Data;

namespace TcpSnifferOWN
{
    public class DBManager
    {
        private static DBManager? instance;

        private DBInstance open_db, mhcatalogue_db;
        List<OpenCmd> openCmds = new List<OpenCmd>();

        public static DBManager getInstance()
        {
            if(instance == null)
                instance = new DBManager();
            return instance;
        }

        public DBManager()
        {
            open_db = new DBInstance(@"F:\Users\Cyril\Documents\Projets\SCS Interface\Software\DataBase\OPEN.db");
            mhcatalogue_db = new DBInstance(@"F:\Users\Cyril\Documents\Projets\SCS Interface\Software\DataBase\MHCatalogue.db");


            DataTable dt = open_db.selectQuery("SELECT * FROM EN_OPEN");
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                int id_open = (int)dr.ItemArray[0];
                string open_label = (string)dr.ItemArray[1];
                string descr = (string)dr.ItemArray[2];
                string open_string = (string)dr.ItemArray[3];
                bool has_address = (bool)dr.ItemArray[4];
                bool has_param = (bool)dr.ItemArray[5];
                int open_type = (int)dr.ItemArray[6];
                bool diag_open = (bool)dr.ItemArray[7];
                int error_open = (int)dr.ItemArray[8];
                bool open4keyo = (bool)dr.ItemArray[9];

                openCmds.Add(new OpenCmd(id_open, open_label, descr, open_string, has_address, has_param, open_type, diag_open, error_open, open4keyo));
            }
            
        }

        public void showDescriptionForThisOPENCommand(string open, Side side)
        {
            foreach (OpenCmd openCmd in openCmds)
            {
                if (
                    (side == Side.CLIENT_TO_SERVER && openCmd.open_type == OPEN_TYPE.PROGRAMMER_TO_DEVICE) ||
                    (side == Side.SERVER_TO_CLIENT && openCmd.open_type == OPEN_TYPE.DEVICE_TO_PROGRAMMER)
                   )
                {
                    Dictionary<OpenVarType, object> map_values;
                    if (openCmd.isCompatible(open, out map_values))
                    {
                        ConsoleWrite(ConsoleColor.Blue, openCmd.getDescription() + ":\n");
                        foreach (var val in map_values)
                        {
                            ConsoleWrite(ConsoleColor.Green, $"{val.Key.GetDescription()}");
                            ConsoleWrite(ConsoleColor.Gray, " [");
                            ConsoleWrite(ConsoleColor.Magenta, val.Key.GetName());
                            ConsoleWrite(ConsoleColor.Gray, "] = (");
                            ConsoleWrite(ConsoleColor.Magenta, val.Value.GetType().Name);
                            ConsoleWrite(ConsoleColor.Gray, ") ");
                            ConsoleWrite(ConsoleColor.Yellow, val.Value.ToString() + "\n");

                        }
                    }
                }
            }
        }

        private static void ConsoleWrite(ConsoleColor color, string msg)
        {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ForegroundColor = old;
        }
    }
}
