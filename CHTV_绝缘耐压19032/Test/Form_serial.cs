using System;
using System.Linq;
using System.Windows.Forms;

namespace Test_Automation
{
    public partial class Form_serial : Form
    {
        public Form_serial()
        {
            InitializeComponent();
            this.CenterToScreen();
        }
        private void Form_serial_Load(object sender, EventArgs e)
        {

          //  string sss = Microsoft.VisualBasic.Interaction.InputBox("请输入密码", "管理员确认", "", -1, -1);
          //  if (sss != config_json.pass) this.Close();

            try
            {
                foreach (string com in System.IO.Ports.SerialPort.GetPortNames()) //自动获取串行口名称

                    cmbPortName.Items.Add(com);
                cmbPortName.SelectedIndex = 0;
            }
            catch { }
            if (System.IO.File.Exists(config_json.config_file_name))
            {
                //读取配置
                try
                {
                    System.IO.StreamReader file = System.IO.File.OpenText(config_json.config_file_name);
                    Newtonsoft.Json.JsonTextReader reader = new Newtonsoft.Json.JsonTextReader(file);
                    Newtonsoft.Json.Linq.JObject jsonObject =
                                    (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.Linq.JToken.ReadFrom(reader);
                    if (jsonObject["Jyny_Device_PortName"] != null)
                        cmbPortName.Text = (string)jsonObject["Jyny_Device_PortName"];
                    if (jsonObject["Jyny_Device_BaudRate"] != null)
                        cmbBaudRate.Text = (string)jsonObject["Jyny_Device_BaudRate"];
                    if (jsonObject["Jyny_Device_DataBits"] != null)
                        tbDataBits.Text = (string)jsonObject["Jyny_Device_DataBits"];
                    if (jsonObject["Jyny_Device_StopBits"] != null)
                        cmbStopBits.Text = (string)jsonObject["Jyny_Device_StopBits"];
                    if (jsonObject["Jyny_Device_Parity"] != null)
                        cmbParity.Text = (string)jsonObject["cmbParity"];
                    file.Close();
                }
                catch
                {
                    //MessageBox.Show("CAN卡配置有误！");
                }
            }
        }

        private void btn_port_open_Click(object sender, EventArgs e)
        {
            // port with some basic settings
            // SerialPort port = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
            try
            {
                if (serialPort1.IsOpen)    //当前为打开，则进行关闭处理
                {
                    serialPort1.Close();
                   // gbPortSettings.Enabled = true;
                    btn_port_open.Text = "打开端口";
                    btnsend.Enabled = false;
                }
                else     //当前为关闭，则进行打开处理
                {
                    // Set the port's settings
                    serialPort1.PortName = cmbPortName.Text;
                    serialPort1.BaudRate = int.Parse(cmbBaudRate.Text);
                    serialPort1.DataBits = int.Parse(tbDataBits.Text);
                    serialPort1.StopBits = System.IO.Ports.StopBits.One;
                        //(System.IO.Ports.StopBits)Enum.Parse(typeof(System.IO.Ports.StopBits), cmbStopBits.Text);
                    serialPort1.Parity = System.IO.Ports.Parity.None;
                        //(System.IO.Ports.Parity)Enum.Parse(typeof(System.IO.Ports.Parity), cmbParity.Text);
                    btn_port_open.Text = "关闭端口";
                    serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                    serialPort1.DiscardOutBuffer();
                    btnsend.Enabled = true;
                }
            }
            catch (Exception er)
            { MessageBox.Show("端口打开失败！" + er.Message, "提示"); }

        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //使用委托进行跨线程读取数据
            Invoke
             (new EventHandler
               (delegate
               {
                   txtreceived.Text += serialPort1.ReadExisting();
               }
               )
              );
        }

        

        private void txtsend_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen != true)
                btn_port_open_Click(null, null);
            for (int i = 0; i < txtsend.Lines.Count(); i++)
                serialPort1.WriteLine(txtsend.Lines[i]);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            //配置文件不存在则创建一个
            if (System.IO.File.Exists(config_json.config_file_name) == false)
            {
                MessageBox.Show("保存失败！");
                return;
            }
            //保存配置文件
            try
            {
                string json = System.IO.File.ReadAllText(config_json.config_file_name);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                jsonObj["Jyny_Device_PortName"] = cmbPortName.Text;
                jsonObj["Jyny_Device_BaudRate"] = cmbBaudRate.Text;
                jsonObj["Jyny_Device_DataBits"] = tbDataBits.Text;
                jsonObj["Jyny_Device_StopBits"] = cmbStopBits.Text;
                jsonObj["Jyny_Device_Parity"] = cmbParity.Text;
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(config_json.config_file_name, output);
            }
            catch (Exception ex)
            {
                MessageBox.Show("配置文件有误：" + ex.Message);
            }
            MessageBox.Show("保存成功！");
        }

      
        private void config_json_read()
        {
           
        }

        private void config_json_save() {
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen != true)
                btn_port_open_Click(null, null);
            serialPort1.WriteLine(comboBox1.Text);
            txtsend.Text = comboBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen != true)
                btn_port_open_Click(null, null);

            serialPort1.WriteLine(comboBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtreceived.Text = "";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            /*
             发送*idn?\r\n 获取设备标识
            GPT-9804 ,EM110867    ,v2.08,
            MEAS1?
            MEAS2?

             */
            if (serialPort1.IsOpen != true)
                btn_port_open_Click(null, null);

            serialPort1.WriteLine("*idn?");
            serialPort1.WriteLine("Function:TEST ON");
            System.Threading.Thread.Sleep(5000);
            serialPort1.WriteLine("MEAS1?");
            serialPort1.WriteLine("MEAS2?");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}


/*
 MANU:STEP?
 MANU:STEP 100

 MANU:NAME?
 MANU:NAME test1

    MNAU:EDIT:MODE?
    MNAU:EDIT:MODE ACW

    MANU:ACW:VOLT?
    MANU:ACW:VOLT 2.5
    MANU:ACW:VOLT 3


     
     */
