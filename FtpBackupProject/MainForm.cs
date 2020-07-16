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
            /**
            List<Record> ci = new List<Record>();
            ci.Add(new Record("127.0.0.1", 25565, "Clame", "qwerty"));
            ci.Add(new Record("192.168.0.1", 25565, "Clame2", "qwerty2"));
            ci.Add(new Record("10.0.0.2", 25565, "Clame3", "qwerty3"));
            StreamWriter sw = new StreamWriter("temp.txt");
            sw.Write(JsonConvert.SerializeObject(ci));
            sw.Close();

            StreamReader sr = new StreamReader("temp.txt");
            List<Record> list = JsonConvert.DeserializeObject<List<Record>>(sr.ReadToEnd());
            **/
            int a = 3;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new AddControllerForm().ShowDialog();
        }
    }
}
