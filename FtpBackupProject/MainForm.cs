using FluentFTP;
using Newtonsoft.Json;
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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SaveClass.LoadAll();
            UpdateList();
        }

        public void UpdateList()
        {
            listBox1.Items.Clear();
            foreach(Record rec in GlobalVars.records)
            {
                listBox1.Items.Add(rec.name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new AddControllerForm(this).ShowDialog();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveClass.SaveAll();
        }
    }
}
