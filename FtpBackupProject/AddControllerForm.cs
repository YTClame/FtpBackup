using FluentFTP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FtpBackupProject
{
    public partial class AddControllerForm : Form
    {
        private MainForm mainform;
        public AddControllerForm(MainForm mf)
        {
            InitializeComponent();
            SetStatus("Ожидание попытки подключения");
            mainform = mf;
        }

        public void SetStatus(string status)
        {
            statusLabel.Text = "Статус: " + status + ".";
        }

        private void SetInputsEnabled(bool isEnabled)
        {
            textBoxIP.Enabled = isEnabled;
            textBoxPort.Enabled = isEnabled;
            textBoxLogin.Enabled = isEnabled;
            textBoxPassword.Enabled = isEnabled;
            textBoxName.Enabled = isEnabled;
        }

        private Record rec;
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            name.Trim(new char[] { ' ' });
            if (name.Contains('\\') || name.Contains('/') || name.Contains(':') || name.Contains('*') || name.Contains('?') || name.Contains('\"') || name.Contains('\'') || name.Contains('<') || name.Contains('>') || name.Contains('+') || name.Contains('|') || name.Contains('%') || name.Contains('!') || name.Contains('@'))
            {
                SetStatus("Имя не должно содержать символы: \\ / : * ? \" \' < > + | % ! #");
                return;
            }
            foreach (Record r in GlobalVars.records)
            {
                if (r.name.Equals(name))
                {
                    SetStatus("Такое имя уже существует в вашей базе! Введите уникальное");
                    return;
                }
            }
            rec = new Record(textBoxIP.Text, Convert.ToInt32(textBoxPort.Text), textBoxLogin.Text, textBoxPassword.Text, name);
            SetStatus("Попытка подключения к " + name + "..");
            SetInputsEnabled(false);
            WorkWithFTP.ConnectToFTP(rec, this);
        }
        public void ConnectionResult(bool Res)
        {
            if (Res)
            {
                SetStatus("Подключено успешно, сканирование файлов..");
                WorkWithFTP.getAllFilesAndDirs(rec, this);
            }
            else
            {
                SetStatus("Ошибка подключения или авторизации");
                SetInputsEnabled(true);
            }
        }

        public void SetTreeNode(TreeNode tn)
        {
            TreeNode tree = tn;
            treeView1.Nodes.Add(tree);
            SetStatus("Ожидание выбора файлов и папок для создания резервных копий");
            panelConn.Visible = false;
            panelSet.Visible = true;
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            Stack<string> path = new Stack<string>();
            TreeNode tn = e.Node;
            while (tn.Parent != null)
            {
                path.Push(tn.Text);
                tn = tn.Parent;
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            labelFolderPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int hours;
            int minuts;
            int seconds;
            try
            {
                hours = Convert.ToInt32(textBoxHours.Text);
                minuts = Convert.ToInt32(textBoxMinuts.Text);
                seconds = Convert.ToInt32(textBoxSeconds.Text);
                if (hours < 0 || minuts < 0 || seconds < 0) throw new Exception("lowtozero");
                if (hours == 0 && minuts == 0 && seconds == 0) throw new Exception("alleqzero");
                if (folderBrowserDialog1.SelectedPath == "") throw new Exception("folder");
            }
            catch(Exception exc)
            {
                if(exc.Message == "folder")
                {
                    SetStatus("Выберите папку для сохранения копий");
                }
                else if(exc.Message == "lowtozero")
                {
                    SetStatus("Период имеет отрицательное значение. Введите корректный период");
                }
                else if(exc.Message == "alleqzero")
                {
                    SetStatus("Период не может быть нулевым");
                }
                else
                {
                    SetStatus("Некорректные введённые данные");
                }
                return;
            }
            button2.Enabled = false;
            button1.Enabled = false;
            treeView1.Enabled = false;
            textBoxHours.Enabled = false;
            textBoxMinuts.Enabled = false;
            textBoxSeconds.Enabled = false;
            DateTime dt = DateTime.Now;
            dt = dt.AddHours(hours);
            dt = dt.AddMinutes(minuts);
            dt = dt.AddSeconds(seconds);
            rec.nextSaveDateTime = dt;
            string folderpath = folderBrowserDialog1.SelectedPath;
            if (folderpath.ToCharArray()[folderpath.Length-1] == '\\')
            {
                rec.folderPath = folderpath;
            }
            else
            {
                rec.folderPath = folderpath + "\\";
            }
            rec.periodH = hours;
            rec.periodM = minuts;
            rec.periodS = seconds;
            GlobalVars.records.Add(rec);
            mainform.UpdateList();
            if (checkBox1.Checked)
            {
                WorkWithFTP.Download(rec, this);
                SetStatus("Создание копии..");
            }
            else
            {
                rec.ftpClient.Disconnect();
                SaveClass.SaveAll();
                Close();
            }
        }
    }
}
