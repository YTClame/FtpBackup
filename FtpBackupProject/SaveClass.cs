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
        private static StreamWriter sw;
        private static Queue<LogInfo> messages;
        public static void WriteToLogFile(string message)
        {
            if(GlobalVars.savingMode != -1)
            {
                try
                {
                    if(GlobalVars.savingMode == 1)
                    {
                        FileInfo file = new System.IO.FileInfo("Log.txt");
                        long size = file.Length;
                        if (size > Convert.ToInt64(GlobalVars.maxLogSize * 1024 * 1024)) File.Delete("Log.txt");
                    }
                }
                catch { }
                if (messages == null) messages = new Queue<LogInfo>();
                try
                {
                    bool isExist = File.Exists("Log.txt");
                    StreamWriter swLOG = new StreamWriter("Log.txt", true);
                    while (messages.Count > 0)
                    {
                        LogInfo li = messages.Dequeue();
                        if(li.mes.Equals("Программа запущена.") && isExist)
                        {
                            swLOG.Write("\r\n[" + li.date.ToString() + "]: " + li.mes + "\r\n");
                        }
                        else
                        {
                            swLOG.Write("[" + li.date.ToString() + "]: " + li.mes + "\r\n");
                        }
                    }
                    if(message.Equals("Программа запущена.") && isExist)
                    {
                        swLOG.Write("\r\n[" + DateTime.Now.ToString() + "]: " + message + "\r\n");
                    }
                    else
                    {
                        swLOG.Write("[" + DateTime.Now.ToString() + "]: " + message + "\r\n");
                    }
                    swLOG.Close();
                }
                catch
                {
                    messages.Enqueue(new LogInfo(message));
                }
            }
        }
        private class LogInfo
        {
            public string mes;
            public DateTime date;
            public LogInfo(string mes)
            {
                this.mes = mes;
                date = DateTime.Now;
            }
        }
        public static void SaveAll()
        {
            try
            {
                sw = new StreamWriter("settings.json");
                sw.Write(JsonConvert.SerializeObject(new Saving(GlobalVars.records, GlobalVars.maxLogSize, GlobalVars.savingMode), Formatting.Indented));
                sw.Close();
            }
            catch(Exception e)
            {
                WriteToLogFile("Ошибка сохранения settings.json: " + e.Message);
            }
        }
        public static void LoadAll()
        {
            try
            {
                StreamReader sr = new StreamReader("settings.json");
                Saving save = JsonConvert.DeserializeObject<Saving>(sr.ReadToEnd());
                sr.Close();
                GlobalVars.records = save.records;
                GlobalVars.maxLogSize = save.maxLogSize;
                GlobalVars.savingMode = save.savingMode;
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
