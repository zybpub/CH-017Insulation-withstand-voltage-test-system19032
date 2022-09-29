namespace Test_Automation
{
    partial class Form_Mysql_laiya
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn_excel = new System.Windows.Forms.Button();
            this.btn_readall = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ts1 = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(520, 621);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 34);
            this.button1.TabIndex = 0;
            this.button1.Text = "读取最近100条";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(22, 26);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1028, 589);
            this.dataGridView1.TabIndex = 1;
            // 
            // btn_excel
            // 
            this.btn_excel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_excel.Location = new System.Drawing.Point(794, 621);
            this.btn_excel.Name = "btn_excel";
            this.btn_excel.Size = new System.Drawing.Size(113, 33);
            this.btn_excel.TabIndex = 13;
            this.btn_excel.Text = "导出成电子表格";
            this.btn_excel.UseVisualStyleBackColor = true;
            this.btn_excel.Click += new System.EventHandler(this.btn_excel_Click);
            // 
            // btn_readall
            // 
            this.btn_readall.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_readall.Location = new System.Drawing.Point(658, 621);
            this.btn_readall.Name = "btn_readall";
            this.btn_readall.Size = new System.Drawing.Size(113, 34);
            this.btn_readall.TabIndex = 0;
            this.btn_readall.Text = "读取全部";
            this.btn_readall.UseVisualStyleBackColor = true;
            this.btn_readall.Click += new System.EventHandler(this.btn_readall_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textBox1.Location = new System.Drawing.Point(163, 629);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(247, 21);
            this.textBox1.TabIndex = 14;
            // 
            // button4
            // 
            this.button4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button4.Location = new System.Drawing.Point(416, 627);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(60, 23);
            this.button4.TabIndex = 15;
            this.button4.Text = "查找";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(134, 632);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "SN:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ts1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 666);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1076, 22);
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ts1
            // 
            this.ts1.Name = "ts1";
            this.ts1.Size = new System.Drawing.Size(131, 17);
            this.ts1.Text = "toolStripStatusLabel1";
            // 
            // Form_Mysql_laiya
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1076, 688);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btn_excel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn_readall);
            this.Controls.Add(this.button1);
            this.Name = "Form_Mysql_laiya";
            this.Text = "测试数据";
            this.Load += new System.EventHandler(this.Form_Mysql_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_excel;
        private System.Windows.Forms.Button btn_readall;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ts1;
    }
}