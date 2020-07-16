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
            StreamWriter sw = new StreamWriter("temp.txt");
            sw.Write(JsonConvert.SerializeObject(a));
            sw.Close();

            StreamReader sr = new StreamReader("temp.txt");
            A a2 = JsonConvert.DeserializeObject<A>(sr.ReadToEnd());
            **/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new AddControllerForm().ShowDialog();
        }
    }
}
