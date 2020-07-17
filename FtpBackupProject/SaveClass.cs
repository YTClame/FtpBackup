using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpBackupProject
{
    public class SaveClass
    {
        public static void SaveAll()
        {
            StreamWriter sw = new StreamWriter("settings.json");
            sw.Write(JsonConvert.SerializeObject(GlobalVars.records, Formatting.Indented));
            sw.Close();
        }
        public static void LoadAll()
        {
            try
            {
                StreamReader sr = new StreamReader("settings.json");
                GlobalVars.records = JsonConvert.DeserializeObject<List<Record>>(sr.ReadToEnd());
            }
            catch
            {
                GlobalVars.records = new List<Record>();
            }
        }
    }
}
