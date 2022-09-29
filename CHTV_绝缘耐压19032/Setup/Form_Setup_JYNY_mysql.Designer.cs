namespace Test_Automation.Setup
{
    partial class Form_Setup_JYNY_mysql
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.Gpt9000_SerialPort = new System.IO.Ports.SerialPort(this.components);
            this.btn_portopen = new System.Windows.Forms.Button();
            this.btn_portclose = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tb_memo = new System.Windows.Forms.TextBox();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_set = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lb_type = new System.Windows.Forms.Label();
            this.lb_id = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_teston = new System.Windows.Forms.Button();
            this.btn_readmes2 = new System.Windows.Forms.Button();
            this.btn_readmes1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(312, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(577, 40);
            this.label1.TabIndex = 127;
            this.label1.Text = "绝缘耐压计GP9000通讯和参设置";
            // 
            // btn_portopen
            // 
            this.btn_portopen.Location = new System.Drawing.Point(845, 76);
            this.btn_portopen.Name = "btn_portopen";
            this.btn_portopen.Size = new System.Drawing.Size(106, 23);
            this.btn_portopen.TabIndex = 134;
            this.btn_portopen.Text = "打开串口连接";
            this.btn_portopen.UseVisualStyleBackColor = true;
            this.btn_portopen.Click += new System.EventHandler(this.btn_portopen_Click);
            // 
            // btn_portclose
            // 
            this.btn_portclose.Location = new System.Drawing.Point(975, 75);
            this.btn_portclose.Name = "btn_portclose";
            this.btn_portclose.Size = new System.Drawing.Size(106, 23);
            this.btn_portclose.TabIndex = 135;
            this.btn_portclose.Text = "关闭串口连接";
            this.btn_portclose.UseVisualStyleBackColor = true;
            this.btn_portclose.Click += new System.EventHandler(this.btn_portclose_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(52, 105);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1064, 251);
            this.dataGridView1.TabIndex = 0;
            // 
            // tb_memo
            // 
            this.tb_memo.Location = new System.Drawing.Point(52, 396);
            this.tb_memo.Multiline = true;
            this.tb_memo.Name = "tb_memo";
            this.tb_memo.Size = new System.Drawing.Size(313, 283);
            this.tb_memo.TabIndex = 136;
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(1010, 372);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(106, 40);
            this.btn_save.TabIndex = 137;
            this.btn_save.Text = "保存修改";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_set
            // 
            this.btn_set.Location = new System.Drawing.Point(1010, 600);
            this.btn_set.Name = "btn_set";
            this.btn_set.Size = new System.Drawing.Size(106, 40);
            this.btn_set.TabIndex = 138;
            this.btn_set.Text = "设置为当前测试参数";
            this.btn_set.UseVisualStyleBackColor = true;
            this.btn_set.Click += new System.EventHandler(this.btn_set_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(458, 424);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(529, 150);
            this.dataGridView2.TabIndex = 140;
            this.dataGridView2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            this.dataGridView2.SelectionChanged += new System.EventHandler(this.dataGridView2_SelectionChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1010, 539);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(106, 35);
            this.button2.TabIndex = 141;
            this.button2.Text = "保存修改";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1008, 465);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(108, 21);
            this.textBox1.TabIndex = 142;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1008, 439);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 143;
            this.label2.Text = "型号搜索";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(827, 604);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 144;
            this.label3.Text = "选定型号：";
            // 
            // lb_type
            // 
            this.lb_type.AutoSize = true;
            this.lb_type.Location = new System.Drawing.Point(899, 603);
            this.lb_type.Name = "lb_type";
            this.lb_type.Size = new System.Drawing.Size(35, 12);
            this.lb_type.TabIndex = 145;
            this.lb_type.Text = "xxxxx";
            // 
            // lb_id
            // 
            this.lb_id.AutoSize = true;
            this.lb_id.Location = new System.Drawing.Point(899, 628);
            this.lb_id.Name = "lb_id";
            this.lb_id.Size = new System.Drawing.Size(35, 12);
            this.lb_id.TabIndex = 147;
            this.lb_id.Text = "xxxxx";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(863, 629);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 146;
            this.label5.Text = "ID：";
            // 
            // btn_teston
            // 
            this.btn_teston.Location = new System.Drawing.Point(461, 655);
            this.btn_teston.Name = "btn_teston";
            this.btn_teston.Size = new System.Drawing.Size(118, 63);
            this.btn_teston.TabIndex = 148;
            this.btn_teston.Text = "启动测试";
            this.btn_teston.UseVisualStyleBackColor = true;
            this.btn_teston.Click += new System.EventHandler(this.btn_teston_Click);
            // 
            // btn_readmes2
            // 
            this.btn_readmes2.Location = new System.Drawing.Point(761, 655);
            this.btn_readmes2.Name = "btn_readmes2";
            this.btn_readmes2.Size = new System.Drawing.Size(93, 63);
            this.btn_readmes2.TabIndex = 150;
            this.btn_readmes2.Text = "读取测试结果2";
            this.btn_readmes2.UseVisualStyleBackColor = true;
            this.btn_readmes2.Click += new System.EventHandler(this.btn_readmes2_Click);
            // 
            // btn_readmes1
            // 
            this.btn_readmes1.Location = new System.Drawing.Point(628, 655);
            this.btn_readmes1.Name = "btn_readmes1";
            this.btn_readmes1.Size = new System.Drawing.Size(92, 63);
            this.btn_readmes1.TabIndex = 149;
            this.btn_readmes1.Text = "读取测试结果1";
            this.btn_readmes1.UseVisualStyleBackColor = true;
            this.btn_readmes1.Click += new System.EventHandler(this.btn_readmes1_Click);
            // 
            // Form_Setup_JYNY_mysql
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1160, 740);
            this.Controls.Add(this.btn_readmes2);
            this.Controls.Add(this.btn_readmes1);
            this.Controls.Add(this.btn_teston);
            this.Controls.Add(this.lb_id);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lb_type);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.btn_set);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.tb_memo);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn_portclose);
            this.Controls.Add(this.btn_portopen);
            this.Controls.Add(this.label1);
            this.Name = "Form_Setup_JYNY_mysql";
            this.Text = "绝缘耐压通讯设置";
            this.Load += new System.EventHandler(this.Form_Setup_JYNY_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.IO.Ports.SerialPort Gpt9000_SerialPort;
        private System.Windows.Forms.Button btn_portopen;
        private System.Windows.Forms.Button btn_portclose;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox tb_memo;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_set;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lb_type;
        private System.Windows.Forms.Label lb_id;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_teston;
        private System.Windows.Forms.Button btn_readmes2;
        private System.Windows.Forms.Button btn_readmes1;
    }
}