using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpBackupProject
{
    public class GlobalVars
    {
        public static List<Record> records;
        public static int maxLogSize;
        public static int savingMode;
    }
    public class Saving
    {
        public List<Record> records;
        public int maxLogSize;
        public int savingMode;
        public Saving(List<Record> records, int maxLogSize, int savingMode)
        {
            this.records = records;
            this.maxLogSize = maxLogSize;
            this.savingMode = savingMode;
        }
    }
}
