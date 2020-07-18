using FluentFTP;
using FluentFTP.Proxy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
                acf.ConnectionResult(rec.ftpClient.IsConnected);
            }
            catch
            {
                acf.ConnectionResult(false);
            }
        }

        public static void AutoDownload()
        {
            while (true)
            {
                Thread.Sleep(700);
                foreach (Record rec in GlobalVars.records)
                {
                    if (rec.nextSaveDateTime < DateTime.Now)
                    {
                        DateTime dt = DateTime.Now;
                        dt = dt.AddHours(rec.periodH);
                        dt = dt.AddMinutes(rec.periodM);
                        dt = dt.AddSeconds(rec.periodS);
                        rec.nextSaveDateTime = dt;
                        DownloadWithMainForm(rec, null, true);
                    }
                }
            }
        }

        public static async void DownloadWithMainForm(Record rec, MainForm mf, bool isBackgroundDownload)
        {
            bool isConnected = true;
            await Task.Run(() =>
            {
                try
                {
                    ConnectToFTP(rec);
                }
                catch
                {
                    isConnected = false;
                }
            });
            if (isConnected)
            {
                if (!isBackgroundDownload)
                {
                    mf.SetStatus("Соединение установлено, скачивание..", Color.DarkGreen);
                }
                await Task.Run(() =>
                {
                    DownloadLocal(rec);
                });
                if (!isBackgroundDownload)
                {
                    mf.SetStatus("Загрузка завершена", Color.DarkGreen);
                    mf.BlockButtons(false);
                }
            }
            else
            {
                if (!isBackgroundDownload)
                {
                    mf.SetStatus("Ошибка соединения", Color.Red);
                    mf.BlockButtons(false);
                }
            }
        }

        public static void ConnectToFTP(Record rec)
        {
            rec.ftpClient = new FtpClient(rec.IP, rec.port, new System.Net.NetworkCredential(rec.login, rec.password));
            rec.ftpClient.Connect();
        }

        public static async void ConnectToFtpAsync(Record rec, MainForm mf)
        {
            rec.ftpClient = new FtpClient(rec.IP, rec.port, new System.Net.NetworkCredential(rec.login, rec.password));
            await Task.Run(() =>
            {
                try
                {
                    rec.ftpClient.Connect();
                }
                catch
                {

                }
            });
            if (rec.ftpClient.IsConnected)
            {
                mf.SetStatus("Подключено, сканирование директорий и файлов..", Color.Blue);
                folders = new Queue<FolderInfo>();
                TreeNode tree = new TreeNode(rec.name, 0, 0);
                TreeNode treeNF = new TreeNode("Не найдены на FTP", 2, 2);
                treeNF.Expand();
                folders.Enqueue(new FolderInfo("", tree, null));
                await Task.Run(() =>
                {
                    while (folders.Count > 0)
                    {
                        FolderInfo fi = folders.Dequeue();
                        CheckDirectory(rec, fi.fullDir, fi.node);
                    }
                });
                mf.SetStatus("Сканирование сервера завершено, синхронизация..", Color.Blue);
                await Task.Run(() =>
                {
                    AddExistingFilesAndDirs(rec, tree, treeNF);
                });
                mf.SetStatus("Ожидание нового списка директорий и файлов..", Color.Blue);
                EditDirs ed = new EditDirs(tree, mf, treeNF, rec);
                ed.ShowDialog();
                mf.UpdatePathUIList(rec);
            }
            else
            {
                mf.SetStatus("Ошибка соединения", Color.Red);
            }
        }

        public static void AddExistingFilesAndDirs(Record rec, TreeNode tree, TreeNode treeNF)
        {
            string tempPath;
            TreeNode tempNode;
            foreach(FileAndDirInfo f in rec.filesAndDirs)
            {
                tempPath = "/";
                tempNode = tree;
                bool isExist = true;
                foreach(string tpath in f.pathParts)
                {
                    tempPath += tpath + "/";
                    try
                    {
                        tempNode = getChildNodeForText(tempNode, tpath);
                        if (tempNode == null) throw new Exception();
                    }
                    catch
                    {
                        isExist = false;
                        break;
                    }
                }
                if (isExist)
                {
                    tempNode.Checked = true;
                }
                else
                {
                    TreeNode temp = new TreeNode(f.pathUI, 2, 2);
                    temp.Checked = true;
                    treeNF.Nodes.Add(temp);
                }
            }
        }

        private static TreeNode getChildNodeForText(TreeNode tree, string text)
        {
            foreach(TreeNode tn in tree.Nodes)
            {
                if (tn.Text.Equals(text)) return tn;
            }
            return null;
        }

        public static void DownloadLocal(Record rec)
        {
            string dir = rec.folderPath + rec.name + "\\" + DateTime.Now.ToString().Replace(':', '.');
            Directory.CreateDirectory(dir);
            foreach (FileAndDirInfo fi in rec.filesAndDirs)
            {
                string pathOnFtp = "";
                string dirTemp = dir;
                foreach (string partPath in fi.pathParts)
                {
                    pathOnFtp += "/" + partPath;
                    dirTemp += "\\" + partPath;
                }
                if (pathOnFtp.Equals("")) pathOnFtp = "/";

                if (fi.isFolder)
                {
                    if(rec.ftpClient.DirectoryExists(pathOnFtp))
                        rec.ftpClient.DownloadDirectory(dirTemp, pathOnFtp, FtpFolderSyncMode.Update);
                }
                else
                {
                    if(rec.ftpClient.FileExists(pathOnFtp))
                        rec.ftpClient.DownloadFile(dirTemp, pathOnFtp);
                }

            }
            rec.ftpClient.Disconnect();
            SaveClass.SaveAll();
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
