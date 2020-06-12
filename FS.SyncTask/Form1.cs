using FrameworkTest.Business.TaskScheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FS.SyncTask
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }


        VLScheduler Scheduler
            ;
        private void Form1_Load(object sender, EventArgs e)
        {
            Scheduler = new VLScheduler();
            Scheduler.LogInfo += SetText;
            Scheduler.Start();
            dgv_task.DataSource = VLScheduler.TaskConfigs;

        }

        delegate void SetTextCallBack(string text);
        private void SetText(string text)
        {
            if (this.listBox1.InvokeRequired)
            {
                SetTextCallBack stcb = new SetTextCallBack(SetText);
                this.Invoke(stcb, new object[] { text });
            }
            else
            {
                listBox1.Items.Add(text);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
