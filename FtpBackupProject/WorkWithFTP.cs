using FluentFTP;
using FluentFTP.Proxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FtpBackupProject
{
    public class WorkWithFTP
    {
        public static async void ConnectToFTP(Record rec, AddControllerForm acf)
        {
            rec.ftpClient = new FtpClient(rec.IP, rec.port, new System.Net.NetworkCredential(rec.login, rec.password));
            try
            {
                rec.ftpClient.ConnectTimeout = 5000;
                await rec.ftpClient.ConnectAsync();

                if (rec.ftpClient.IsConnected)
                {
                    rec.isOnline = true;
                }
                else
                {
                    rec.isOnline = false;
                }
                acf.ConnectionResult(rec.isOnline);
            }
            catch
            {
                rec.isOnline = false;
                acf.ConnectionResult(rec.isOnline);
            }
        }

        public static async void Download(Record rec, AddControllerForm acf)
        {
            string dir = rec.folderPath + rec.name + "\\" + DateTime.Now.ToString().Replace(':', '.');
            Directory.CreateDirectory(dir);
            await Task.Run(() =>
            {
                foreach(FileAndDirInfo fi in rec.filesAndDirs)
                {
                    string pathOnFtp = "";
                    string dirTemp = dir;
                    foreach(string partPath in fi.pathParts)
                    {
                        pathOnFtp += "/" + partPath;
                        dirTemp += "\\" + partPath;
                    }
                    if (pathOnFtp.Equals("")) pathOnFtp = "/";
                    if (fi.isFolder)
                    {
                        rec.ftpClient.DownloadDirectory(dirTemp, pathOnFtp, FtpFolderSyncMode.Update);
                    }
                    else
                    {
                        rec.ftpClient.DownloadFile(dirTemp, pathOnFtp);
                    }
                }
            });
            rec.ftpClient.Disconnect();
            rec.isOnline = false;
            SaveClass.SaveAll();
            acf.Close();
        }

        public static async void getAllFilesAndDirs(Record rec, AddControllerForm acf)
        {
            folders = new Queue<FolderInfo>();
            TreeNode tree = new TreeNode(rec.name, 0, 0);
            folders.Enqueue(new FolderInfo("", tree, null));
            await Task.Run(() =>
            {
                while (folders.Count > 0)
                {
                    FolderInfo fi = folders.Dequeue();
                    CheckDirectory(rec, fi.fullDir, fi.node);
                }
            });
            acf.SetTreeNode(tree);
        }

        private static Queue<FolderInfo> folders;

        private static void CheckDirectory(Record rec, string path, TreeNode parent)
        {
            foreach (FtpListItem item in rec.ftpClient.GetListing(path))
            {
                if (item.Type == FtpFileSystemObjectType.File)
                {
                    TreeNode tempFileNode = new TreeNode(item.Name, 1, 1);
                    if (parent != null) parent.Nodes.Add(tempFileNode);
                }
                if (item.Type == FtpFileSystemObjectType.Directory)
                {
                    TreeNode tempFolderNode = new TreeNode(item.Name, 0, 0);
                    if (parent != null) parent.Nodes.Add(tempFolderNode);
                    folders.Enqueue(new FolderInfo(item.FullName, tempFolderNode, parent));
                }
            }
        }

        private class FolderInfo
        {
            public string fullDir;
            public TreeNode node;
            public TreeNode parent;
            public FolderInfo(string fullDir, TreeNode node, TreeNode parent)
            {
                this.fullDir = fullDir;
                this.node = node;
                this.parent = parent;
            }
        }
    }
}
