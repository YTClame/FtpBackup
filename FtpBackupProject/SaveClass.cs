using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FtpBackupProject
{
    public class SaveClass
    {
        private static StreamWriter swLOG;
        private static StreamWriter sw;
        private static int counter = 0;
        public static void OpenLogFile()
        {
            try
            {
                swLOG = new StreamWriter("Log.txt", true);
            }
            catch
            {
                swLOG = null;
            }
        }
        public static void WriteToLogFile(string message)
        {
            counter++;
            try
            {
                swLOG.Write("[" + DateTime.Now.ToString() + "]: " + message + "\r\n");
                if(counter >= 10)
                {
                    counter = 0;
                    CloseLogFile();
                    OpenLogFile();
                }
            }
            catch
            {

            }
        }
        public static void CloseLogFile()
        {
            try
            {
                swLOG.Close();
            }
            catch
            {

            }
        }
        public static void SaveAll()
        {
            try
            {
                sw = new StreamWriter("settings.json");
                sw.Write(JsonConvert.SerializeObject(GlobalVars.records, Formatting.Indented));
                sw.Close();
            }
            catch
            {
                try
                {
                    sw.Write(JsonConvert.SerializeObject(GlobalVars.records, Formatting.Indented));
                    sw.Close();
                }
                catch { }
            }
        }
        public static void LoadAll()
        {
            try
            {
                StreamReader sr = new StreamReader("settings.json");
                GlobalVars.records = JsonConvert.DeserializeObject<List<Record>>(sr.ReadToEnd());
                sr.Close();
                if (GlobalVars.records == null) throw new Exception();
            }
            catch
            {
                GlobalVars.records = new List<Record>();
            }
        }

        public static void SaveTempRec(Record rec)
        {
            try
            {
                StreamWriter sw = new StreamWriter("TempRecord_" + rec.name + ".json");
                sw.Write(JsonConvert.SerializeObject(rec, Formatting.Indented));
                sw.Close();
            }
            catch{

            }
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
