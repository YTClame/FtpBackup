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
    public class FileAndDirInfo
    {
        public List<string> pathParts;
        public bool isFolder;
        public string pathUI;
        [JsonIgnore]
        public bool isRemoving;
        public FileAndDirInfo(List<string> pathParts, bool isFolder, string pathUI)
        {
            this.pathParts = pathParts;
            this.isFolder = isFolder;
            this.pathUI = pathUI;
            this.isRemoving = false;
        }
    }
    public class Record
    {
        public string IP;
        public int port;
        public string login;
        public string password;
        public string name;
        public List<FileAndDirInfo> filesAndDirs;
        public DateTime nextSaveDateTime;
        public string folderPath;
        public int periodH;
        public int periodM;
        public int periodS;
        
        [JsonIgnore]
        public FtpClient ftpClient;

        public Record(string IP, int port, string login, string password, string name)
        {
            this.IP = IP;
            this.port = port;
            this.login = login;
            this.password = password;
            this.name = name;
            this.filesAndDirs = new List<FileAndDirInfo>();
        }

        public void RemoveFileOrDir(string path)
        {
            List<FileAndDirInfo> list = filesAndDirs;
            foreach(FileAndDirInfo fadi in list)
            {
                if (fadi.pathUI.Equals(path))
                {
                    filesAndDirs.Remove(fadi);
                    return;
                }
            }
        }

        public static Record findRecordForName(string name)
        {
            foreach(Record rec in GlobalVars.records)
            {
                if (rec.name.Equals(name)) return rec;
            }
            return null;
        }
        public static FileAndDirInfo findFileAndDirInfoForPathUI(Record rec, string path)
        {
            foreach (FileAndDirInfo f in rec.filesAndDirs)
            {
                if (f.pathUI.Equals(path)) return f;
            }
            return null;
        }
    }
}
