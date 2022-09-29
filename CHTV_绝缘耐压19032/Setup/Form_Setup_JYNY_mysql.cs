using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_Automation.Setup
{
    public partial class Form_Setup_JYNY_mysql : Form
    { 
        public Form_Setup_JYNY_mysql()
        {
            InitializeComponent();
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        MySql.Data.MySqlClient.MySqlConnection mysql_Conn;
        System.Data.DataSet dataset_ybset;
        MySql.Data.MySqlClient.MySqlDataAdapter mysql_da_ybset;
        System.Data.DataTable dtable_ybset;

        System.Data.DataSet dataset_type_set;
        MySql.Data.MySqlClient.MySqlDataAdapter mysql_da_type_set;
        System.Data.DataTable dtable_type_set;

        private void Form_Setup_JYNY_Load(object sender, EventArgs e)
        {
            config_json.config_json_readall();

            mysql_Conn = new MySql.Data.MySqlClient.MySqlConnection("Database="
               + config_json.mysql_database_name + ";Data Source=" + config_json.Mysql_IP
               + ";User Id=" + config_json.Mysql_User + ";Password=" + config_json.Mysql_Pass + ";charset=utf8");


            try
            {
                mysql_Conn.Open();
                mysql_da_ybset = new MySql.Data.MySqlClient.MySqlDataAdapter("select  * from jyny_仪表设置 order by 名称", mysql_Conn);
                dataset_ybset = new System.Data.DataSet();
                mysql_da_ybset.Fill(dataset_ybset, "wb");
                dtable_ybset = dataset_ybset.Tables["wb"];
                dataGridView1.DataSource = dtable_ybset;


              //  mysql_da_type_set = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT jyny_机型设置.id,jyny_机型设置.机型,jyny_仪表设置.名称 FROM jyny_机型设置,jyny_仪表设置 WHERE jyny_机型设置.设置名称id=jyny_仪表设置.id", mysql_Conn);

                mysql_da_type_set = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT * FROM jyny_机型设置", mysql_Conn);
                dataset_type_set = new System.Data.DataSet();
                mysql_da_type_set.Fill(dataset_type_set, "mo");
                dtable_type_set = dataset_type_set.Tables["mo"];
                dataGridView2.DataSource = dtable_type_set;

                mysql_Conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据读取出错："+ex.Message);
            }


            CheckForIllegalCrossThreadCalls = false;

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
                addmemo("发送指令*idn?到绝缘耐压计");
                Gpt9000_SerialPort.WriteLine("*idn?");
                //System.Threading.Thread.Sleep(1000);

                Gpt9000_Event.WaitOne();

                addmemo("收绝缘耐压计回复：" + gpt9000_result);
                if (gpt9000_result.IndexOf("GPT-9804") == -1)
                {
                    addmemo("耐压计回复不正确");
                    return false;
                }
                addmemo("耐压计回复正确");
                addmemo("发送指令TEST:RET ON到绝缘耐压计");
                Gpt9000_SerialPort.WriteLine("TEST:RET ON");
                return true;
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
            addmemo("仪表回复:" + gpt9000_result);
            if (exe_event) Gpt9000_Event.Set();
        }
        bool exe_event = false;


        private void btn_portopen_Click(object sender, EventArgs e)
        {
            if (serial_inited == false) { 
            Gpt9000_Event = new AutoResetEvent(false);
            exe_event = true;
            Task.Run(() =>
            {
                if (serial_init() == false) return;
            //    groupBox1.Enabled = true;
                btn_portclose.Enabled = true;
            });
            }
        }

        private void btn_portclose_Click(object sender, EventArgs e)
        {
            Gpt9000_SerialPort.Close();
          //  groupBox1.Enabled = false;
            btn_portopen.Enabled = true;
            serial_inited = false;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (dataset_ybset.HasChanges())
            {
                MySql.Data.MySqlClient.MySqlCommandBuilder commondbuilder = new MySql.Data.MySqlClient.MySqlCommandBuilder(mysql_da_ybset);
                mysql_da_ybset.Update(dataset_ybset.Tables["wb"]);
                dataset_ybset.Tables["wb"].AcceptChanges();
                MessageBox.Show("保存成功！");
            }
        }

        string 名称, 交流耐压电压, 交流耐压漏电流最大值, 交流耐压漏电流最小值, 交流耐压漏电流修正值, 交流耐压测试时间, 交流耐压测试频率;
        string 绝缘电阻电压, 绝缘电阻最大值, 绝缘电阻最小值, 绝缘电阻修正值, 绝缘电阻测试时间;

        private void btn_readmes2_Click(object sender, EventArgs e)
        {
            if (serial_inited == false)
            {
                MessageBox.Show("请先打开端口");
                return;
            }
            commmand = "MEAS2?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void btn_readmes1_Click(object sender, EventArgs e)
        {
            if (serial_inited == false)
            {
                MessageBox.Show("请先打开端口");
                return;
            }
            commmand = "MEAS1?";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void btn_teston_Click(object sender, EventArgs e)
        {
            if (serial_inited == false)
            {
                MessageBox.Show("请先打开端口");
                return;
            }
            commmand = "FUNC:TEST ON";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
        }

        private void btn_set_Click(object sender, EventArgs e)
        {
            if (serial_inited == false) {
                MessageBox.Show("请先打开端口");
                return;
            }
            string sql = "select  * from jyny_仪表设置 where id="+lb_id.Text;

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, mysql_Conn);

            try
            {
                mysql_Conn.Open();

                MySql.Data.MySqlClient.MySqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())          //循环读取结果集
                {
                    名称 = dr["名称"].ToString();
                    交流耐压电压 = dr["交流耐压电压"].ToString();
                    交流耐压漏电流最大值 = dr["交流耐压漏电流最大值"].ToString();
                    交流耐压漏电流最小值 = dr["交流耐压漏电流最小值"].ToString();
                    交流耐压漏电流修正值 = dr["交流耐压漏电流修正值"].ToString();
                    交流耐压测试时间 = dr["交流耐压测试时间"].ToString();
                    交流耐压测试频率 = dr["交流耐压测试频率"].ToString();

                    绝缘电阻电压 = dr["绝缘电阻电压"].ToString();
                    绝缘电阻最大值 = dr["绝缘电阻最大值"].ToString();
                    绝缘电阻最小值 = dr["绝缘电阻最小值"].ToString();
                    绝缘电阻修正值 = dr["绝缘电阻修正值"].ToString();
                    绝缘电阻测试时间 = dr["绝缘电阻测试时间"].ToString();
                }
                else {
                    MessageBox.Show("数据为空！");
                }

                dr.Close();
                mysql_Conn.Close();
            }
            catch
            {
                MessageBox.Show("mysql数据库连接失败");
            }

            addmemo("保存ACW参数");
            exe_event = false;
            commmand = "MAIN: FUNC MANU";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            // Gpt9000_Event.WaitOne();

            commmand = "MANU:STEP 1";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            // Gpt9000_Event.WaitOne();

            commmand = "MANU:NAME MANU_ACW";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU: EDIT: MODE ACW";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);


            commmand = "MANU:ACW:VOLT " + 交流耐压电压;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:ACW:FREQ " + 交流耐压测试频率;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:ACW:CHIS " + 交流耐压漏电流最大值;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:ACW:CLOS " + 交流耐压漏电流最小值;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:ACW:REF " + 交流耐压漏电流修正值;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:ACW:TTIM " + 交流耐压测试时间;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            addmemo("保存IR参数");
            exe_event = false;
            commmand = "MAIN: FUNC MANU";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            // Gpt9000_Event.WaitOne();

            commmand = "MANU:STEP 2";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            // Gpt9000_Event.WaitOne();

            commmand = "MANU:NAME MANU_IR";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:EDIT:MODE IR";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);


            commmand = "MANU:IR:VOLT " + 绝缘电阻电压;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:IR:RHIS " + 绝缘电阻最大值;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:IR:RLOS " + 绝缘电阻最小值;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:IR:REF " + 绝缘电阻修正值;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            commmand = "MANU:IR:TTIM " + 绝缘电阻测试时间;
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);



            addmemo("保存AUTO参数");
            exe_event = false;
            commmand = "MAIN:FUNC AUTO";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            // Gpt9000_Event.WaitOne();

            commmand = "AUTO:STEP 1";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
            // Gpt9000_Event.WaitOne();

            commmand = "AUTO:NAME AUTO_ACWIR";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);

            //清空原有步骤
            for (int i = 1; i <= 16; i++)
            {
                commmand = "AUTO:PAGE:DEL 1";
                addmemo(commmand);
                Gpt9000_SerialPort.WriteLine(commmand);
            }
           
            commmand = "AUTO:EDIT:ADD 001";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
           
            commmand = "AUTO:EDIT:ADD 002";
            addmemo(commmand);
            Gpt9000_SerialPort.WriteLine(commmand);
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            mysql_Conn.Open();

            mysql_da_type_set = new MySql.Data.MySqlClient.MySqlDataAdapter("SELECT * FROM jyny_机型设置 where 机型 like '%"+textBox1.Text+"%'", mysql_Conn);
            dataset_type_set = new System.Data.DataSet();
            mysql_da_type_set.Fill(dataset_type_set, "mo");
            dtable_type_set = dataset_type_set.Tables["mo"];
            dataGridView2.DataSource = dtable_type_set;

            mysql_Conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataset_type_set.HasChanges())
            {
                MySql.Data.MySqlClient.MySqlCommandBuilder commondbuilder = new MySql.Data.MySqlClient.MySqlCommandBuilder(mysql_da_type_set);
                mysql_da_type_set.Update(dataset_type_set.Tables["mo"]);
                dataset_type_set.Tables["mo"].AcceptChanges();
                MessageBox.Show("保存成功！");
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            lb_type.Text = dataGridView2.CurrentRow.Cells["机型"].Value.ToString();
            lb_id.Text= dataGridView2.CurrentRow.Cells["设置名称id"].Value.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
