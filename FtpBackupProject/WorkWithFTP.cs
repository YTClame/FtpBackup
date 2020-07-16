using FluentFTP;
using FluentFTP.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FtpBackupProject
{
    public class WorkWithFTP
    {
        public static void ConnectToFTP(Record rec)
        {
            rec.ftpClient = new FtpClient(rec.IP, rec.port, new System.Net.NetworkCredential(rec.login, rec.password));
            try
            {
                rec.ftpClient.ConnectTimeout = 5000;
                rec.ftpClient.Connect();
                //rec.ftpClient.DownloadDirectory(@"E:\website\logs\", @"/", FtpFolderSyncMode.Update);
                if (rec.ftpClient.IsConnected)
                {
                    rec.isOnline = true;
                }
                else
                {
                    rec.isOnline = false;
                }
            }
            catch
            {
                rec.isOnline = false;
            }
        }

        public static TreeNode getAllFilesAndDirs(Record rec)
        {
            folders = new Queue<FolderInfo>();
            TreeNode tree = new TreeNode(rec.name);
            folders.Enqueue(new FolderInfo("", tree, null));
            while (folders.Count > 0)
            {
                FolderInfo fi = folders.Dequeue();
                CheckDirectory(rec, fi.fullDir, fi.node);
            }
            return tree;
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
                    if(parent != null) parent.Nodes.Add(tempFolderNode);
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
