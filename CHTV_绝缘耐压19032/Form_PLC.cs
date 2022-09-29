using System;
using System.Windows.Forms;

namespace Test_Automation
{
    public partial class Form_PLC : Form
    {
        public Form_PLC()
        {
        　　//参考网址 https://www.cnblogs.com/dathlin/p/7782315.html
     　　　 //在Visual Studio 中的NuGet管理器中可以下载安装，
            //也可以直接在NuGet控制台输入下面的指令安装：Install-Package HslCommunication
            InitializeComponent();
        }
        private void Form_PLC_Load(object sender, EventArgs e)
        {
           config_json.config_json_readall();
            tbPLC_ip.Text = config_json.PLC_IP;
            tbPLC_port.Text = config_json.PLC_Port;
            tbPLC_station.Text = config_json.PLC_Station;
          

        }

        private HslCommunication.ModBus.ModbusTcpNet PLC;
        private string PLC_ip = "192.168.1.5";
        private int PLC_port = 502;
        private byte PLC_station = 1;
        private bool connected=false;

        //开启用于测试的服务器端
        private void btn_modbus_tcp_server_Click(object sender, EventArgs e)
        {

        }



        private void btn_client_connect_Click(object sender, EventArgs e)
        {
            PLC_ip = tbPLC_ip.Text;
            PLC_port = Convert.ToInt16(tbPLC_port.Text);
            PLC_station = Convert.ToByte(tbPLC_station.Text);

            //在使用读写功能之前必须先进行实例化：
            PLC = new HslCommunication.ModBus.ModbusTcpNet(PLC_ip, PLC_port, PLC_station);   // 站号1
          //  PLC=
           //     new HslCommunication.ModBus.ModbusTcpNet("10.7.141.154", 502, Convert.ToByte(1));
            //上面的实例化指定了服务器的IP地址，端口号（一般都是502），以及自己的站号，允许设置为0-255，
            //后面的两个参数有默认值，在实例化的时候可以省略。

            HslCommunication.OperateResult resut= PLC.ConnectServer();
            if (resut.IsSuccess) {
                lb_connect_info.Text = "已成功连接PLC";
                btn_client_connect.Enabled = false;
                btn_client_disconnect.Enabled = true;
                btn_reg_write.Enabled = true;
                btn_reg_read.Enabled = true;
                connected = true;
            }
        }

        private void btn_client_disconnect_Click(object sender, EventArgs e)
        {
            HslCommunication.OperateResult resut = PLC.ConnectClose();
            if (resut.IsSuccess)
            {
                lb_connect_info.Text = "断开连接PLC";
                btn_client_connect.Enabled = true;
                btn_client_disconnect.Enabled = false;
                btn_reg_write.Enabled = false;
                btn_reg_read.Enabled = false;
            }
        }



        private void btn_reg_read_Click(object sender, EventArgs e)
        {
            // lb_reg.Text=   PLC.ReadInt16(tb_reg.Text,1).Content.ToString();

            HslCommunication.OperateResult<byte[]> read = PLC.Read(tb_PLC_StartTest_Register.Text, 1);
            if (read.IsSuccess)
            {
                // 共返回20个字节，每个数据2个字节，高位在前，低位在后
                // 在数据解析前需要知道里面到底存了什么类型的数据，所以需要进行一些假设：
                // 前两个字节是short数据类型
                short value1 = PLC.ByteTransform.TransInt16(read.Content, 0);
                lb_reg.Text = value1.ToString();
                // 接下来的2个字节是ushort类型
                //  ushort value2 = PLC.ByteTransform.TransUInt16(read.Content, 2);
                // 接下来的4个字节是int类型
                //  int value3 = PLC.ByteTransform.TransInt32(read.Content, 4);
                // 接下来的4个字节是float类型
                // string value4 = PLC.ByteTransform.TransString(read.Content, 8,1,Encoding.UTF8);
                // 接下来的全部字节，共8个字节是规格信息
                // string speci = Encoding.ASCII.GetString(read.Content, 12, 8);

                // 已经提取完所有的数据
            }
            else
            {
                MessageBox.Show(read.ToMessageShowString());
            }
        }

