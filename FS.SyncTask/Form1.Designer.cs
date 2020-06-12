namespace FS.SyncTask
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dgv_task = new System.Windows.Forms.DataGridView();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.lb = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.text = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.freq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastExecuteTimeStr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.run = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.interval = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tasktype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_task)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_task
            // 
            this.dgv_task.AllowUserToOrderColumns = true;
            this.dgv_task.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_task.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.text,
            this.freq,
            this.LastExecuteTimeStr,
            this.run,
            this.interval,
            this.tasktype});
            this.dgv_task.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_task.Location = new System.Drawing.Point(0, 0);
            this.dgv_task.Name = "dgv_task";
            this.dgv_task.RowHeadersWidth = 51;
            this.dgv_task.RowTemplate.Height = 23;
            this.dgv_task.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_task.Size = new System.Drawing.Size(1571, 622);
            this.dgv_task.TabIndex = 15;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(213, 380);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(126, 47);
            this.button2.TabIndex = 18;
            this.button2.Text = "清理任务";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(62, 380);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 47);
            this.button1.TabIndex = 17;
            this.button1.Text = "更新任务";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // lb
            // 
            this.lb.FormattingEnabled = true;
            this.lb.ItemHeight = 12;
            this.lb.Location = new System.Drawing.Point(465, 0);
            this.lb.Margin = new System.Windows.Forms.Padding(2);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(427, 364);
            this.lb.TabIndex = 16;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(1142, 0);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(429, 628);
            this.listBox1.TabIndex = 20;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(285, 380);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(126, 47);
            this.button3.TabIndex = 22;
            this.button3.Text = "清理任务";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(134, 380);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(126, 47);
            this.button4.TabIndex = 21;
            this.button4.Text = "更新任务";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // id
            // 
            this.id.DataPropertyName = "Id";
            this.id.HeaderText = "编号";
            this.id.MinimumWidth = 6;
            this.id.Name = "id";
            this.id.Width = 40;
            // 
            // text
            // 
            this.text.DataPropertyName = "Name";
            this.text.HeaderText = "任务名称";
            this.text.MinimumWidth = 6;
            this.text.Name = "text";
            this.text.Width = 140;
            // 
            // freq
            // 
            this.freq.DataPropertyName = "FrequencyType";
            this.freq.HeaderText = "周期";
            this.freq.MinimumWidth = 6;
            this.freq.Name = "freq";
            this.freq.Width = 80;
            // 
            // LastExecuteTimeStr
            // 
            this.LastExecuteTimeStr.DataPropertyName = "LastExecuteTimeStr";
            this.LastExecuteTimeStr.HeaderText = "最近执行时间";
            this.LastExecuteTimeStr.MinimumWidth = 6;
            this.LastExecuteTimeStr.Name = "LastExecuteTimeStr";
            this.LastExecuteTimeStr.Width = 120;
            // 
            // run
            // 
            this.run.DataPropertyName = "IsActivated";
            this.run.HeaderText = "执行";
            this.run.MinimumWidth = 6;
            this.run.Name = "run";
            this.run.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.run.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.run.Width = 40;
            // 
            // interval
            // 
            this.interval.DataPropertyName = "Interval";
            this.interval.HeaderText = "间隔";
            this.interval.MinimumWidth = 6;
            this.interval.Name = "interval";
            this.interval.Width = 40;
            // 
            // tasktype
            // 
            this.tasktype.DataPropertyName = "TaskType";
            this.tasktype.HeaderText = "任务类型";
            this.tasktype.MinimumWidth = 6;
            this.tasktype.Name = "tasktype";
            this.tasktype.Width = 80;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1571, 622);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.dgv_task);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lb);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_task)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lb;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn text;
        private System.Windows.Forms.DataGridViewTextBoxColumn freq;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastExecuteTimeStr;
        private System.Windows.Forms.DataGridViewCheckBoxColumn run;
        private System.Windows.Forms.DataGridViewTextBoxColumn interval;
        private System.Windows.Forms.DataGridViewTextBoxColumn tasktype;
        private System.Windows.Forms.DataGridView dgv_task;
    }
}

