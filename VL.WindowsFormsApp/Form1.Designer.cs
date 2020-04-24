namespace VL.WindowsFormsApp
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
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.text = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.freq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sendtime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.run = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.interval = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tasktype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lb = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
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
            this.sendtime,
            this.run,
            this.interval,
            this.tasktype});
            this.dgv_task.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_task.Location = new System.Drawing.Point(0, 0);
            this.dgv_task.Margin = new System.Windows.Forms.Padding(4);
            this.dgv_task.Name = "dgv_task";
            this.dgv_task.RowHeadersWidth = 51;
            this.dgv_task.RowTemplate.Height = 23;
            this.dgv_task.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_task.Size = new System.Drawing.Size(1309, 561);
            this.dgv_task.TabIndex = 1;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "编号";
            this.id.MinimumWidth = 6;
            this.id.Name = "id";
            this.id.Width = 40;
            // 
            // text
            // 
            this.text.DataPropertyName = "text";
            this.text.HeaderText = "任务名称";
            this.text.MinimumWidth = 6;
            this.text.Name = "text";
            this.text.Width = 140;
            // 
            // freq
            // 
            this.freq.DataPropertyName = "freq";
            this.freq.HeaderText = "周期";
            this.freq.MinimumWidth = 6;
            this.freq.Name = "freq";
            this.freq.Width = 80;
            // 
            // sendtime
            // 
            this.sendtime.DataPropertyName = "sendtime";
            this.sendtime.HeaderText = "执行时间";
            this.sendtime.MinimumWidth = 6;
            this.sendtime.Name = "sendtime";
            this.sendtime.Width = 80;
            // 
            // run
            // 
            this.run.DataPropertyName = "run";
            this.run.HeaderText = "执行";
            this.run.MinimumWidth = 6;
            this.run.Name = "run";
            this.run.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.run.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.run.Width = 40;
            // 
            // interval
            // 
            this.interval.DataPropertyName = "interval";
            this.interval.HeaderText = "间隔";
            this.interval.MinimumWidth = 6;
            this.interval.Name = "interval";
            this.interval.Width = 40;
            // 
            // tasktype
            // 
            this.tasktype.DataPropertyName = "tasktype";
            this.tasktype.HeaderText = "任务类型";
            this.tasktype.MinimumWidth = 6;
            this.tasktype.Name = "tasktype";
            this.tasktype.Width = 80;
            // 
            // lb
            // 
            this.lb.FormattingEnabled = true;
            this.lb.ItemHeight = 15;
            this.lb.Location = new System.Drawing.Point(553, 0);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(756, 454);
            this.lb.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(204, 475);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(168, 59);
            this.button1.TabIndex = 13;
            this.button1.Text = "更新任务";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(406, 475);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(168, 59);
            this.button2.TabIndex = 14;
            this.button2.Text = "清理任务";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1309, 561);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lb);
            this.Controls.Add(this.dgv_task);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_task)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_task;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn text;
        private System.Windows.Forms.DataGridViewTextBoxColumn freq;
        private System.Windows.Forms.DataGridViewTextBoxColumn sendtime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn run;
        private System.Windows.Forms.DataGridViewTextBoxColumn interval;
        private System.Windows.Forms.DataGridViewTextBoxColumn tasktype;
        private System.Windows.Forms.ListBox lb;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

