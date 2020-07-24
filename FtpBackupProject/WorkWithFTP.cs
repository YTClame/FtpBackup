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
using System.Web.Management;
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
            SaveClass.WriteToLogFile("ConnectToFTP(...): " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString() + ": [" + rec.ftpClient.IsConnected.ToString() + "]");
        }

        public static void AutoDownload()
        {
            while (true)
            {
                Thread.Sleep(700);
                if (GlobalVars.records == null)
                {
                    continue;
                }
                foreach (Record rec in GlobalVars.records)
                {
                    if (rec != null && rec.nextSaveDateTime < DateTime.Now && rec.filesAndDirs.Count > 0)
                    {
                        DateTime dt = DateTime.Now;
                        SaveClass.WriteToLogFile("АВТОМАТИЧЕСКАЯ КОПИЯ! Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
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
            string exc = "Ошибка";
            await Task.Run(() =>
            {
                try
                {
                    ConnectToFTP(rec);
                }
                catch(System.Net.Sockets.SocketException)
                {
                    isConnected = false;
                    exc = "FTP сервер не отвечает";
                    SaveClass.WriteToLogFile("DownloadWithMainForm(...) System.Net.Sockets.SocketException ; Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
                }
                catch (FluentFTP.FtpAuthenticationException)
                {
                    isConnected = false;
                    exc = "Ошибка авторизации";
                    SaveClass.WriteToLogFile("DownloadWithMainForm(...) FluentFTP.FtpAuthenticationException ; Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
                }
                catch
                {
                    isConnected = false;
                    exc = "Неожиданная ошибка";
                    SaveClass.WriteToLogFile("DownloadWithMainForm(...) Неизвестная ошибка подключения к серверу ; Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
                }
            });
            if (isConnected)
            {
                SaveClass.WriteToLogFile("Соединение установлено успешно, скачивание начинается. Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
                if (!isBackgroundDownload)
                {
                    mf.SetStatus("Соединение установлено, скачивание..", Color.DarkGreen);
                }
                bool isGood = true;
                await Task.Run(() =>
                {
                    if (!DownloadLocal(rec)) isGood = false; ;
                });
                rec.ftpClient.Disconnect();
                if (!isGood && !isBackgroundDownload)
                {
                    SaveClass.WriteToLogFile("Загрузка завершена, но не все файлы были найдены на сервере.");
                    mf.SetStatus("Загрузка завершена, но не все файлы были найдены на сервере", Color.Red);
                    mf.BlockButtons(false);
                }
                if (!isBackgroundDownload && isGood)
                {
                    SaveClass.WriteToLogFile("Загрузка завершена без проблем. Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
                    mf.SetStatus("Загрузка завершена", Color.DarkGreen);
                    mf.BlockButtons(false);
                }
                if(isBackgroundDownload && isGood)
                {
                    SaveClass.WriteToLogFile("Загрузка завершена без проблем. Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
                }
                if(isBackgroundDownload && !isGood)
                {
                    SaveClass.WriteToLogFile("Загрузка завершена, но не все файлы были найдены на сервере.");
                }
            }
            else
            {
                if (!isBackgroundDownload)
                {
                    mf.SetStatus(exc, Color.Red);
                    mf.BlockButtons(false);
                }
                rec.ftpClient.Disconnect();
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
            string exc = "Ошибка при подключении";
            bool isAllGood = true;
            await Task.Run(() =>
            {
                try
                {
                    rec.ftpClient.Connect();
                    SaveClass.WriteToLogFile("Попытка подключения. Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
                }
                catch(System.Net.Sockets.SocketException)
                {
                    exc = "FTP сервер не отвечает";
                    isAllGood = false;
                    SaveClass.WriteToLogFile("ConnectToFtpAsync(...): System.Net.Sockets.SocketException. ; Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
                }
                catch(FluentFTP.FtpAuthenticationException)
                {
                    exc = "Ошибка авторизации";
                    isAllGood = false;
                    SaveClass.WriteToLogFile("ConnectToFtpAsync(...): FluentFTP.FtpAuthenticationException. ; Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
                }
                catch
                {
                    exc = "Неожиданная ошибка";
                    isAllGood = false;
                    SaveClass.WriteToLogFile("ConnectToFtpAsync(...): Неизвестная ошибка. ; Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
                }
            });
            if (rec.ftpClient.IsConnected && isAllGood)
            {
                mf.SetStatus("Подключено, сканирование директорий и файлов..", Color.Blue);
                folders = new Queue<FolderInfo>();
                TreeNode tree = new TreeNode(rec.name, 0, 0);
                TreeNode treeNF = new TreeNode("Не найдены на FTP", 2, 2);
                treeNF.Expand();
                folders.Enqueue(new FolderInfo("", tree, null));
                bool isGoodChecking = true;
                SaveClass.WriteToLogFile("Сканирование директорий и файлов. Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
                await Task.Run(() =>
                {
                    while (folders.Count > 0)
                    {
                        FolderInfo fi = folders.Dequeue();
                        if(!CheckDirectory(rec, fi.fullDir, fi.node)) isGoodChecking = false;
                    }
                });
                if (!isGoodChecking)
                {
                    rec.ftpClient.Disconnect();
                    mf.SetStatus("Ошибка при сканировании файлов", Color.Red);
                    SaveClass.WriteToLogFile("Ошибка при сканировании файлов.");
                    mf.BlockButtons(false);
                    return;
                }
                SaveClass.WriteToLogFile("Сканирование завершено. Синхронизация...");
                mf.SetStatus("Сканирование сервера завершено, синхронизация..", Color.Blue);
                await Task.Run(() =>
                {
                    AddExistingFilesAndDirs(rec, tree, treeNF);
                });
                SaveClass.WriteToLogFile("Синхронизация завершена.");
                mf.SetStatus("Ожидание нового списка директорий и файлов..", Color.Blue);
                rec.ftpClient.Disconnect();
                EditDirs ed = new EditDirs(tree, mf, treeNF, rec);
                ed.ShowDialog();
                mf.UpdatePathUIList(rec);
            }
            else
            {
                rec.ftpClient.Disconnect();
                mf.SetStatus(exc, Color.Red);
                mf.BlockButtons(false);
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

        public static bool DownloadLocal(Record rec)
        {
            string pathOnFtp = "";
            DateTime now = DateTime.Now;
            try
            {
                long size = 0;
                string dir = rec.folderPath + rec.name + "\\" + now.ToString().Replace(':', '.');
                Directory.CreateDirectory(dir);
                foreach (FileAndDirInfo fi in rec.filesAndDirs)
                {
                    pathOnFtp = "";
                    string dirTemp = dir;
                    foreach (string partPath in fi.pathParts)
                    {
                        pathOnFtp += "/" + partPath;
                        dirTemp += "\\" + partPath;
                    }
                    if (pathOnFtp.Equals("")) pathOnFtp = "/";

                    if (fi.isFolder)
                    {
                        try
                        {
                            if (rec.ftpClient.DirectoryExists(pathOnFtp))
                            {
                                List<FtpResult> ftpResults = rec.ftpClient.DownloadDirectory(dirTemp, pathOnFtp, FtpFolderSyncMode.Update);
                                foreach (FtpResult ftpRes in ftpResults)
                                {
                                    size += ftpRes.Size;
                                }
                            }
                            else
                            {
                                SaveClass.WriteToLogFile("Директория не найдена: " + pathOnFtp + " Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
                            }
                        }
                        catch
                        {
                            SaveClass.WriteToLogFile("Ошибка скачивания из - за перебоя с интернет соединением. Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
                        }
                            
                    }
                    else
                    {
                        try
                        {
                            if (rec.ftpClient.FileExists(pathOnFtp))
                            {
                                rec.ftpClient.DownloadFile(dirTemp, pathOnFtp);
                                size += rec.ftpClient.GetFileSize(pathOnFtp);
                            }
                            else
                            {
                                SaveClass.WriteToLogFile("Файл не найден: " + pathOnFtp + " Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
                            }
                        }
                        catch
                        {
                            SaveClass.WriteToLogFile("Ошибка скачивания из - за перебоя с интернет соединением. Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
                        }
                    }
                }
                rec.ftpClient.Disconnect();
                //Размеры и перезапись господи спаси:
                try
                {
                    string direct = "";
                    try
                    {
                        direct = Environment.CurrentDirectory + "\\sizesInfo\\" + rec.name + "\\";
                        if (!Directory.Exists(direct)) Directory.CreateDirectory(direct);
                    }
                    catch
                    {

                    }
                    try
                    {
                        if (!Directory.Exists(direct + now.ToString().Replace(':', '.'))) Directory.CreateDirectory(direct + now.ToString().Replace(':', '.'));
                        StreamWriter sw1 = new StreamWriter(direct + now.ToString().Replace(':', '.') + "\\size");
                        sw1.WriteLine(size.ToString());
                        sw1.Close();
                    }
                    catch
                    {
                        SaveClass.WriteToLogFile("Ошибка определения размера файлов бэкапа или же записи их в файл.");
                    }
                    StreamReader sr;
                    try
                    {
                        sr = new StreamReader(direct + "\\size");
                    }
                    catch
                    {
                        StreamWriter sw2 = new StreamWriter(direct + "\\size");
                        sw2.Write("0");
                        sw2.Close();
                        sr = new StreamReader(direct + "\\size");
                    }
                    long tempSize = Convert.ToInt64(sr.ReadToEnd());
                    sr.Close();
                    tempSize += size;
                    StreamWriter sw3 = new StreamWriter(direct + "\\size");
                    sw3.Write(tempSize.ToString());
                    sw3.Close();
                    //Проверка размерности и удаление папок:
                    
                    while (tempSize >= rec.maxFilesSize)
                    {
                        string[] allfolders = Directory.GetDirectories(rec.folderPath + rec.name + "\\");
                        string[] allfoldersSIZES = Directory.GetDirectories(direct);
                        if (allfoldersSIZES.Length > 0)
                        {
                            StreamReader sr2 = new StreamReader(allfoldersSIZES[0] + "\\size");
                            long tempS = Convert.ToInt64(sr2.ReadToEnd());
                            sr2.Close();
                            Directory.Delete(allfolders[0], true);
                            Directory.Delete(allfoldersSIZES[0], true);
                            SaveClass.WriteToLogFile("Удалена папка: " + allfolders[0] + " из-за превышения допустимого размера папки бэкапов контроллера. (" + ((tempSize/1024)/1024).ToString() + " -> " + (((tempSize - tempS) / 1024) / 1024).ToString() + ") / " + ((rec.maxFilesSize/1024)/1024).ToString() + " (МБ).");
                            StreamWriter sw4 = new StreamWriter(direct + "size");
                            tempSize -= tempS;
                            sw4.Write(tempSize.ToString());
                            sw4.Close();
                            allfolders = Directory.GetDirectories(rec.folderPath + rec.name + "\\");
                            allfoldersSIZES = Directory.GetDirectories(direct);
                        }
                    }
                    
                }
                catch
                {
                    SaveClass.WriteToLogFile("Ошибка доступа к файлам size.");
                }
                return true;
            }
            catch
            {
                SaveClass.WriteToLogFile("Ошибка скачивания: " + pathOnFtp + " Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
                rec.ftpClient.Disconnect();
                return false;
            }
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

        private static bool CheckDirectory(Record rec, string path, TreeNode parent)
        {
            try
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
            catch
            {
                return false;
            }
            return true;
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
