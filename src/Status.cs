using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SWRPre
{
    public partial class Status : Form
    {
        public Status()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (this.progressBar1.Value < 100)
            {
                Environment.Exit(0);       
            }
            else
            {
                this.Close();
            }
        }
        public void writeTask(string task)
        {    
            this.listBoxTask.Items.Add(task);
            //this.listBoxTask.Items.Insert(0, task);
            //string lastMember = (string)this.listBoxTask.Items[this.listBoxTask.Items.Count - 1];
            //this.textBox1.Text = lastMember;
            //this.listBoxTask.DisplayMember = lastMember;
            this.listBoxTask.SelectedIndex = this.listBoxTask.Items.Count - 1;
            
    
            //this.Update();
            Application.DoEvents();
        }
        public void updateProgress(int complete)
        {
            this.progressBar1.Value = complete;
            if (complete >= 100)
            {
                buttonCancel.Text = "OK";
            }
            //this.Update();
            Application.DoEvents();
        }

        private void listBoxTask_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonTask2Clipboard_Click(object sender, EventArgs e)
        {
            string str = "";
            for (int i = 0; i < this.listBoxTask.Items.Count; i++)
            {
                str += this.listBoxTask.Items[i].ToString() + '\n';
            }
            Clipboard.SetData(System.Windows.Forms.DataFormats.Text, str);

        }

        private void Status_Load(object sender, EventArgs e)
        {

        }
        

    }
}
