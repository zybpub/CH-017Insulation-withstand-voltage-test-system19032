using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_Automation.Setup
{
    public partial class Form_Setup_19032 : Form
    { 
        public Form_Setup_19032()
        {
            InitializeComponent();
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        private void Form_Setup_JYNY_Load(object sender, EventArgs e)
        {
            config_json.config_json_readall();

            CheckForIllegalCrossThreadCalls = false;

            JYNY_SerialPort_Used.Checked = (bool)config_json.JYNY_SerialPort_Used;
            foreach (string com in System.IO.Ports.SerialPort.GetPortNames()) //自动获取串行口名称
            {
                JYNY_SerialPort_PortName.Items.Add(com);
            }
            try { JYNY_SerialPort_PortName.Text = config_json.Jyny_Device_PortName; } catch { }

            JYNY_SerialPort_PortName.Text = config_json.Jyny_Device_PortName;
            JYNY_SerialPort_BaudRate.Text = config_json.Jyny_Device_BaudRate;
            JYNY_SerialPort_DataBits.Text = config_json.Jyny_Device_DataBits;
            JYNY_SerialPort_StopBits.Text = config_json.Jyny_Device_StopBits;
            JYNY_SerialPort_Parity.Text = config_json.Jyny_Device_Parity;
        }

        private void btn_search_serial_no_Click(object sender, EventArgs e)
        {
            string comNum = Class_Serial.GetComNum(JYNY_Machine_Keywords.Text);

            if (comNum == "") { lb_message.Text = "未找到符合要求的串口号！"; }
            else
            {
                lb_message.Text = "找到串口号:" + comNum;
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists(config_json.config_file_name))
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo("d:\\软件配置");
                if (di.Exists == false) di.Create();

                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(config_json.config_file_name, true))
                {
                    writer.WriteLine("{}");
                    writer.Close();
                }
            }

            string json = System.IO.File.ReadAllText(config_json.config_file_name);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            jsonObj["JYNY_SerialPort_Used"] = JYNY_SerialPort_Used.Checked.ToString();

            jsonObj["Jyny_Device_PortName"] = JYNY_SerialPort_PortName.Text;
            jsonObj["Jyny_Device_BaudRate"] = JYNY_SerialPort_BaudRate.Text;
            jsonObj["Jyny_Device_DataBits"] = JYNY_SerialPort_DataBits.Text;
            jsonObj["Jyny_Device_StopBits"] = JYNY_SerialPort_StopBits.Text;
            jsonObj["Jyny_Device_Parity"] = JYNY_SerialPort_Parity.Text;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(config_json.config_file_name, output);
            config_json.config_json_readall();
            MessageBox.Show("保存成功！");
        }

        private void JYNY_SerialPort_Used_CheckedChanged(object sender, EventArgs e)
        {
            groupBox_jyny.Enabled = JYNY_SerialPort_Used.Checked;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }


        void read_value1() {
            //ACW,2.500kV,H=05.00mA,L=00.20mA,R=000.1S,T=001.0S
            //读取不到名称，频率值
            Device_Event = new AutoResetEvent(false);
            exe_event = true;
            if (serial_init() == false) return;
            addmemo("读取ACW原来参数");
            commmand = "MANU1:EDIT:SHOW?";
            addmemo(commmand);
            serialPort.WriteLine(commmand);
            Device_Event.WaitOne();
            string[] values = test_result.Split(',');

            tb_acw_vol.Text = values[1];
            tb_acw_chis.Text = values[2];
            tb_acw_clos.Text = values[3];
            tb_acw_ttim.Text = values[4];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            read_value2();
        }
        void read_value2()
        {
            DateTime start = DateTime.Now;
                Device_Event = new AutoResetEvent(false);
             exe_event = true;
            if (serial_init() == false) return;
            addmemo("读取ACW原来参数");
            commmand = "MAIN: FUNC MANU";
            addmemo(commmand);
            serialPort.WriteLine(commmand);

            // Gpt9000_Event.WaitOne();

            commmand = "MANU:STEP "+ tb_acw_stepnum.Text;
            addmemo(commmand);
            serialPort.WriteLine(commmand);
            // Gpt9000_Event.WaitOne();

            commmand = "MANU:NAME?";
            addmemo(commmand);
            serialPort.WriteLine(commmand);
            Device_Event.WaitOne();


            commmand = "MANU: EDIT: MODE?";
            addmemo(commmand);
            serialPort.WriteLine(commmand);
            Device_Event.WaitOne();


            commmand = "MANU:ACW:VOLT?";
            addmemo(commmand);
            serialPort.WriteLine(commmand);
            Device_Event.WaitOne();
            tb_acw_vol.Text = test_result;

            commmand = "MANU:ACW:FREQ?";
            addmemo(commmand);
            serialPort.WriteLine(commmand);
            Device_Event.WaitOne();

            commmand = "MANU:ACW:CHIS?";
            addmemo(commmand);
            serialPort.WriteLine(commmand);
            Device_Event.WaitOne();
            tb_acw_chis.Text = test_result;

            commmand = "MANU:ACW:CLOS?";
            addmemo(commmand);
            serialPort.WriteLine(commmand);
            Device_Event.WaitOne();
            tb_acw_clos.Text = test_result;

            commmand = "MANU:ACW:REF?";
            addmemo(commmand);
            serialPort.WriteLine(commmand);
            Device_Event.WaitOne();
            tb_acw_freq.Text = test_result;

            commmand = "MANU:ACW:TTIM?";
            addmemo(commmand);
            serialPort.WriteLine(commmand);
            Device_Event.WaitOne();
            tb_acw_ttim.Text = test_result;

            TimeSpan ts = DateTime.Now - start;
            addmemo("用时：" + ts.TotalMilliseconds.ToString());
        }
        bool serial_inited = false;

        public AutoResetEvent Device_Event;

        bool serial_init()
        {
            //打开串口，连接绝缘耐压仪
            if (serial_inited == false)
            {
              
                try
                {
                    serialPort = new System.IO.Ports.SerialPort();
                    serialPort.PortName = config_json.Jyny_Device_PortName;
                    serialPort.BaudRate = Convert.ToInt32(config_json.Jyny_Device_BaudRate);
                    serialPort.DataBits = Convert.ToInt16(config_json.Jyny_Device_DataBits);
                    serialPort.DataReceived += serialPort1_DataReceived;
                    addmemo("端口号："+serialPort.PortName);
                    if (serialPort.IsOpen == false) serialPort.Open();
                    else
                    {
                        MessageBox.Show("串口被占用!");
                        return false;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("绝缘耐压仪连接出错,请检查仪表是否已开机或者："+ex.Message);
                    return false;
                }

                if (false == GPT9804_Check())
                {
                    addmemo("串口连接的不是绝缘耐压仪！");
                    return false;
                }
                else
                {
                    addmemo("绝缘耐压仪连接正确");
                  
                  
                }
                serial_inited = true;
                return true;
            }
            return true;
        }
        string commmand = "";
  
        private bool GPT9804_Check()
        {
         //   Task.Run(()=> {
                Device_Event = new AutoResetEvent(false);
                addmemo("发送指令*idn?到绝缘耐压计，如果收到正确响应后右边的设置项将变为可用");
                serialPort.WriteLine("*IDN?");
            //System.Threading.Thread.Sleep(1000);

            if (Device_Event.WaitOne(2000))
            {
                addmemo("收绝缘耐压计回复：" + test_result);
                if (test_result.IndexOf("19032") == -1)
                {
                    addmemo("耐压计回复不正确");
                    return false;
                }
                addmemo("耐压计回复正确");

            }
            else {
                addmemo("超时，未收到正确响应，请检查连接和参数设置！");
                return false;
            }
               
               
          //  });
            return true;
        }
        private void addmemo(string memo)
        {
           
                memo =  memo + "\r\n";
                tb_memo.AppendText(memo);
                tb_memo.ScrollToCaret();
           
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(200);
            test_result = serialPort.ReadExisting();
          //  addmemo("gpt received:"+gpt9000_result);
            Device_Event.Set();
        }
        bool exe_event = false;
        private void button5_Click(object sender, EventArgs e)
        {
            addmemo("保存ACW参数");
            exe_event = false;
            commmand = "MAIN: FUNC MANU";
            addmemo(commmand);
            serialPort.WriteLine(commmand);

            // Gpt9000_Event.WaitOne();

            commmand = "MANU:STEP "+tb_acw_stepnum.Text;
            addmemo(commmand);
            serialPort.WriteLine(commmand);
            // Gpt9000_Event.WaitOne();

            addmemo(commmand);
            serialPort.WriteLine(commmand);

            commmand = "MANU: EDIT: MODE ACW";
            addmemo(commmand);
            serialPort.WriteLine(commmand);
         

            commmand = "MANU:ACW:VOLT " + tb_acw_vol.Text.Trim().Replace("kV", "");
            addmemo(commmand);
            serialPort.WriteLine(commmand );

          //  commmand = "MANU:ACW:FREQ " + tb_acw_freq.Text.Trim().Replace(" Hz", "");
            addmemo(commmand);
            serialPort.WriteLine(commmand);

            commmand = "MANU:ACW:CHIS " + tb_acw_chis.Text.Trim().Replace("mA", "");
            addmemo(commmand);
            serialPort.WriteLine(commmand);

            commmand = "MANU:ACW:CLOS " + tb_acw_clos.Text.Trim().Replace("mA", "");
            addmemo(commmand);
            serialPort.WriteLine(commmand);

            commmand = "MANU:ACW:REF " + tb_acw_freq.Text.Trim().Replace("mA", "");
            addmemo(commmand);
            serialPort.WriteLine(commmand);

            commmand = "MANU:ACW:TTIM " + tb_acw_ttim.Text.Trim().Replace(" S", "");
            addmemo(commmand);
            serialPort.WriteLine(commmand);
        }



        private void btn_remoteoff_Click(object sender, EventArgs e)
        {
            commmand = Serial_Command.装置重置;
            addmemo(commmand);
            serialPort.WriteLine(commmand);
        }

        private void btn_send_command_Click(object sender, EventArgs e)
        {
            commmand = tb_command.Text;
            addmemo(commmand);
            serialPort.WriteLine(commmand);
        }

        private void btn_teston_Click(object sender, EventArgs e)
        {
            commmand =Serial_Command.启动测试;
            addmemo(commmand);
            serialPort.WriteLine(commmand);
        }

        private void btn_testoff_Click(object sender, EventArgs e)
        {
            commmand = Serial_Command.停止测试;
            addmemo(commmand);
            serialPort.WriteLine(commmand);
        }

        private void btn_readmes1_Click(object sender, EventArgs e)
        {
            commmand = "MEAS1?";
            addmemo(commmand);
            serialPort.WriteLine(commmand);
        }

        private void btn_portopen_Click(object sender, EventArgs e)
        {
            Device_Event = new AutoResetEvent(false);
            exe_event = true;
            Task.Run(() =>
            {
                if (serial_init() == false) return;
                groupBox1.Enabled = true;
                btn_portclose.Enabled = true;
                button6_Click_1(null, null);
            });
        }

        private void btn_portclose_Click(object sender, EventArgs e)
        {
            serialPort.Close();
            groupBox1.Enabled = false;
            btn_portopen.Enabled = true;
            serial_inited = false;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Setup.Form_Setup_JYNY_mysql f = new Form_Setup_JYNY_mysql();
            f.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            command = "SOUR:SAFE:STEP 2:IR:LEV "+ tb_ir_vol.Text;
            addmemo("IR LEV（" + command + "）");
            serialPort.WriteLine(command);

            command = "SOUR:SAFE:STEP 2:IR:TIME:RAMP "+tb_ir_ramp.Text;
            addmemo("IR RAMP（" + command + "）");
            serialPort.WriteLine(command);
        
            command = "SOUR:SAFE:STEP 2:IR:TIME:FALL " + tb_ir_fall.Text;
            addmemo("IR FALL（" + command + "）");
            serialPort.WriteLine(command);
         
            command = "SOUR:SAFE:STEP 2:IR:TIME:TEST "+ tb_ir_ttim.Text;
            addmemo("IR TEST（" + command + "）");
            serialPort.WriteLine(command);
          
            command = "SOUR:SAFE:STEP 2:IR:LIM:HIGH "+ tb_ir_rhis.Text;
            addmemo("IR HIGH（" + command + "）");
            serialPort.WriteLine(command);
        
            command = "SOUR:SAFE:STEP 2:IR:LIM:LOW "+ tb_ir_rlos.Text;
            addmemo("IR LOW（" + command + "）");
            serialPort.WriteLine(command);
            MessageBox.Show("IR 设置完成");
        }

        private void button11_Click(object sender, EventArgs e)
        {
         
        }

        private void button10_Click(object sender, EventArgs e)
        {
            tb_gb_name.Text = "GB";
            tb_mode3.Text = "GB";
            tb_gb_ttim.Text = "3";
            tb_gb_curr.Text = "25";
            tb_gb_freq.Text = "50";
            tb_gb_rhis.Text = "120";
            tb_gb_rlos.Text = "0";
            tb_gb_ref.Text = "20";
        }
        string command = "";
        string test_result = "";
        private void button6_Click_1(object sender, EventArgs e)
        {
            test_result = "";
            command = Serial_Command.查询测试项目数;
            addmemo("查询测试项目数（" + command + "）");
            serialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);

            test_result = "";
            command = "SOUR:SAFE:STEP 1:SET?";
            addmemo("查询步骤1测试详细设置（" + command + "）");
            serialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);

            test_result = "";
            command = "SOUR:SAFE:STEP 1:AC:LEV?";
            addmemo("AC LEV（" + command + "）");
            serialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            
            try
            {
                tb_acw_vol.Text = Convert.ToDouble(test_result).ToString();
            }
            catch { }

            test_result = "";
            command = "SOUR:SAFE:STEP 1:AC:TIME:TEST?";
            addmemo("AC TEST（" + command + "）");
            serialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            tb_acw_ttim.Text = Convert.ToDouble(test_result).ToString();

            test_result = "";
            command = "SOUR:SAFE:STEP 1:AC:TIME:RAMP?";
            addmemo("AC RAMP（" + command + "）");
            serialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            tb_ac_ramp.Text = Convert.ToDouble(test_result).ToString();

            test_result = "";
            command = "SOUR:SAFE:STEP 1:AC:TIME:FALL?";
            addmemo("AC FALL（" + command + "）");
            serialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            tb_ac_fall.Text = Convert.ToDouble(test_result).ToString();


            test_result = "";
            command = "SOUR:SAFE:STEP 1:AC:LIMI:HIGH?";
            addmemo("AC HIGH（" + command + "）");
            serialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            try
            { tb_acw_chis.Text = Convert.ToDouble(test_result).ToString();
            }
            catch { }
            test_result = "";
            command = "SOUR:SAFE:STEP 1:AC:LIMI:LOW?";
            addmemo("AC LOW（" + command + "）");
            serialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            tb_acw_clos.Text = Convert.ToDouble(test_result).ToString();

            test_result = "";
            command = "SOUR:SAFE:STEP 1:AC:FREQ?";
            addmemo("AC FREQ（" + command + "）");
            serialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            tb_acw_freq.Text = Convert.ToDouble(test_result).ToString();

            test_result = "";
            command = "SOUR:SAFE:STEP 1:AC:CURR:OFFSET?";
            addmemo("AC OFFSET（" + command + "）");
            serialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            tb_acw_offset.Text = Convert.ToDouble(test_result).ToString();



            test_result = "";
            command = "SOUR:SAFE:STEP 2:SET?";
            addmemo("查询步骤2测试详细设置（" + command + "）");
            serialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);

            test_result = "";
            command = "SOUR:SAFE:STEP 2:IR:LEV?";
            addmemo("IR LEV（" + command + "）");
            serialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            tb_ir_vol.Text = Convert.ToDouble(test_result).ToString();

            test_result = "";
            command = "SOUR:SAFE:STEP 2:IR:TIME:RAMP?";
            addmemo("IR RAMP（" + command + "）");
            serialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            tb_ir_ramp.Text = Convert.ToDouble(test_result).ToString();

            test_result = "";
            command = "SOUR:SAFE:STEP 2:IR:TIME:FALL?";
            addmemo("IR FALL（" + command + "）");
            serialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            tb_ir_fall.Text = Convert.ToDouble(test_result).ToString();

            test_result = "";
            command = "SOUR:SAFE:STEP 2:IR:TIME:TEST?";
            addmemo("IR TEST（" + command + "）");
            serialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            tb_ir_ttim.Text = Convert.ToDouble(test_result).ToString();


            test_result = "";
            command = "SOUR:SAFE:STEP 2:IR:LIM:HIGH?";
            addmemo("IR HIGH（" + command + "）");
            serialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            tb_ir_rhis.Text = Convert.ToDouble(test_result).ToString();

            test_result = "";
            command = "SOUR:SAFE:STEP 2:IR:LIM:LOW?";
            addmemo("IR LOW（" + command + "）");
            serialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            tb_ir_rlos.Text = Convert.ToDouble(test_result).ToString();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            command = "SOUR:SAFE:STEP 1:AC:LEV "+tb_acw_vol.Text;
            addmemo("AC LEV（" + command + "）");
            serialPort.WriteLine(command);

            command = "SOUR:SAFE:STEP 1:AC:TIME:TEST "+ tb_acw_ttim.Text;
            addmemo("AC TEST（" + command + "）");
            serialPort.WriteLine(command);
         
            command = "SOUR:SAFE:STEP 1:AC:TIME:RAMP " + tb_ac_ramp.Text;
            addmemo("AC RAMP（" + command + "）");
            serialPort.WriteLine(command);
           
            command = "SOUR:SAFE:STEP 1:AC:TIME:FALL "+ tb_ac_fall.Text;
            addmemo("AC FALL（" + command + "）");
            serialPort.WriteLine(command);
        
            command = "SOUR:SAFE:STEP 1:AC:LIMI:HIGH " + tb_acw_chis.Text;
            addmemo("AC HIGH（" + command + "）");
            serialPort.WriteLine(command);
           
            command = "SOUR:SAFE:STEP 1:AC:LIMI:LOW "+tb_acw_clos.Text;
            addmemo("AC LOW（" + command + "）");
            serialPort.WriteLine(command);
         
            command = "SOUR:SAFE:STEP 1:AC:FREQ "+ tb_acw_freq.Text;
            addmemo("AC FREQ（" + command + "）");
            //serialPort.WriteLine(command);
        
            command = "SOUR:SAFE:STEP 1:AC:CURR:OFFSET " + tb_acw_offset.Text;
            addmemo("AC OFFSET（" + command + "）");
            serialPort.WriteLine(command);

            MessageBox.Show("AC设置完成");
          
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           
            DialogResult dr = MessageBox.Show("你确认要清除原来的所有设置吗？", "严重警告", MessageBoxButtons.YesNoCancel);
            if (dr == DialogResult.Yes) {
                DialogResult dr2 = MessageBox.Show("你确认要清除原来的所有设置吗？", "再次警告", MessageBoxButtons.YesNoCancel);
                if (dr2 == DialogResult.Yes)
                {
                    command = "SOUR:SAFE:SNUMB?";
                    addmemo("查询测试总步骤数量（" + command + "）");
                    serialPort.WriteLine(command);
                    Device_Event.WaitOne();
                    addmemo("收绝缘耐压计回复：" + test_result);
                    for(int i = 1; i <= Convert.ToInt16(test_result); i++)
                    {
                        command = "SOUR:SAFE:STEP "+i.ToString()+":DEL";
                        addmemo("删除步骤"+i.ToString()+"（" + command + "）");
                        serialPort.WriteLine(command);
                    }
                }
            }
           
        }
    }
}


