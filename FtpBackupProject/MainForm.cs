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
using System.Diagnostics;
using System.Threading;

namespace FtpBackupProject
{
    public partial class MainForm : Form
    {
        private Thread autoDownload = null;
        public MainForm()
        {
            InitializeComponent();
            SaveClass.OpenLogFile();
            SaveClass.WriteToLogFile("Программа запущена.");
            SaveClass.WriteToLogFile("Чтение settings.txt, восстановление данных.");
            SaveClass.LoadAll();
            SaveClass.WriteToLogFile("Обновление списка контроллеров.");
            UpdateList();
            SaveClass.WriteToLogFile("Запуск потока для автоматического создания бэкапов.");
            autoDownload = new Thread(WorkWithFTP.AutoDownload);
            autoDownload.Start();
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
            SaveClass.WriteToLogFile("Закрытие программы. Сохранение данных. Остановка второго потока.");
            SaveClass.SaveAll();
            SaveClass.CloseLogFile();
            autoDownload.Abort();
        }

        private int oldChoice = -2;

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem != null && oldChoice != listBox1.SelectedIndex)
            {
                oldChoice = listBox1.SelectedIndex;
                panel1.Visible = true;
                button2.Enabled = true;
                rec = Record.findRecordForName(listBox1.SelectedItem.ToString());
                UpdatePathUIList(rec);
                textBoxHours.Text = rec.periodH.ToString();
                textBoxMinuts.Text = rec.periodM.ToString();
                textBoxSeconds.Text = rec.periodS.ToString();
                labelPath.Text = rec.folderPath;
                labelStatus.Text = "";
                label4.Text = "Следующее сохранение в " + rec.nextSaveDateTime.ToString();
                label5.Text = rec.login + "@" + rec.IP + ":" + rec.port.ToString();
            }
        }

        public void UpdatePathUIList(Record rec)
        {
            listBox2.Items.Clear();
            foreach (FileAndDirInfo f in rec.filesAndDirs)
            {
                listBox2.Items.Add(f.pathUI);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveClass.WriteToLogFile("Удаление контроллера " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
            GlobalVars.records.Remove(rec);
            panel1.Visible = false;
            button2.Enabled = false;
            UpdateList();
            oldChoice = -2;
            listBox1.ClearSelected();
            SaveClass.SaveAll();
        }

        private Record rec;
        private void button3_Click(object sender, EventArgs e)
        {
            SetStatus("Выбор новой папки для сохранения копий", Color.Blue);
            folderBrowserDialog1.ShowDialog();
            string folderpath = folderBrowserDialog1.SelectedPath;
            if (folderpath.Equals(""))
            {
                SetStatus("Выбор новой папки отменён", Color.Red);
                return;
            }
            labelPath.Text = folderBrowserDialog1.SelectedPath;
            if (folderpath.ToCharArray()[folderpath.Length - 1] == '\\')
            {
                SaveClass.WriteToLogFile("Обновление места хранения копий. Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString() + "; Старая директория: " + rec.folderPath + "; Новая директория: " + folderpath);
                rec.folderPath = folderpath;
            }
            else
            {
                SaveClass.WriteToLogFile("Обновление места хранения копий. Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString() + "; Старая директория: " + rec.folderPath + "; Новая директория: " + folderpath + "\\");
                rec.folderPath = folderpath + "\\";
            }
            SaveClass.SaveAll();
            SetStatus("Новое местоположение для копий сохранено", Color.DarkGreen);
        }

        public void SetStatus(string message, Color color)
        {
            labelStatus.Text = "Статус: " + message + ".";
            labelStatus.ForeColor = color;
        }

        private void button4_Click(object sender, EventArgs e)
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
            }
            catch (Exception exc)
            {
                if (exc.Message == "lowtozero")
                {
                    SetStatus("Период имеет отрицательное значение. Введите корректный период", Color.Red);
                }
                else if (exc.Message == "alleqzero")
                {
                    SetStatus("Период не может быть нулевым", Color.Red);
                }
                else
                {
                    SetStatus("Некорректные введённые данные", Color.Red);
                }
                return;
            }
            DateTime dt = DateTime.Now;
            dt = dt.AddHours(hours);
            dt = dt.AddMinutes(minuts);
            dt = dt.AddSeconds(seconds);
            SaveClass.WriteToLogFile("Изменение периода сохранения. Контроллер: " + rec.name + ": " + rec.login + "@" + rec.IP + ":" + rec.port.ToString());
            SaveClass.WriteToLogFile("Старый период: " + rec.periodH.ToString() + "Ч; " + rec.periodM.ToString() + "М; " + rec.periodS.ToString() + "С;");
            rec.nextSaveDateTime = dt;
            rec.periodH = hours;
            rec.periodM = minuts;
            rec.periodS = seconds;
            SaveClass.WriteToLogFile("Новый период: " + rec.periodH.ToString() + "Ч; " + rec.periodM.ToString() + "М; " + rec.periodS.ToString() + "С;");
            SetStatus("Период сохранения успешно изменён", Color.DarkGreen);
            SaveClass.SaveAll();
            label4.Text = "Следующее сохранение в " + rec.nextSaveDateTime.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BlockButtons(true);
            SetStatus("Соединение..", Color.Blue);
            WorkWithFTP.DownloadWithMainForm(rec, this, false);
        }

        public void BlockButtons(bool isBlocked)
        {
            button3.Enabled = !isBlocked;
            button4.Enabled = !isBlocked;
            button5.Enabled = !isBlocked;
            button6.Enabled = !isBlocked;
            button7.Enabled = !isBlocked;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(rec.folderPath + rec.name);
            }
            catch
            {
                MessageBox.Show("Папка с копиями ещё не создана т.к. вы ещё не делали копирование в эту директорию.", "Ошибка!");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            BlockButtons(true);
            SetStatus("Подключение..", Color.Blue);
            WorkWithFTP.ConnectToFtpAsync(rec, this);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(rec != null) label4.Text = "Следующее сохранение в " + rec.nextSaveDateTime.ToString();
        }
    }
}