        private void btn_reg_write_Click(object sender, EventArgs e)
        {
            if (connected == false) btn_client_connect_Click(null, null);

            HslCommunication.OperateResult result = PLC.Write(tb_PLC_StartTest_Register.Text, Convert.ToInt16(tb_value.Text));
            if (result.IsSuccess)
            {
                lb_reg_write_message.Text = "写入成功：" + tb_value.Text;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (connected == false) btn_client_connect_Click(null, null);
            HslCommunication.OperateResult result = PLC.Write(tb_PLC_StartTest_Register.Text, Convert.ToInt16(2));
            if (result.IsSuccess)
            {
                lb_reg_write_message.Text = "写入成功：2";
            }
        }



        private void btn_save_Click(object sender, EventArgs e)
        {
            //配置文件不存在则创建一个
            if (System.IO.File.Exists(config_json.config_file_name) == false)
                return;
            try
            {
                string json = System.IO.File.ReadAllText(config_json.config_file_name);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                jsonObj["PLC_IP"] = tbPLC_ip.Text;
                jsonObj["PLC_PORT"] = tbPLC_port.Text;
                jsonObj["PLC_STATION"] = tbPLC_station.Text;
                jsonObj["PLC_Adapter_Register"] = tb_PLC_Adapter_Register.Text;
                jsonObj["PLC_Light_Register"] = tb_light_reg.Text;
                jsonObj["PLC_StartTest_Register"] = tb_PLC_StartTest_Register.Text;
                jsonObj["PLC_Tvpass_Register"] = tb_PLC_Tvpass_Register.Text;
                jsonObj["PLC_SN_Register"] = tb_PLC_SN_Register.Text;

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
            //读取配置
            try
            {
                System.IO.StreamReader file = System.IO.File.OpenText("config.json");
                Newtonsoft.Json.JsonTextReader reader = new Newtonsoft.Json.JsonTextReader(file);
                Newtonsoft.Json.Linq.JObject jsonObject =
                                (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.Linq.JToken.ReadFrom(reader);
                if (jsonObject["PLC_IP"] != null)
                    tbPLC_ip.Text = (string)jsonObject["PLC_IP"];
                if (jsonObject["PLC_PORT"] != null)
                    tbPLC_port.Text = (string)jsonObject["PLC_PORT"];
                if (jsonObject["PLC_STATION"] != null)
                    tbPLC_station.Text = (string)jsonObject["PLC_STATION"];
                if (jsonObject["PLC_REGISTER"] != null)
                    tb_PLC_StartTest_Register.Text = (string)jsonObject["PLC_REGISTER"];
                file.Close();
            }
            catch
            {
                //MessageBox.Show("CAN卡配置有误！");
            }
        }

        private void config_json_save()
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (connected == false) btn_client_connect_Click(null, null);
            HslCommunication.OperateResult result = PLC.Write("4097", Convert.ToInt16(1));
            if (result.IsSuccess)
            {
                lb_reg_write_message.Text = "4097写入成功：1";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (connected == false) btn_client_connect_Click(null, null);
            HslCommunication.OperateResult result = PLC.Write(tb_PLC_StartTest_Register.Text, Convert.ToInt16(2));
            if (result.IsSuccess)
            {
                lb_reg_write_message.Text = "写入成功：4";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (connected == false) btn_client_connect_Click(null, null);
            HslCommunication.OperateResult result = PLC.Write(tb_PLC_StartTest_Register.Text, Convert.ToInt16(5));
            if (result.IsSuccess)
            {
                lb_reg_write_message.Text = "写入成功：5";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (connected == false) btn_client_connect_Click(null, null);
            HslCommunication.OperateResult result = PLC.Write("4097", Convert.ToInt16(2));
            if (result.IsSuccess)
            {
                lb_reg_write_message.Text = "4097写入成功：2";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (connected == false) btn_client_connect_Click(null, null);
            HslCommunication.OperateResult result = PLC.Write("4097", Convert.ToInt16(3));
            if (result.IsSuccess)
            {
                lb_reg_write_message.Text = "4097写入成功：3";
            }
        }

        private void btn_modbus_tcp_server_Click_1(object sender, EventArgs e)
        {

        }
    }
}