/*

AC LEV（SOUR:SAFE:STEP 1:AC:LEV 3000）
AC TEST（SOUR:SAFE:STEP 1:AC:TIME:TEST 2）
AC RAMP（SOUR:SAFE:STEP 1:AC:TIME:RAMP 0）
AC FALL（SOUR:SAFE:STEP 1:AC:TIME:FALL 0）
AC HIGH（SOUR:SAFE:STEP 1:AC:LIMI:HIGH 0.01）
AC LOW（SOUR:SAFE:STEP 1:AC:LIMI:LOW 0.0003）
AC FREQ（SOUR:SAFE:STEP 1:AC:FREQ 0）
AC OFFSET（SOUR:SAFE:STEP 1:AC:CURR:OFFSET 0）

IR LEV（SOUR:SAFE:STEP 2:IR:LEV 500）
IR RAMP（SOUR:SAFE:STEP 2:IR:TIME:RAMP 0）
IR FALL（SOUR:SAFE:STEP 2:IR:TIME:FALL 0）
IR TEST（SOUR:SAFE:STEP 2:IR:TIME:TEST 2）
IR HIGH（SOUR:SAFE:STEP 2:IR:LIM:HIGH 0）
IR LOW（SOUR:SAFE:STEP 2:IR:LIM:LOW 4000000）


查询测试总步骤数量（SOUR:SAFE:SNUMB?）
收绝缘耐压计回复：+2

删除步骤1（SOUR:SAFE:STEP 1:DEL）
删除步骤2（SOUR:SAFE:STEP 2:DEL）
     */
