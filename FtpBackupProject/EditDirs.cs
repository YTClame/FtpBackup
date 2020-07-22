using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FtpBackupProject
{
    public partial class EditDirs : Form
    {
        private MainForm mf;
        private Record rec;
        private string name;
        public EditDirs(TreeNode tree, MainForm mf, TreeNode treeNF, Record rec)
        {
            InitializeComponent();
            SaveClass.WriteToLogFile("Открыто окно редактирования директорий контроллера [EditDirs]");
            SaveClass.WriteToLogFile("Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
            this.mf = mf;
            this.rec = rec;
            customTreeView1.Nodes.Add(tree);
            customTreeView1.Nodes.Add(treeNF);
            name = rec.name;
            SaveClass.SaveTempRec(rec);
            isButtonClose = false;
            SaveClass.WriteToLogFile("Текущие директории:");
            foreach (FileAndDirInfo f in rec.filesAndDirs)
            {
                listBox1.Items.Add(f.pathUI);
                SaveClass.WriteToLogFile(f.pathUI);
            }
        }

        private void customTreeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            Stack<string> path = new Stack<string>();
            TreeNode tn = e.Node;
            while (tn.Parent != null)
            {
                path.Push(tn.Text);
                tn = tn.Parent;
            }
            if(!e.Node.Text.Equals("Не найдены на FTP"))
            {
                if (!tn.Text.Equals("Не найдены на FTP"))
                {
                    string pathString = "./";
                    List<string> partsPath = new List<string>();
                    foreach (string s in path)
                    {
                        pathString += s + "/";
                        partsPath.Add(s);
                    }
                    if (e.Node.Checked)
                    {
                        if (e.Node.ImageIndex == 0)
                        {
                            pathString += "*";
                            rec.filesAndDirs.Add(new FileAndDirInfo(partsPath, true, pathString));
                        }
                        if (e.Node.ImageIndex == 1)
                        {
                            pathString = pathString.Remove(pathString.Length - 1);
                            rec.filesAndDirs.Add(new FileAndDirInfo(partsPath, false, pathString));
                        }
                        listBox1.Items.Add(pathString);
                    }
                    else
                    {
                        if (e.Node.ImageIndex == 0)
                        {
                            pathString += "*";
                        }
                        if (e.Node.ImageIndex == 1)
                        {
                            pathString = pathString.Remove(pathString.Length - 1);
                        }
                        rec.RemoveFileOrDir(pathString);
                        listBox1.Items.Remove(pathString);
                    }
                }
                else
                {
                    if (e.Node.Checked)
                    {
                        listBox1.Items.Add(e.Node.Text);
                        Record.findFileAndDirInfoForPathUI(rec, e.Node.Text).isRemoving = false;
                    }
                    else
                    {
                        listBox1.Items.Remove(e.Node.Text);
                        Record.findFileAndDirInfoForPathUI(rec, e.Node.Text).isRemoving = true;
                    }
                }
            }
        }

        private bool isButtonClose;

        private void button1_Click(object sender, EventArgs e)
        {
            isButtonClose = true;
            List<FileAndDirInfo> list = new List<FileAndDirInfo>();
            foreach(FileAndDirInfo f in rec.filesAndDirs)
            {
                if (!f.isRemoving)
                {
                    list.Add(f);
                }
            }
            rec.filesAndDirs = list;
            SaveClass.SaveAll();
            Close();
        }

        private void EditDirs_FormClosed(object sender, FormClosedEventArgs e)
        {
            mf.BlockButtons(false);
            if (isButtonClose)
            {
                mf.SetStatus("Изменения директорий и файлов сохранены", Color.DarkGreen);
                SaveClass.WriteToLogFile("Новый список директорий и файлов:");
                foreach(FileAndDirInfo fff in rec.filesAndDirs)
                {
                    SaveClass.WriteToLogFile(fff.pathUI);
                }
            }
            else
            {
                GlobalVars.records.Remove(rec);
                GlobalVars.records.Add(SaveClass.LoadTempRec(name));
                mf.SetStatus("Изменения не были сохранены", Color.Red);
                SaveClass.WriteToLogFile("Нажатие на крестик - закрытие окна без сохранения.");
            }
            File.Delete("TempRecord_" + name + ".json");
            SaveClass.SaveAll();
        }
    }
}
