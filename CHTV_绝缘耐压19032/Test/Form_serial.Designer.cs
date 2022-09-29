namespace Test_Automation
{
    partial class Form_serial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_serial));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.cmbBaudRate = new System.Windows.Forms.TextBox();
            this.tbDataBits = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_port_open = new System.Windows.Forms.Button();
            this.txtsend = new System.Windows.Forms.TextBox();
            this.btnsend = new System.Windows.Forms.Button();
            this.cmbStopBits = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbParity = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtreceived = new System.Windows.Forms.TextBox();
            this.cmbPortName = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "端口号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(232, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "波特率：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "数据位：";
            // 
            // serialPort1
            // 
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 201);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "奇偶校验：";
            // 
            // cmbBaudRate
            // 
            this.cmbBaudRate.Location = new System.Drawing.Point(279, 125);
            this.cmbBaudRate.Name = "cmbBaudRate";
            this.cmbBaudRate.Size = new System.Drawing.Size(100, 21);
            this.cmbBaudRate.TabIndex = 5;
            this.cmbBaudRate.Text = "9600";
            // 
            // tbDataBits
            // 
            this.tbDataBits.Enabled = false;
            this.tbDataBits.Location = new System.Drawing.Point(95, 162);
            this.tbDataBits.Name = "tbDataBits";
            this.tbDataBits.Size = new System.Drawing.Size(100, 21);
            this.tbDataBits.TabIndex = 6;
            this.tbDataBits.Text = "8";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(48, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "串口设置";
            // 
            // btn_port_open
            // 
            this.btn_port_open.Location = new System.Drawing.Point(285, 198);
            this.btn_port_open.Name = "btn_port_open";
            this.btn_port_open.Size = new System.Drawing.Size(94, 57);
            this.btn_port_open.TabIndex = 9;
            this.btn_port_open.Text = "打开端口";
            this.btn_port_open.UseVisualStyleBackColor = true;
            this.btn_port_open.Click += new System.EventHandler(this.btn_port_open_Click);
            // 
            // txtsend
            // 
            this.txtsend.Location = new System.Drawing.Point(57, 395);
            this.txtsend.Multiline = true;
            this.txtsend.Name = "txtsend";
            this.txtsend.Size = new System.Drawing.Size(262, 62);
            this.txtsend.TabIndex = 11;
            this.txtsend.Text = "*idn?\r\n";
            // 
            // btnsend
            // 
            this.btnsend.Location = new System.Drawing.Point(357, 421);
            this.btnsend.Name = "btnsend";
            this.btnsend.Size = new System.Drawing.Size(118, 32);
            this.btnsend.TabIndex = 12;
            this.btnsend.Text = "发送数据";
            this.btnsend.UseVisualStyleBackColor = true;
            this.btnsend.Click += new System.EventHandler(this.txtsend_Click);
            // 
            // cmbStopBits
            // 
            this.cmbStopBits.Enabled = false;
            this.cmbStopBits.Location = new System.Drawing.Point(279, 162);
            this.cmbStopBits.Name = "cmbStopBits";
            this.cmbStopBits.Size = new System.Drawing.Size(100, 21);
            this.cmbStopBits.TabIndex = 14;
            this.cmbStopBits.Text = "1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(232, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "停止位：";
            // 
            // cmbParity
            // 
            this.cmbParity.Enabled = false;
            this.cmbParity.FormattingEnabled = true;
            this.cmbParity.Items.AddRange(new object[] {
            "None"});
            this.cmbParity.Location = new System.Drawing.Point(107, 198);
            this.cmbParity.Name = "cmbParity";
            this.cmbParity.Size = new System.Drawing.Size(88, 20);
            this.cmbParity.TabIndex = 15;
            this.cmbParity.Text = "None";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(55, 269);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "接收到的数据";
            // 
            // txtreceived
            // 
            this.txtreceived.Location = new System.Drawing.Point(53, 284);
            this.txtreceived.Multiline = true;
            this.txtreceived.Name = "txtreceived";
            this.txtreceived.Size = new System.Drawing.Size(567, 101);
            this.txtreceived.TabIndex = 17;
            // 
            // cmbPortName
            // 
            this.cmbPortName.FormattingEnabled = true;
            this.cmbPortName.Location = new System.Drawing.Point(95, 125);
            this.cmbPortName.Name = "cmbPortName";
            this.cmbPortName.Size = new System.Drawing.Size(101, 20);
            this.cmbPortName.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(420, 105);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(179, 72);
            this.label8.TabIndex = 20;
            this.label8.Text = "发送*idn?\\r\\n 获取设备标识\r\nGPT-9804 ,EM110867    ,v2.08,\r\nTEST:RET ON\r\nMEAS1?\r\nMEAS2?\r\n\r" +
    "\n";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "*idn?",
            "Function:TEST ON",
            "MEAS1?",
            "MEAS2?"});
            this.comboBox1.Location = new System.Drawing.Point(357, 395);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(118, 20);
            this.comboBox1.TabIndex = 21;
            this.comboBox1.Text = "*idn?";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(562, 392);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(58, 24);
            this.button2.TabIndex = 23;
            this.button2.Text = "清除";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(498, 426);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 26);
            this.button1.TabIndex = 24;
            this.button1.Text = "全自动发送";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(57, 464);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(563, 167);
            this.textBox1.TabIndex = 25;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(178, 28);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(309, 27);
            this.label13.TabIndex = 77;
            this.label13.Text = "绝缘耐压计串口通讯测试";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(553, 645);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 78;
            this.button3.Text = "关闭";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form_serial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 680);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbPortName);
            this.Controls.Add(this.txtreceived);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbParity);
            this.Controls.Add(this.cmbStopBits);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnsend);
            this.Controls.Add(this.txtsend);
            this.Controls.Add(this.btn_port_open);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbDataBits);
            this.Controls.Add(this.cmbBaudRate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form_serial";
            this.Text = "绝缘耐压计串口通讯测试";
            this.Load += new System.EventHandler(this.Form_serial_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox cmbBaudRate;
        private System.Windows.Forms.TextBox tbDataBits;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_port_open;
        private System.Windows.Forms.TextBox txtsend;
        private System.Windows.Forms.Button btnsend;
        private System.Windows.Forms.TextBox cmbStopBits;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbParity;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtreceived;
        private System.Windows.Forms.ComboBox cmbPortName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button button3;
    }
}