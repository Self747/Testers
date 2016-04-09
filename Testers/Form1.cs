﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Testers
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Log testersLogs = new Log();

        private int testersCount;

        private string[] names = new string[] { "Jhon", "Alex", "Bob", "Carl", "Monika", "Philip", "Andrew", "George", "Josefina", "Danielle", "Tommy", "Arnold", "Carolyn", "Herman", "Raymond", "Ricky", "Allan", "Kerry", "Miguel", "Ken" };


        private void Form1_Load(object sender, EventArgs e)
        {
            Log.logSource = listBox1;
            Log.mainControlForm = this;

            testersCountTrackBar.Maximum = names.Length;
            testersCount = testersCountTrackBar.Value;

            Tester.OnTextStatusUpdate += TesterTextStatusUpdateHandler;
            ProgPool.OnProgPoolSizeUpdate += ProgPoolSizeUpdateHandler;
        }

        private void TesterTextStatusUpdateHandler(object t, string msg, string prefix)
        {
            try
            {
                var tester = (Tester)t;
                testersLogs.push($"{DateTime.Now.ToLongTimeString()} {prefix} {tester.name} : {msg}", true);
                testersInfoGrid.Rows[tester.index].Cells[1].Value = msg;
            }
            catch (Exception)
            {
            }
        }

        private void ProgPoolSizeUpdateHandler(int size)
        {
            //Log.show(size);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            testersLogs.Save();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            testersLogs = Log.Open();
        }

        private void testersCountTrackBar_ValueChanged(object sender, EventArgs e)
        {
            testersCountLabel.Text = $"Кількість тестерів: {testersCountTrackBar.Value}";
            testersCount = testersCountTrackBar.Value;
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < testersCount; i++)
            {
                string name = names[i];
                int temp = i;
                testersInfoGrid.Rows.Add(new string[] { name, " " });
                Thread t = new Thread(() =>
                {
                    new Tester(name, temp);
                });
                t.Start();
            }
        }
    }
}
