using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FtpBackupProject
{
    public partial class LogSettings : Form
    {
        private int max;
        private int mode;
        private bool isClick;
        public LogSettings()
        {
            InitializeComponent();
            UpdateEnables();
            max = GlobalVars.maxLogSize;
            mode = GlobalVars.savingMode;
            isClick = false;
            SaveClass.WriteToLogFile("Открыто окно настройки логирования. Текущие настройки: " + getSettingsNow(GlobalVars.savingMode, GlobalVars.maxLogSize));
        }
        private string getSettingsNow(int mode, int size)
        {
            if (mode == 1) return "Лог ограничивается максимальным размером в " + size.ToString() + " МБ.";
            if (mode == 0) return "Лог не ограничивается максимальным размером файла.";
            if (mode == -1) return "Лог не пишется.";
            return null;
        }
        private void UpdateEnables()
        {
            textBox1.Text = GlobalVars.maxLogSize.ToString();
            radioButton1.Checked = false;
            textBox1.Enabled = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            if (GlobalVars.savingMode == -1) radioButton1.Checked = true;
            if (GlobalVars.savingMode == 0) radioButton2.Checked = true;
            if(GlobalVars.savingMode == 1)
            {
                radioButton3.Checked = true;
                textBox1.Enabled = true;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                GlobalVars.savingMode = 1;
                textBox1.Enabled = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                GlobalVars.savingMode = 0;
                textBox1.Enabled = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                GlobalVars.savingMode = -1;
                textBox1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            int size = 0;
            try
            {
                size = Convert.ToInt32(text);
            }
            catch
            {
                MessageBox.Show("Не удалось преобразовать число в целочисленный формат", "Ошибка");
                return;
            }
            if(size <= 0)
            {
                MessageBox.Show("Введённое число должно быть больше 0", "Ошибка");
                return;
            }
            isClick = true;
            Close();
        }

        private void LogSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isClick)
            {
                GlobalVars.maxLogSize = Convert.ToInt32(textBox1.Text);
                if (radioButton1.Checked)
                {
                    SaveClass.WriteToLogFile("Новые настройки логирования: Логирование отключено.");
                    GlobalVars.savingMode = -1;
                }
                if (radioButton2.Checked)
                {
                    SaveClass.WriteToLogFile("Новые настройки логирования: Логирование включено, ограничения по размеру файла логов нет.");
                    GlobalVars.savingMode = 0;
                }
                if (radioButton3.Checked)
                {
                    SaveClass.WriteToLogFile("Новые настройки логирования: Логирование включено, ограничения по размеру файла логов: " + GlobalVars.maxLogSize.ToString() + " МБ.");
                    GlobalVars.savingMode = 1;
                }
                SaveClass.SaveAll();
            }
            else
            {
                GlobalVars.maxLogSize = max;
                GlobalVars.savingMode = mode;
                SaveClass.WriteToLogFile("Настройки логирования не были сохранены из - за закрытия окна настроек.");
            }
        }
    }
}
