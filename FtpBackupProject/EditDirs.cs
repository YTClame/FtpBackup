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
    public partial class EditDirs : Form
    {
        private MainForm mf;
        public EditDirs(TreeNode tree, MainForm mf)
        {
            InitializeComponent();
            this.mf = mf;
            customTreeView1.Nodes.Add(tree);
        }
    }
}
