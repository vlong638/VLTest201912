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
            Scheduler.DoLogEvent += SetText;
            Scheduler.UpdateConfigEvent += SetDataGrid;
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
                if (listBox1.Items.Count > 50)
                {
                    listBox1.Items.Clear();
                }
                listBox1.Items.Add(text);
            }
        }


        delegate void SetDataGridCallBack(List<TaskConfig> configs);
        private void SetDataGrid(List<TaskConfig> configs)
        {
            if (this.dgv_task.InvokeRequired)
            {
                SetDataGridCallBack stcb = new SetDataGridCallBack(SetDataGrid);
                this.Invoke(stcb, new object[] { configs });
            }
            else
            {
                dgv_task.DataSource = VLScheduler.TaskConfigs;
                dgv_task.Refresh();
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
