using FluentFTP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FtpBackupProject
{
    public class Record
    {
        public string IP;
        public int port;
        public string login;
        public string password;
        public string name;

        [JsonIgnore]
        public bool isOnline;
        [JsonIgnore]
        public FtpClient ftpClient;

        public Record(string IP, int port, string login, string password, string name)
        {
            this.IP = IP;
            this.port = port;
            this.login = login;
            this.password = password;
            this.name = name;
        }
    }
}
