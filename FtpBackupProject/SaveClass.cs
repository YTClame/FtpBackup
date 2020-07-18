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
                sr.Close();
            }
            catch
            {
                GlobalVars.records = new List<Record>();
            }
        }

        public static void SaveTempRec(Record rec)
        {
            StreamWriter sw = new StreamWriter("TempRecord_"+rec.name+".json");
            sw.Write(JsonConvert.SerializeObject(rec, Formatting.Indented));
            sw.Close();
        }

        public static Record LoadTempRec(string name)
        {
            try
            {
                StreamReader sr = new StreamReader("TempRecord_" + name + ".json");
                Record record = JsonConvert.DeserializeObject<Record>(sr.ReadToEnd());
                sr.Close();
                return record;
            }
            catch
            {
                return null;
            }
        }
    }
}
