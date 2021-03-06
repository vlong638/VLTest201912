﻿namespace VL.WindowsFormsApp
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
            this.lb = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.frequenceType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastexecutetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActivated = new System.Windows.Forms.DataGridViewCheckBoxColumn();
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
            this.Id,
            this.name,
            this.frequenceType,
            this.lastexecutetime,
            this.IsActivated,
            this.interval,
            this.tasktype});
            this.dgv_task.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_task.Location = new System.Drawing.Point(0, 0);
            this.dgv_task.Name = "dgv_task";
            this.dgv_task.RowHeadersWidth = 51;
            this.dgv_task.RowTemplate.Height = 23;
            this.dgv_task.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_task.Size = new System.Drawing.Size(1313, 485);
            this.dgv_task.TabIndex = 1;
            // 
            // lb
            // 
            this.lb.FormattingEnabled = true;
            this.lb.ItemHeight = 12;
            this.lb.Location = new System.Drawing.Point(886, 0);
            this.lb.Margin = new System.Windows.Forms.Padding(2);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(427, 484);
            this.lb.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(153, 380);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 47);
            this.button1.TabIndex = 13;
            this.button1.Text = "更新任务";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(304, 380);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(126, 47);
            this.button2.TabIndex = 14;
            this.button2.Text = "清理任务";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // name
            // 
            this.name.DataPropertyName = "Name";
            this.name.HeaderText = "任务名称";
            this.name.MinimumWidth = 6;
            this.name.Name = "name";
            this.name.Width = 140;
            // 
            // frequenceType
            // 
            this.frequenceType.DataPropertyName = "FrequencyType";
            this.frequenceType.HeaderText = "周期";
            this.frequenceType.MinimumWidth = 6;
            this.frequenceType.Name = "frequenceType";
            this.frequenceType.Width = 80;
            // 
            // lastexecutetime
            // 
            this.lastexecutetime.DataPropertyName = "LastExecuteTime";
            this.lastexecutetime.HeaderText = "最后执行时间";
            this.lastexecutetime.MinimumWidth = 6;
            this.lastexecutetime.Name = "lastexecutetime";
            this.lastexecutetime.Width = 80;
            // 
            // IsActivated
            // 
            this.IsActivated.DataPropertyName = "IsActivated";
            this.IsActivated.HeaderText = "执行";
            this.IsActivated.MinimumWidth = 6;
            this.IsActivated.Name = "IsActivated";
            this.IsActivated.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsActivated.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.IsActivated.Width = 40;
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
            this.ClientSize = new System.Drawing.Size(1313, 485);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lb);
            this.Controls.Add(this.dgv_task);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_task)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_task;
        private System.Windows.Forms.ListBox lb;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn frequenceType;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastexecutetime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsActivated;
        private System.Windows.Forms.DataGridViewTextBoxColumn interval;
        private System.Windows.Forms.DataGridViewTextBoxColumn tasktype;
    }
}

