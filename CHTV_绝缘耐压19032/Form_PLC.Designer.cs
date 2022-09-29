namespace Test_Automation
{
    partial class Form_PLC
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
            this.lb_connect_info = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbPLC_station = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbPLC_port = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbPLC_ip = new System.Windows.Forms.TextBox();
            this.lb_reg_write_message = new System.Windows.Forms.Label();
            this.lb_reg = new System.Windows.Forms.Label();
            this.btn_reg_read = new System.Windows.Forms.Button();
            this.tb_PLC_StartTest_Register = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_value = new System.Windows.Forms.TextBox();
            this.btn_reg_write = new System.Windows.Forms.Button();
            this.btn_client_disconnect = new System.Windows.Forms.Button();
            this.btn_client_connect = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tb_PLC_Adapter_Register = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_light_reg = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_PLC_Tvpass_Register = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_PLC_SN_Register = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.SuspendLayout();
            // 
            // lb_connect_info
            // 
            this.lb_connect_info.AutoSize = true;
            this.lb_connect_info.Location = new System.Drawing.Point(669, 46);
            this.lb_connect_info.Name = "lb_connect_info";
            this.lb_connect_info.Size = new System.Drawing.Size(59, 12);
            this.lb_connect_info.TabIndex = 36;
            this.lb_connect_info.Text = "未连接PLC";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 111);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 35;
            this.label6.Text = "Station：";
            // 
            // tbPLC_station
            // 
            this.tbPLC_station.Location = new System.Drawing.Point(107, 108);
            this.tbPLC_station.Name = "tbPLC_station";
            this.tbPLC_station.Size = new System.Drawing.Size(80, 21);
            this.tbPLC_station.TabIndex = 34;
            this.tbPLC_station.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(59, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 33;
            this.label5.Text = "Port：";
            // 
            // tbPLC_port
            // 
            this.tbPLC_port.Location = new System.Drawing.Point(107, 77);
            this.tbPLC_port.Name = "tbPLC_port";
            this.tbPLC_port.Size = new System.Drawing.Size(80, 21);
            this.tbPLC_port.TabIndex = 32;
            this.tbPLC_port.Text = "502";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(69, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 31;
            this.label4.Text = "IP：";
            // 
            // tbPLC_ip
            // 
            this.tbPLC_ip.Location = new System.Drawing.Point(107, 50);
            this.tbPLC_ip.Name = "tbPLC_ip";
            this.tbPLC_ip.Size = new System.Drawing.Size(106, 21);
            this.tbPLC_ip.TabIndex = 30;
            this.tbPLC_ip.Text = "10.52.99.132";
            // 
            // lb_reg_write_message
            // 
            this.lb_reg_write_message.AutoSize = true;
            this.lb_reg_write_message.Location = new System.Drawing.Point(407, 314);
            this.lb_reg_write_message.Name = "lb_reg_write_message";
            this.lb_reg_write_message.Size = new System.Drawing.Size(0, 12);
            this.lb_reg_write_message.TabIndex = 29;
            // 
            // lb_reg
            // 
            this.lb_reg.AutoSize = true;
            this.lb_reg.Location = new System.Drawing.Point(403, 353);
            this.lb_reg.Name = "lb_reg";
            this.lb_reg.Size = new System.Drawing.Size(11, 12);
            this.lb_reg.TabIndex = 28;
            this.lb_reg.Text = "0";
            // 
            // btn_reg_read
            // 
            this.btn_reg_read.Enabled = false;
            this.btn_reg_read.Location = new System.Drawing.Point(326, 348);
            this.btn_reg_read.Name = "btn_reg_read";
            this.btn_reg_read.Size = new System.Drawing.Size(71, 22);
            this.btn_reg_read.TabIndex = 27;
            this.btn_reg_read.Text = "读取";
            this.btn_reg_read.UseVisualStyleBackColor = true;
            this.btn_reg_read.Click += new System.EventHandler(this.btn_reg_read_Click);
            // 
            // tb_PLC_StartTest_Register
            // 
            this.tb_PLC_StartTest_Register.Location = new System.Drawing.Point(142, 308);
            this.tb_PLC_StartTest_Register.Name = "tb_PLC_StartTest_Register";
            this.tb_PLC_StartTest_Register.Size = new System.Drawing.Size(45, 21);
            this.tb_PLC_StartTest_Register.TabIndex = 26;
            this.tb_PLC_StartTest_Register.Text = "5000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 314);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 25;
            this.label3.Text = "启动测试寄存器：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(198, 311);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 24;
            this.label2.Text = "值：";
            // 
            // tb_value
            // 
            this.tb_value.Location = new System.Drawing.Point(246, 309);
            this.tb_value.Name = "tb_value";
            this.tb_value.Size = new System.Drawing.Size(62, 21);
            this.tb_value.TabIndex = 23;
            this.tb_value.Text = "1";
            // 
            // btn_reg_write
            // 
            this.btn_reg_write.Location = new System.Drawing.Point(323, 309);
            this.btn_reg_write.Name = "btn_reg_write";
            this.btn_reg_write.Size = new System.Drawing.Size(75, 23);
            this.btn_reg_write.TabIndex = 22;
            this.btn_reg_write.Text = "写入";
            this.btn_reg_write.UseVisualStyleBackColor = true;
            this.btn_reg_write.Click += new System.EventHandler(this.btn_reg_write_Click);
            // 
            // btn_client_disconnect
            // 
            this.btn_client_disconnect.Enabled = false;
            this.btn_client_disconnect.Location = new System.Drawing.Point(518, 89);
            this.btn_client_disconnect.Name = "btn_client_disconnect";
            this.btn_client_disconnect.Size = new System.Drawing.Size(137, 47);
            this.btn_client_disconnect.TabIndex = 20;
            this.btn_client_disconnect.Text = "断开服务端";
            this.btn_client_disconnect.UseVisualStyleBackColor = true;
            // 
            // btn_client_connect
            // 
            this.btn_client_connect.Location = new System.Drawing.Point(518, 36);
            this.btn_client_connect.Name = "btn_client_connect";
            this.btn_client_connect.Size = new System.Drawing.Size(137, 47);
            this.btn_client_connect.TabIndex = 21;
            this.btn_client_connect.Text = "连接服务端";
            this.btn_client_connect.UseVisualStyleBackColor = true;
            this.btn_client_connect.Click += new System.EventHandler(this.btn_client_connect_Click);
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(589, 433);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 27);
            this.btn_save.TabIndex = 41;
            this.btn_save.Text = "保存设置";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(326, 269);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 42;
            this.button1.Text = "写入2";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tb_PLC_Adapter_Register
            // 
            this.tb_PLC_Adapter_Register.Location = new System.Drawing.Point(142, 154);
            this.tb_PLC_Adapter_Register.Name = "tb_PLC_Adapter_Register";
            this.tb_PLC_Adapter_Register.Size = new System.Drawing.Size(45, 21);
            this.tb_PLC_Adapter_Register.TabIndex = 49;
            this.tb_PLC_Adapter_Register.Text = "4099";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(35, 160);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 12);
            this.label7.TabIndex = 48;
            this.label7.Text = "对接控制寄存器：";
            // 
            // tb_light_reg
            // 
            this.tb_light_reg.Location = new System.Drawing.Point(142, 213);
            this.tb_light_reg.Name = "tb_light_reg";
            this.tb_light_reg.Size = new System.Drawing.Size(45, 21);
            this.tb_light_reg.TabIndex = 51;
            this.tb_light_reg.Text = "4119";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(35, 219);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 12);
            this.label8.TabIndex = 50;
            this.label8.Text = "信号灯控制寄存器：";
            // 
            // tb_PLC_Tvpass_Register
            // 
            this.tb_PLC_Tvpass_Register.Location = new System.Drawing.Point(142, 370);
            this.tb_PLC_Tvpass_Register.Name = "tb_PLC_Tvpass_Register";
            this.tb_PLC_Tvpass_Register.Size = new System.Drawing.Size(45, 21);
            this.tb_PLC_Tvpass_Register.TabIndex = 53;
            this.tb_PLC_Tvpass_Register.Text = "5001";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 376);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 52;
            this.label1.Text = "电视放行寄存器：";
            // 
            // tb_PLC_SN_Register
            // 
            this.tb_PLC_SN_Register.Location = new System.Drawing.Point(142, 405);
            this.tb_PLC_SN_Register.Name = "tb_PLC_SN_Register";
            this.tb_PLC_SN_Register.Size = new System.Drawing.Size(45, 21);
            this.tb_PLC_SN_Register.TabIndex = 55;
            this.tb_PLC_SN_Register.Text = "5020";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(35, 411);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 54;
            this.label9.Text = "电视SN寄存器：";
            // 
            // Form_PLC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 472);
            this.Controls.Add(this.tb_PLC_SN_Register);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tb_PLC_Tvpass_Register);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_light_reg);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tb_PLC_Adapter_Register);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.lb_connect_info);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbPLC_station);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbPLC_port);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbPLC_ip);
            this.Controls.Add(this.lb_reg_write_message);
            this.Controls.Add(this.lb_reg);
            this.Controls.Add(this.btn_reg_read);
            this.Controls.Add(this.tb_PLC_StartTest_Register);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_value);
            this.Controls.Add(this.btn_reg_write);
            this.Controls.Add(this.btn_client_disconnect);
            this.Controls.Add(this.btn_client_connect);
            this.Name = "Form_PLC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PLC测试";
            this.Load += new System.EventHandler(this.Form_PLC_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_connect_info;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbPLC_station;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbPLC_port;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbPLC_ip;
        private System.Windows.Forms.Label lb_reg_write_message;
        private System.Windows.Forms.Label lb_reg;
        private System.Windows.Forms.Button btn_reg_read;
        private System.Windows.Forms.TextBox tb_PLC_StartTest_Register;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_value;
        private System.Windows.Forms.Button btn_reg_write;
        private System.Windows.Forms.Button btn_client_disconnect;
        private System.Windows.Forms.Button btn_client_connect;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tb_PLC_Adapter_Register;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_light_reg;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_PLC_Tvpass_Register;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_PLC_SN_Register;
        private System.Windows.Forms.Label label9;
        private System.IO.Ports.SerialPort serialPort1;
    }
}