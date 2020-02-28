namespace WindowsFormsApp1
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
            this.count = new System.Windows.Forms.Button();
            this.data1 = new System.Windows.Forms.TextBox();
            this.data2 = new System.Windows.Forms.TextBox();
            this.result = new System.Windows.Forms.Label();
            this.oprate = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // count
            // 
            this.count.Location = new System.Drawing.Point(478, 141);
            this.count.Name = "count";
            this.count.Size = new System.Drawing.Size(57, 54);
            this.count.TabIndex = 0;
            this.count.Text = "=";
            this.count.UseVisualStyleBackColor = true;
            this.count.Click += new System.EventHandler(this.count_Click);
            // 
            // data1
            // 
            this.data1.Location = new System.Drawing.Point(158, 158);
            this.data1.Name = "data1";
            this.data1.Size = new System.Drawing.Size(80, 25);
            this.data1.TabIndex = 1;
            this.data1.TextChanged += new System.EventHandler(this.data1_TextChanged);
            // 
            // data2
            // 
            this.data2.Location = new System.Drawing.Point(393, 158);
            this.data2.Name = "data2";
            this.data2.Size = new System.Drawing.Size(79, 25);
            this.data2.TabIndex = 2;
            this.data2.TextChanged += new System.EventHandler(this.data2_TextChanged);
            // 
            // result
            // 
            this.result.AutoSize = true;
            this.result.Location = new System.Drawing.Point(541, 163);
            this.result.Name = "result";
            this.result.Size = new System.Drawing.Size(55, 15);
            this.result.TabIndex = 3;
            this.result.Text = "label1";
            this.result.Visible = false;
            this.result.Click += new System.EventHandler(this.result_Click);
            // 
            // oprate
            // 
            this.oprate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.oprate.FormattingEnabled = true;
            this.oprate.IntegralHeight = false;
            this.oprate.Items.AddRange(new object[] {
            "+",
            "-",
            "*",
            "/"});
            this.oprate.Location = new System.Drawing.Point(253, 160);
            this.oprate.Name = "oprate";
            this.oprate.Size = new System.Drawing.Size(121, 23);
            this.oprate.TabIndex = 4;
            this.oprate.SelectedIndexChanged += new System.EventHandler(this.oprate_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.oprate);
            this.Controls.Add(this.result);
            this.Controls.Add(this.data2);
            this.Controls.Add(this.data1);
            this.Controls.Add(this.count);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button count;
        private System.Windows.Forms.TextBox data1;
        private System.Windows.Forms.TextBox data2;
        private System.Windows.Forms.Label result;
        private System.Windows.Forms.ComboBox oprate;
    }
}

