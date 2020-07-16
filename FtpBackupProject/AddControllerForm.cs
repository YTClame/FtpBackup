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
        public AddControllerForm()
        {
            InitializeComponent();
            SetStatus("Ожидание попытки подключения");
        }

        private void SetStatus(string status)
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

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            Record rec = new Record(textBoxIP.Text, Convert.ToInt32(textBoxPort.Text), textBoxLogin.Text, textBoxPassword.Text, textBoxName.Text);
            SetStatus("Попытка подключения к " + rec.IP + ":" + rec.port.ToString());
            SetInputsEnabled(false);
            WorkWithFTP.ConnectToFTP(rec);
            if (rec.isOnline)
            {
                SetStatus("Подключено успешно, сканирование файлов...");
                TreeNode tree = WorkWithFTP.getAllFilesAndDirs(rec);
                treeView1.Nodes.Add(tree);
                SetStatus("Ожидание выбора файлов и папок для создания резервных копий");
                panelConn.Visible = false;
                panelSet.Visible = true;
            }
            else
            {
                SetStatus("Ошибка подключения");
                SetInputsEnabled(true);
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            Stack<string> path = new Stack<string>();
            TreeNode tn = e.Node;
            while (tn != null)
            {
                path.Push(tn.Text);
                tn = tn.Parent;
            }
            string pathString = "";
            foreach (string s in path)
            {
                pathString += s + "/";
            }
            if (e.Node.ImageIndex == 0) pathString += "*";
            if (e.Node.ImageIndex == 1) pathString = pathString.Remove(pathString.Length - 1);
            listBox1.Items.Add(pathString);
        }
    }
}
