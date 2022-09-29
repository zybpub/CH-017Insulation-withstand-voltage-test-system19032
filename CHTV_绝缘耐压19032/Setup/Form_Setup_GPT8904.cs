using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_Automation.Setup
{
    public partial class Form_Setup_GPT8904 : Form
    { 
        public Form_Setup_GPT8904()
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
            Gpt9000_Event = new AutoResetEvent(false);
            exe_event = true;
            if (serial_init() == false) return;
            addmemo("读取ACW原来参数");
            commmand = "MANU1:EDIT:SHOW?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            string[] values = gpt9000_result.Split(',');
            tb_mode1.Text = values[0];

            tb_acw_vol.Text = values[1];
            tb_acw_chis.Text = values[2];
            tb_acw_clos.Text = values[3];
            tb_acw_ttim.Text = values[4];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            read_value2();
        }
            void read_value2() {
            DateTime start = DateTime.Now;
                Gpt9000_Event = new AutoResetEvent(false);
             exe_event = true;
            if (serial_init() == false) return;
            addmemo("读取ACW原来参数");
            commmand = "MAIN: FUNC MANU";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            // Gpt9000_Event.WaitOne();

            commmand = "MANU:STEP "+ tb_acw_stepnum.Text;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            // Gpt9000_Event.WaitOne();

            commmand = "MANU:NAME?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_acw_name.Text = gpt9000_result;

            commmand = "MANU: EDIT: MODE?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_mode1.Text = gpt9000_result;

            if (!tb_mode1.Text.Contains("ACW"))
            {
                addmemo("请先将STEP 1 测试模式修改为耐压测试ACW");
                return;
            }

            commmand = "MANU:ACW:VOLT?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_acw_vol.Text = gpt9000_result;

            commmand = "MANU:ACW:FREQ?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_acw_freq.Text = gpt9000_result;

            commmand = "MANU:ACW:CHIS?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_acw_chis.Text = gpt9000_result;

            commmand = "MANU:ACW:CLOS?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_acw_clos.Text = gpt9000_result;

            commmand = "MANU:ACW:REF?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_acw_ref.Text = gpt9000_result;

            commmand = "MANU:ACW:TTIM?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_acw_ttim.Text = gpt9000_result;

            TimeSpan ts = DateTime.Now - start;
            addmemo("用时：" + ts.TotalMilliseconds.ToString());
        }

        private void Read_PreValue()
        {
           



        }
        bool serial_inited = false;

        public AutoResetEvent Gpt9000_Event;

        bool serial_init()
        {
            //打开串口，连接绝缘耐压仪
            if (serial_inited == false)
            {
              
                try
                {
                    Gpt9000_SerialPort = new System.IO.Ports.SerialPort();
                    Gpt9000_SerialPort.PortName = config_json.Jyny_Device_PortName;
                    Gpt9000_SerialPort.BaudRate = Convert.ToInt32(config_json.Jyny_Device_BaudRate);
                    Gpt9000_SerialPort.DataBits = Convert.ToInt16(config_json.Jyny_Device_DataBits);
                    Gpt9000_SerialPort.DataReceived += serialPort1_DataReceived;

                    if (Gpt9000_SerialPort.IsOpen == false) Gpt9000_SerialPort.Open();
                    else
                    {
                        MessageBox.Show("串口被占用!");
                        return false;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("绝缘耐压仪连接出错:"+ex.Message);
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
  

        string gpt9000_result = "";
        private bool GPT9804_Check()
        {
         //   Task.Run(()=> {
                Gpt9000_Event = new AutoResetEvent(false);
                addmemo("发送指令*idn?到绝缘耐压计，如果收到正确响应后右边的设置项将变为可用");
                Gpt9000_SerialPort.WriteLine("*idn?");
            //System.Threading.Thread.Sleep(1000);

            if (Gpt9000_Event.WaitOne(2000))
            {
                addmemo("收绝缘耐压计回复：" + gpt9000_result);
                if (gpt9000_result.IndexOf("GPT-9804") == -1)
                {
                    addmemo("耐压计回复不正确");
                    return false;
                }
                addmemo("耐压计回复正确");
                addmemo("发送指令TEST:RET ON到绝缘耐压计");
                Gpt9000_SerialPort.WriteLine("TEST:RET ON");
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
            System.Threading.Thread.Sleep(100);
            gpt9000_result = Gpt9000_SerialPort.ReadExisting();
            addmemo("gpt received:"+gpt9000_result);
           if (exe_event)  Gpt9000_Event.Set();
        }

        private void btn_setmode_acw_Click(object sender, EventArgs e)
        {
            commmand = "MAIN: FUNC MANU";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            // Gpt9000_Event.WaitOne();

            commmand = "MANU:STEP 1";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            // Gpt9000_Event.WaitOne();


            commmand = "MANU: EDIT: MODE ACW";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            //Gpt9000_Event.WaitOne();
            tb_mode1.Text = "ACW";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tb_mode1.Text = "ACW";
            tb_acw_name.Text = "ACW25KV";
            tb_acw_chis.Text = "5.00mA";
            tb_acw_clos.Text = "0.2mA";
            tb_acw_freq.Text = "50 Hz";
            tb_acw_ref.Text = "0mA";
            tb_acw_vol.Text = "2.5kV";
        }

        bool exe_event = false;
        private void button5_Click(object sender, EventArgs e)
        {
            addmemo("保存ACW参数");
            exe_event = false;
            commmand = "MAIN: FUNC MANU";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            // Gpt9000_Event.WaitOne();

            commmand = "MANU:STEP "+tb_acw_stepnum.Text;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            // Gpt9000_Event.WaitOne();

            commmand = "MANU:NAME " + tb_acw_name.Text.Trim();
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU: EDIT: MODE ACW";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
         

            commmand = "MANU:ACW:VOLT " + tb_acw_vol.Text.Trim().Replace("kV", "");
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand );

            commmand = "MANU:ACW:FREQ " + tb_acw_freq.Text.Trim().Replace(" Hz", "");
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:ACW:CHIS " + tb_acw_chis.Text.Trim().Replace("mA", "");
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:ACW:CLOS " + tb_acw_clos.Text.Trim().Replace("mA", "");
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:ACW:REF " + tb_acw_ref.Text.Trim().Replace("mA", "");
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:ACW:TTIM " + tb_acw_ttim.Text.Trim().Replace(" S", "");
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Gpt9000_Event = new AutoResetEvent(false);
            exe_event = true;
            if (serial_init() == false) return;

           
            //读取自动信息
            commmand = "MAIN:FUNC AUTO";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

         
            commmand = "AUTO:STEP 1"; //这里的STEP应理解为测试组
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "AUTO:NAME?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_autoname.Text = gpt9000_result;

            commmand = "AUTO1:PAGE:SHOW?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            //
            string[] step_name = gpt9000_result.Split(',');
            tb_auto_step1.Text = step_name[0].Split(':')[1].Trim();
            tb_auto_step2.Text = step_name[1].Split(':')[1].Trim();
            tb_auto_step3.Text = step_name[2].Split(':')[1].Trim();
            // tb_autoname.Text = gpt9000_result;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (serial_init() == false) return;
            Gpt9000_Event = new AutoResetEvent(false);
            exe_event = true;
            if (serial_init() == false) return;
            addmemo("读取IR原来参数");
            commmand = "MAIN: FUNC MANU";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            // Gpt9000_Event.WaitOne();

            commmand = "MANU:STEP "+tb_ir_stepnum.Text;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            // Gpt9000_Event.WaitOne();

            commmand = "MANU:NAME?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_ir_name.Text = gpt9000_result;

            commmand = "MANU: EDIT: MODE?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_mode2.Text = gpt9000_result;

            if (!tb_mode2.Text.Contains("IR"))
            {
                addmemo("请先将STEP 2 测试模式修改为绝缘测试IR");
                return;
            }

            commmand = "MANU:IR:RHIS?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_ir_rhis.Text = gpt9000_result;

            commmand = "MANU:IR:RLOS?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_ir_rlos.Text = gpt9000_result;

            commmand = "MANU:IR:TTIM?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_ir_ttim.Text = gpt9000_result;

            commmand = "MANU:IR:REF?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_ir_ref.Text = gpt9000_result;

            commmand = "MANU: IR: VOLT ?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_ir_vol.Text = gpt9000_result;

            
        }

        private void btn_save_auto_Click(object sender, EventArgs e)
        {
            addmemo("保存AUTO参数");
            exe_event = false;
            commmand = "MAIN:FUNC AUTO";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            // Gpt9000_Event.WaitOne();

            commmand = "AUTO:STEP "+ tb_autostep.Text;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            // Gpt9000_Event.WaitOne();

            commmand = "AUTO:NAME " + tb_autoname.Text.Trim();
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            //清空原有步骤
            for(int i = 1; i <= 16; i++)
            {
                commmand = "AUTO:PAGE:DEL 1";
                addmemo(commmand);
                Gpt9000_SerialPort.WriteLine(commmand);
            }
           


            if (tb_auto_step1.Text != "")
            {
                commmand = "AUTO:EDIT:ADD "+ tb_auto_step1.Text;
                addmemo(commmand);
                Gpt9000_SerialPort.WriteLine(commmand);
            }
            if (tb_auto_step2.Text != "") {
                commmand = "AUTO:EDIT:ADD " + tb_auto_step2.Text;
                addmemo(commmand);
                Gpt9000_SerialPort.WriteLine(commmand);
            }
               

            if (tb_auto_step3.Text != "") {
                Gpt9000_SerialPort.WriteLine(commmand);
                commmand = "AUTO:EDIT:ADD " + tb_auto_step3.Text;
                addmemo(commmand);
                Gpt9000_SerialPort.WriteLine(commmand);
            }
          
        }

        private void button8_Click(object sender, EventArgs e)
        {
            tb_autoname.Text = "AUTO001";
            tb_auto_step1.Text = "001";
            tb_auto_step2.Text = "002";
            tb_auto_step3.Text = "003";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tb_mode1.Text = "ACW";
            tb_acw_name.Text = "ACW25KV";
            tb_acw_chis.Text = "5.00mA";
            tb_acw_clos.Text = "0.2mA";
            tb_acw_freq.Text = "50 Hz";
            tb_acw_ref.Text = "0mA";
            tb_acw_vol.Text = "2.5kV";
            tb_acw_ttim.Text = "3.0 S";
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            tb_ir_name.Text = "IR";
            tb_mode2.Text = "IR";
            tb_ir_ttim.Text = "2";
            tb_ir_vol.Text = "0.5";
            tb_ir_rhis.Text = "9999";
            tb_ir_rlos.Text = "500";
            tb_ir_ref.Text = "0";
        }

        private void btn_remoteoff_Click(object sender, EventArgs e)
        {
            commmand = "*RMTOFF";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            commmand = "TEST:RET ON";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            commmand = "TEST:RET OFF";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void btn_send_command_Click(object sender, EventArgs e)
        {
            commmand = tb_command.Text;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void btn_teston_Click(object sender, EventArgs e)
        {
            commmand = "FUNC:TEST ON";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void btn_testoff_Click(object sender, EventArgs e)
        {
            commmand = "FUNC:TEST OFF";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            commmand = "AUTO:EDIT:DEL 1";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            commmand = "MAIN:FUNC MANU";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            commmand = "MAIN:FUNC AUTO";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void btn_readmes1_Click(object sender, EventArgs e)
        {
            commmand = "MEAS1?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void btn_readmes2_Click(object sender, EventArgs e)
        {
            commmand = "MEAS2?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void btn_readmes3_Click(object sender, EventArgs e)
        {
            commmand = "MEAS3?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void btn_portopen_Click(object sender, EventArgs e)
        {
            Gpt9000_Event = new AutoResetEvent(false);
            exe_event = true;
            Task.Run(() =>
            {
                if (serial_init() == false) return;
                groupBox1.Enabled = true;
                btn_portclose.Enabled = true;
            });
        }

        private void btn_portclose_Click(object sender, EventArgs e)
        {
            Gpt9000_SerialPort.Close();
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
            if (serial_init() == false) return;
            Gpt9000_Event = new AutoResetEvent(false);
            exe_event = true;
            if (serial_init() == false) return;
            addmemo("读取GB原来参数");
            commmand = "MAIN: FUNC MANU";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            // Gpt9000_Event.WaitOne();

            commmand = "MANU:STEP " + tb_gb_stepnum.Text; ;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            // Gpt9000_Event.WaitOne();

            commmand = "MANU:NAME?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_gb_name.Text = gpt9000_result;

            commmand = "MANU: EDIT: MODE?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_mode3.Text = gpt9000_result;

            if (!tb_mode3.Text.Contains("GB"))
            {
                addmemo("请先将STEP 3 测试模式修改为阻抗测试GB");
                return;
            }

            commmand = "MANU:GB:RHIS?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_gb_rhis.Text = gpt9000_result;

            commmand = "MANU:GB:RLOS?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_gb_rlos.Text = gpt9000_result;

            commmand = "MANU:GB:TTIM?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_gb_ttim.Text = gpt9000_result;

            commmand = "MANU:GB:REF?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_gb_ref.Text = gpt9000_result;

            commmand = "MANU:GB:CURR?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_gb_curr.Text = gpt9000_result;

            commmand = "MANU:GB:FREQ?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_gb_freq.Text = gpt9000_result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            addmemo("保存IR参数");
            exe_event = false;
            commmand = "MAIN: FUNC MANU";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            // Gpt9000_Event.WaitOne();

            commmand = "MANU:STEP " + tb_ir_stepnum.Text; ;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            // Gpt9000_Event.WaitOne();

            commmand = "MANU:NAME " + tb_ir_name.Text.Trim();
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:EDIT:MODE IR";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);


            commmand = "MANU:IR:VOLT " + tb_ir_vol.Text.Trim().Replace("kV", "");
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:IR:RHIS " + tb_ir_rhis.Text.Trim().Replace("M", "");
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:IR:RLOS " + tb_ir_rlos.Text.Trim().Replace("M", "");
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:IR:REF " + tb_ir_ref.Text.Trim().Replace("M", "");
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:IR:TTIM " + tb_ir_ttim.Text.Trim().Replace(" S", "");
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            addmemo("保存GB参数");
            exe_event = false;
            commmand = "MAIN: FUNC MANU";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            // Gpt9000_Event.WaitOne();

            commmand = "MANU:STEP 3";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            // Gpt9000_Event.WaitOne();

            commmand = "MANU:NAME " + tb_gb_name.Text.Trim();
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:EDIT:MODE GB";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);


            commmand = "MANU:GB:CURR " + tb_gb_curr.Text.Trim().Replace("m Ohm", "");
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:GB:FREQ " + tb_gb_freq.Text.Trim().Replace(" Hz", "");
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:GB:RHIS " + tb_gb_rhis.Text.Trim().Replace("m Ohm", "");
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:GB:RLOS " + tb_gb_rlos.Text.Trim().Replace("m Ohm", "");
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:GB:REF " + tb_gb_ref.Text.Trim().Replace("m Ohm", "");
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:GB:TTIM " + tb_gb_ttim.Text.Trim().Replace(" S", "");
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void btn_autostep_read_Click(object sender, EventArgs e)
        {
            Gpt9000_Event = new AutoResetEvent(false);
            exe_event = true;
            if (serial_init() == false) return;
            commmand = "MAIN:FUNC AUTO";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            commmand = "AUTO:STEP?"; //这里的STEP应理解为测试组
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            Gpt9000_Event.WaitOne();
            tb_autostep.Text = gpt9000_result;
        }

        private void btn_autostep_write_Click(object sender, EventArgs e)
        {
            Gpt9000_Event = new AutoResetEvent(false);
            exe_event = true;
            if (serial_init() == false) return;
            commmand = "MAIN:FUNC AUTO";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            commmand = "AUTO:STEP "+tb_autostep.Text;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //        MANU: EDIT: MODE?
            //gpt received: GB

            //MANU:GB: RHIS?
            //gpt received: 126.7m Ohm

            //MANU:GB: RLOS?
            //gpt received: 000.0m Ohm

            // MANU:GB: TTIM?
            //  gpt received: 003.0 S

            //   MANU:GB: REF?
            //    gpt received: 024.6m Ohm

            //     MANU:GB: CURR?
            //      gpt received: 25.00A

            //       MANU:GB: FREQ?
            //        gpt received: 50 Hz

            tb_gb_name.Text = "GB";
            tb_mode3.Text = "GB";
            tb_gb_ttim.Text = "3";
            tb_gb_curr.Text = "25";
            tb_gb_freq.Text = "50";
            tb_gb_rhis.Text = "120";
            tb_gb_rlos.Text = "0";
            tb_gb_ref.Text = "20";
        }
    }
}
