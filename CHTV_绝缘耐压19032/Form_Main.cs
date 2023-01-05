using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace Test_Automation
{
    public partial class Form_Main : Form
    {
        int first_pass_num = 0; //一次测试通过
        int pass_num = 0;     //测试通过数量，包括重测
        int ng_num = 0;       //fail 数量
        int faulttest_num = 0;  //误测数量
        int read_sn_times = 0;  //重读sn次数
        int test_times = 1;   //测试次数

        System.Timers.Timer timer_plc;  //间断读取PLC数据
        System.Timers.Timer TestTimeTimer;  //显示测试时间
        System.Timers.Timer Timer_addapteron_check;  //检查对接头位置
        System.Timers.Timer Timer_addapteroff_check;  //检查对接头位置
        System.Timers.Timer timer_read_sn;
        System.Timers.Timer Timer_Sendtomes;

        DateTime starttime;//测试开始时间
        bool plc_inited = false;
        bool serial_inited = false;
        HslCommunication.ModBus.ModbusTcpNet PLC;
        string logfile = "";  //保存日志文件 文件名
        int test_item_number = -1; //测试项目数量
        string[] test_item_names = new string[3];

        public Form_Main()
        {
            InitializeComponent();
        }
        private bool Clear_TV_SN()
        {
            if (plc_inited)
            for (int i = 0; i < 50; i++)
            {
                PLC.Write(Convert.ToString(5022 + i), 0);
            }
            return true;
        }
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private void Form1_Load(object sender, EventArgs e)
        {
            SOFT_VER.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            System.IO.DirectoryInfo dir_config = new DirectoryInfo(config_json.save_config_file_dir);
            if (dir_config.Exists == false) dir_config.Create();
            System.IO.DirectoryInfo dir_debug = new DirectoryInfo(config_json.debug_dir);
            if (dir_debug.Exists == false) dir_debug.Create();
            System.IO.DirectoryInfo dir_log = new DirectoryInfo(config_json.log_dir);
            if (dir_log.Exists == false) dir_log.Create();

            config_json.config_json_readall();
            ts1.Text = "当前使用数据库：" + config_json.mysql_database_name 
                + " 测试表：" + config_json.mysql_testdata_table
                 + " 统计表：" + config_json.mysql_yielddata_table;

            ts_username.Text = config_json.login_name;
            logfile = DateTime.Now.ToString("yyyyMMdd HHmmss") + ".txt";
            addmemo("测试软件代号：" + tc_softcode.Text);
            addmemo("测试软件版本：" + SOFT_VER.Text);
            addmemo("读取配置文件：" + config_json.config_file_name);

            if (File.Exists(config_json.config_file_name) == false)
            {
                using (StreamWriter sw = File.CreateText(config_json.config_file_name))
                {
                    sw.WriteLine("{}");
                    sw.Close();
                }
                MessageBox.Show("已创建新的配置文件，请根据实际情况更新相关数据。");
                //Form_Workstation_JYNY_Set fset = new Form_Workstation_JYNY_Set();
               // fset.ShowDialog();
            }

            window_init();
            
            addmemo("检查日志目录是否存在：" + config_json.save_config_file_dir);
            check_logdir();

            TextBox.CheckForIllegalCrossThreadCalls = false;

            lb_title.BackColor = Color.Transparent;  //设备标题文字背景透明
            lb_title.Parent = pictureBox1;//将pictureBox1设为标签的父控件
            lb_title.ForeColor = Color.Blue;
            lb_title.Location = new Point(200, 50);//重新设定标签的位置，这个位置时相对于父控件的左上角

           // SkinEngine.SkinFile = "DiamondOlive.ssk";//加皮肤step2
           // SkinEngine.Active = true;//加皮肤step3

            TestTimeTimer = new System.Timers.Timer(1000);  //每秒刷新测试时间
            TestTimeTimer.Elapsed += new System.Timers.ElapsedEventHandler(TestTimeTimer_Tick);//到时间的时候执行事件；
            TestTimeTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            TestTimeTimer.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；

            timer_plc = new System.Timers.Timer(200);
            timer_plc.Elapsed += new System.Timers.ElapsedEventHandler(Timer_PLC_Tick);
            timer_plc.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            timer_plc.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；

            Timer_addapteron_check = new System.Timers.Timer(200);
            Timer_addapteron_check.Elapsed += new System.Timers.ElapsedEventHandler(Timer_addapteron_check_Tick);
            Timer_addapteron_check.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            Timer_addapteron_check.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；

            Timer_addapteroff_check = new System.Timers.Timer(200);
            Timer_addapteroff_check.Elapsed += new System.Timers.ElapsedEventHandler(Timer_addapteroff_check_Tick);
            Timer_addapteroff_check.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            Timer_addapteroff_check.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；

            timer_read_sn = new System.Timers.Timer(200);
            timer_read_sn.Elapsed += new System.Timers.ElapsedEventHandler(timer_read_sn_Tick);
            timer_read_sn.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            timer_read_sn.Enabled = false;//是否执行System.Timers.Timer.Elapsed事件；

            Timer_Sendtomes = new System.Timers.Timer(1000);
            Timer_Sendtomes.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Sendtomes_Tick);
            Timer_Sendtomes.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；

            string result = Class_Mysql.check_and_start_mysql_service();
            addmemo("检查和启动服务："+result, 0);

            addmemo( "检查数据库及表："+ Class_Mysql.check_mysql());

            con = new MySql.Data.MySqlClient.MySqlConnection(
                          "Database=" + config_json.mysql_database_name
                          + ";Data Source=" + config_json.Mysql_IP
                          + ";User Id=" + config_json.Mysql_User
                          + ";Password=" + config_json.Mysql_Pass+";charset=utf8"
                      );
            addmemo("读取生产数据");
            int[] temp1 = Class_Mysql.get_produce_data_cn();
            if (temp1 != null)
            {
                first_pass_num = temp1[0];
                pass_num = temp1[1];
                faulttest_num = temp1[2];
                ng_num = temp1[3];
                tongji_update();
                addmemo("OK");
            }
            else
            {
                addmemo("Fail");
            }

            if (config_json.Workstationid == "")
            {
                addmemo("当前设置的工作站为空，请先设置工作站名称");
            }
            else {
                addmemo("当前设置的工作站是：" + config_json.Workstationid);
            }
           

            if (cb_autorun.Checked) btn_start_Click(null, null);
        }
        private void TestTimeTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                TimeSpan span = DateTime.Now - starttime;
                LabelTime.Text = span.TotalSeconds.ToString("0");
            }
            catch { }
            if (LabelTime.Text == config_json.TestTimeOutSeconds)
            {   //测试超时
                TestTimeOut();
            }
        }

        private void TestTimeOut()
        {
            addmemo("测试超时");
        }

        private void check_logdir()
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(config_json.save_config_file_dir);
            if (di.Exists == false)
                try
                {
                    di.Create();
                    addmemo("日志目录创建成功");
                }
                catch
                {
                    MessageBox.Show("日志目录创建失败，请确认系统有D盘！");
                    return;
                }
            else
            {
                addmemo("目录存在");
            }
        }
      
        private void Timer_PLC_Tick(object sender, ElapsedEventArgs e)
        {
            if (plc_inited == false)  return;
            plc_reg_5000_value = Convert.ToInt16(plc_read(config_json.PLC_StartTest_Register));
            if (plc_reg_5000_value == 0)
            {
                lb_mes_code.Text = "0";
                lb_mes_status.Text = "工装板未就位";
                timer_plc.Enabled = true;
                return;
            }

            lb_mes_code.Text = plc_reg_5000_value.ToString();
            System.Threading.Thread.Sleep(100);
          
            tb_sn.Text = Get_TV_SN();
            tb_type.Text = Get_TV_Type();
            addmemo("获取到机型：" + tb_type.Text);
            addmemo("获取到SN:" + tb_sn.Text);

            switch (plc_reg_5000_value)
            {
                case 1:
                    lb_mes_status.Text = "NG";
                    break;
                case 2:
                    lb_mes_status.Text = "NG";
                    break;
                case 4:
                    lb_mes_status.Text = "空板";
                    break;
                case 8:
                    lb_mes_status.Text = "流程异常";
                    break;
                case 16:
                    lb_mes_status.Text = "条码异常";
                    break;
                default:
                    break;
            }

            if (lb_mes_code.Text==config_json.ShakeHand_OK_Code) {
                lb_mes_status.Text = "OK";
            }
            addmemo("获取到mes信息：5000=" + plc_reg_5000_value + " " + lb_mes_status.Text);

            if (lb_mes_status.Text == "OK")
            {
                lb_mes_code.Text = "工装板已就位";
                read_sn_times = 0;
                if (tb_sn.Text.Length < 5)
                {
                    light_red_on();
                    addmemo("获取SN失败，1秒后再试");
                    timer_read_sn.Enabled = true;
                }
                else
                {
                    light_green_on();
                    tb_memo.Text = "";
                    test_times = 1;
                    lb_test_times.Text = test_times.ToString();
                    adapter_on();
                }
                
            }
            else
            {
                plc_write(config_json.PLC_StartTest_Register, "0");

                timer_plc.Enabled = true;
                if (tb_sn.Text != ""&& tb_sn.Text != "-3") {
                    try
                    {
                        //todo 保存本地数据 Mysql_Class_Laiya.DataInsert
                        data.WORKSTATIONID = lb_workstationid.Text;
                        data.testdatetime = DateTime.Now.ToString();
                        data.MO = tb_type.Text;
                        data.SN = tb_sn.Text;
                        data.memo = "MES状态:" + lb_mes_status.Text + "，未进行测试";
                        data.SOFT_VER = SOFT_VER.Text;
                        addmemo(Class_Mysql.DataInsert_jyny(data));
                    }
                    catch { }
                }
            }
        }
        void Test_Begin()
        {
            testing = true;
            starttime = DateTime.Now;
            TestTimeTimer.Enabled = true;
            tb_mesreply.Text = "";
            tb_memo.Text = "";
            Sendtomes_failtimes = 0;
            lb_result.Text = "测试中";
            lb_result.BackColor = System.Drawing.Color.Blue;
            addapteron_check_times = 0;
            addapteroff_check_times = 0;
           // test_times = 1;

            test_result = "";
            if (SerialPort.IsOpen == false)
                try
                {
                    SerialPort.Open();
                }
                catch
                {
                    addmemo("串口打开失败");
                    return;
                }
            addmemo("发送测试指令("+ Serial_Command.启动测试 + ")，等待测试结果");
            SerialPort.WriteLine(Serial_Command.启动测试);
            if (Device_Event.WaitOne(20000))
            {
                addmemo("收到仪表回复：" + test_result);
                test_result_process();
            }
            else
            {
                addmemo("仪表响应超时：20秒");
            }
        }

        private void test_result_process() {
            //AC,+3.000000E+03,+1.400000E-04,34,IR,+9.910000E+37,+9.910000E+37,112
            string[] test_results = test_result.Split(',');
            if (test_results.Length < 8)
            {
                addmemo("接收数据有误");
               // MessageBox.Show("接收数据有误");
                return;
            }
            else
            {
                try
                {
                    if (test_results[0] == "AC")
                    {
                       // tb_acw_vol.Text = Convert.ToDouble(test_results[1]).ToString("0");
                        tb_acw_curr.Text = (Convert.ToDouble(test_results[2])*1000).ToString("0.000");
                       // tb_acw_time.Text = test_results[3];
                        lb_acw_code.Text = test_results[3];
                    }
                    if (test_results[4] == "IR")
                    {
                        //tb_ir_vol.Text = (Convert.ToDouble(test_results[5]) / (1E+37)).ToString();
                      //  tb_ir_vol.Text = (Convert.ToDouble(test_results[5]) ).ToString("0");
                       // tb_ir_res.Text =( Convert.ToDouble(test_results[6]) / (1E+37)).ToString();
                        tb_ir_res.Text = (Convert.ToDouble(test_results[6])/1000000).ToString();
                        // tb_ir_time.Text = test_results[8];
                        lb_ir_code.Text = test_results[7];
                    }

                    //设置输出带时间时在第二次收不到结果回复
                    //if (test_results[0] == "AC")
                    //{
                    //    tb_acw_vol.Text = Convert.ToDouble(test_results[1]).ToString("0");
                    //    tb_acw_curr.Text = Convert.ToDouble(test_results[2]).ToString("0.000");
                    //    tb_acw_time.Text = test_results[3];
                    //    lb_acw_code.Text = test_results[4];
                    //}
                    //if (test_results[5] == "IR")
                    //{
                    //    tb_ir_vol.Text = (Convert.ToDouble(test_results[6]) / (1E+37)).ToString();
                    //    tb_ir_res.Text = Convert.ToDouble(test_results[7]).ToString();
                    //    tb_ir_time.Text = test_results[8];
                    //    lb_ir_code.Text = test_results[9];
                    //}
                }
                catch { }
                if (lb_acw_code.Text.Contains("116")) { lb_acw.Text = "PASS"; } else lb_acw.Text = "FAIL";
                if (lb_ir_code.Text.Contains("116")) { lb_ir.Text = "PASS"; } else lb_ir.Text = "FAIL";

                 checkTestResult(lb_acw.Text, lb_ir.Text);  //todo 测试结果处理
            }

        }


        private void timer_read_sn_Tick(object sender, ElapsedEventArgs e)
        {
            timer_read_sn.Enabled = false;
            tb_type.Text = Get_TV_Type();
            tb_sn.Text = Get_TV_SN();
            if (tb_sn.Text.Length<5)
            {
                light_red_on();
                read_sn_times++;
                addmemo("获取SN失败，重试:"+ read_sn_times.ToString());
                timer_read_sn.Enabled = true;
            }
            else
            {
                addmemo("获取SN成功");
                addmemo("机型：" + tb_type.Text);
                addmemo("SN:" + tb_sn.Text);
                light_green_on();
              
                adapter_on();
            }
        }


        MySql.Data.MySqlClient.MySqlConnection con;
        void plc_init()
        {
            if (config_json.PLC_Used == false)
            {
                addmemo("设置中未启用PLC");
                return;
            } 
            if (plc_inited == false)
            {
                addmemo("PLC连接...");
                PLC = new HslCommunication.ModBus.ModbusTcpNet(config_json.PLC_IP, 502, 1);
                HslCommunication.OperateResult resut = PLC.ConnectServer();
                if (resut.IsSuccess)
                {
                    plc_inited = true;
                    addmemo("PLC连接成功！");
                }
                else
                {
                    addmemo("PLC连接不成功");
                }
            }
            else
            {
                addmemo("PLC已连接成功！");
            }

        }
        bool serial_init()
        {
            //打开串口，连接绝缘耐压仪
            if (serial_inited == false)
            {
                addmemo("开始连接绝缘耐压计");
                try
                {
                    SerialPort = new System.IO.Ports.SerialPort();
                    SerialPort.PortName = config_json.Jyny_Device_PortName;
                    SerialPort.BaudRate = Convert.ToInt16(config_json.Jyny_Device_BaudRate);
                    SerialPort.DataBits = Convert.ToInt16(config_json.Jyny_Device_DataBits);

                    addmemo("耐压计端口号："+SerialPort.PortName+" 波特率："+ SerialPort.BaudRate);

                    if (config_json.Jyny_Device_StopBits == "0") SerialPort.StopBits = System.IO.Ports.StopBits.None;
                    if (config_json.Jyny_Device_StopBits == "1") SerialPort.StopBits = System.IO.Ports.StopBits.One;
                    if (config_json.Jyny_Device_StopBits == "1.5") SerialPort.StopBits = System.IO.Ports.StopBits.OnePointFive;
                    if (config_json.Jyny_Device_StopBits == "2") SerialPort.StopBits = System.IO.Ports.StopBits.Two;

                    if (config_json.Jyny_Device_Parity == "None") SerialPort.Parity = System.IO.Ports.Parity.None;
                    if (config_json.Jyny_Device_Parity == "Even") SerialPort.Parity = System.IO.Ports.Parity.Even;
                    if (config_json.Jyny_Device_Parity == "Odd") SerialPort.Parity = System.IO.Ports.Parity.Odd;

                    SerialPort.DataReceived += serialPort1_DataReceived;

                    if (SerialPort.IsOpen == false) SerialPort.Open();
                    else
                    {
                        MessageBox.Show("串口被占用!");
                        return false;
                    }
                }
                catch
                {
                    addmemo("绝缘耐压仪连接出错,请检查端口是否被占用!");
                    return false;
                }
                serial_inited = true;
                if (false == Device_Check())
                {
                    //addmemo("串口连接的不是绝缘耐压仪！");
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


        //todo 测试开始按钮
        private void btn_start_Click(object sender, EventArgs e)
        {
            testing = true;
            plc_init();
            if (serial_init() == false) return;
            test_result = "";
            btn_start.Enabled = false;
            btn_stop.Enabled = true;

            if (plc_inited == true )
            {
                light_green_on();
                timer_plc.Enabled = true;
                addmemo("已启用自动测试，等待机器到位");
            }
            else {
                addmemo("未启用PLC，当前为手动测试");
            }
            this.ActiveControl = tb_sn;
        }
        string command = "";
        AutoResetEvent Device_Event = new AutoResetEvent(false);
        private bool Device_Check()
        {
            addmemo("发送指令*idn?到绝缘耐压计");
            SerialPort.WriteLine("*idn?");//Chroma,19032-P,19032P101788,6.04 
            //System.Threading.Thread.Sleep(1000);

            if (Device_Event.WaitOne(2000) == false) {
                addmemo("绝缘耐压计未正常响应，请检查仪器是否开机，相关连线或设置是否正常！");
                return false;
            }

            addmemo("收绝缘耐压计回复：" + test_result);
            if (!test_result.Contains("19032"))
            {
                addmemo("耐压计回复不正确,请检查是否连接的测试仪是Chroma 19032");
                return false;
            }
            addmemo("耐压计回复正确");
            command = "SAFE:RES:AREP ON";
            addmemo("发送指令"+command+"到绝缘耐压计,设置为自动回报测试结果");
            SerialPort.WriteLine(command);

            test_result = "";
            command = Serial_Command.设定自动测试回报资料1;
            addmemo("获取绝缘计自动回报格式("+ command + ")");
            SerialPort.WriteLine(command);
           Thread.Sleep(500);

            test_result = "";
            command = "SOUR:SAFE:RES:ASAV ON";
           addmemo("设置下次开机仍自动回报（"+ command + "）");
            SerialPort.WriteLine(command);

            test_result = "";
            command = "SOUR:SAFE:STEP 1:MODE?";
            addmemo("获取测试1测试类型（" + command + "）");
            SerialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            if (!test_result.Contains("AC")) {
                addmemo("测试步骤1不为AC，请将1设置为AC");
                MessageBox.Show("测试步骤1不为AC，请将1设置为AC");
                return false;
            }

            test_result = "";
            command = "SOUR:SAFE:STEP 2:MODE?";
            addmemo("获取测试2测试类型（" + command + "）");
            SerialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            if (!test_result.Contains("IR"))
            {
                addmemo("测试步骤2不为IR，请将2设置为IR");
                MessageBox.Show("测试步骤2不为IR，请将2设置为IR");
                return false;
            }
            test_result = "";
            command = "SOUR:SAFE:STEP 1:SET?";
            addmemo("查询AC测试详细设置（" + command + "）");
            SerialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);

            test_result = "";
            command = "SOUR:SAFE:STEP 1:AC:LEV?";
            addmemo("获取AC测试电压（" + command + "）");
            SerialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            try
            {
                tb_acw_vol.Text = Convert.ToDouble(test_result).ToString();
            }
            catch {
                try
                {
                    tb_acw_vol.Text = Convert.ToDouble(test_result.Split('\n')[1].Trim()).ToString();
                }
                catch 
                {
                    addmemo("收绝缘耐压计回复数据不正确");
                }
               
            }



            test_result = "";
            command = "SOUR:SAFE:STEP 1:AC:TIME:TEST?";
            addmemo("获取AC测试时间（" + command + "）");
            SerialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            tb_acw_time.Text =Convert.ToDouble(test_result).ToString();

            test_result = "";
            command = "SOUR:SAFE:STEP 2:SET?";
            addmemo("查询IR测试详细设置（" + command + "）");
            SerialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);

            test_result = "";
            command = "SOUR:SAFE:STEP 2:IR:LEV?";
            addmemo("获取IR测试电压（" + command + "）");
            SerialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            tb_ir_vol.Text = Convert.ToDouble(test_result).ToString();

            test_result = "";
            command = "SOUR:SAFE:STEP 2:IR:TIME:TEST?";
            addmemo("获取IR测试时间（" + command + "）");
            SerialPort.WriteLine(command);
            Device_Event.WaitOne();
            addmemo("收绝缘耐压计回复：" + test_result);
            tb_ir_time.Text = Convert.ToDouble(test_result).ToString();

            return true;
        }

        //todo timer_readdata_sys_Tick
        //string acw_result = null;  //用于保存测试结果
        //string ir_result = null;
   
        string memo = "";
        /// <summary>
        /// todo StopTest()
        /// </summary>
        private void StopTest()
        {
            if (config_json.PLC_Used) {
                light_red_on();
                timer_read_sn.Enabled = false;

                timer_plc.Enabled = false;
                btn_start.Enabled = true;
                btn_stop.Enabled = false;

                adapter_off();
                addmemo("夹具已收回");
            }
            addmemo("测试已停止");
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            addmemo("人工点击停止按钮");
            testing = false;
            btn_start.Enabled = true;
            StopTest();
        }

        string test_result = "";
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(100);
            test_result = SerialPort.ReadExisting();
            Device_Event.Set();
        }

        int plc_reg_5000_value;

        public void adapter_on()
        {
            addapteron_check_times = 0;
            addmemo("当前产品SN： " + tb_sn.Text + "");
            if (config_json.PLC_Used)
            {
                addmemo("伸出夹具");
                plc_write(config_json.PLC_Adapter_Register, "4");
                if (config_json.Adapter_on_check)
                {
                    Timer_addapteron_check.Enabled = true;  //检查夹具是否到位
                }
                else
                {
                    //等待夹具伸出到位
                    System.Threading.Thread.Sleep(1000);
                    Test_Begin();
                }
            }
            else {
                Test_Begin();
            }
      
        }

     


        int addapteron_check_times = 0;
        private void Timer_addapteron_check_Tick(object sender, ElapsedEventArgs e)
        {
            Timer_addapteron_check.Enabled = false;
            try
            {
                string plc_value = plc_read(config_json.PLC_Adapter_Register);

                if (plc_value == "1")
                {
                    addapteron_check_times = 0;

                    addmemo("夹具已到位，启动测试",0);
                    //PLC.Write(plc_reg_gasline, Convert.ToInt16(0));  //从4097寄存器读到1后，先清0，再启动测试
                    Test_Begin();
                }
                else
                {
                    if (addapteron_check_times == 5)
                    {
                        addmemo("夹具检测多次未到位,测试已停止！");
                        light_red_on();
                        btn_start.Enabled = true;
                        MessageBox.Show("夹具检测多次未到位,测试已停止！");
                        return;
                    }
                    addapteron_check_times++;
                    addmemo("夹具合上未到位，" + config_json.PLC_Adapter_Register + "=" + plc_value.ToString());
                    Timer_addapteron_check.Enabled = true;
                }
            }
            catch
            {
                addmemo("夹具检测到位出错！");
            }
        }

        public void adapter_off()
        {
            if (config_json.PLC_Used)
            {
                addmemo("收回夹具");
                plc_write(config_json.PLC_Adapter_Register, "5");
                if (config_json.Adapter_off_check)
                    Timer_addapteroff_check.Enabled = true;
            }
           
        }

        int addapteroff_check_times = 0;
        private void Timer_addapteroff_check_Tick(object sender, ElapsedEventArgs e)
        {
            if (plc_inited == false) return;
            Timer_addapteroff_check.Enabled = false;
            try
            {
                string plc_value = plc_read(config_json.PLC_Adapter_Register);

                if (plc_value == "2")
                {
                    addapteroff_check_times = 0;
                    addmemo("夹具已松开！");
                }
                else
                {
                    if (addapteroff_check_times == 5)
                    {
                        addmemo("夹具检测多次未松开,测试已停止！");
                        light_red_on();
                        btn_start.Enabled = true;
                        MessageBox.Show("夹具检测多次未到位,测试已停止！");
                        return;
                    }
                    addapteroff_check_times++;
                    addmemo("夹具松开未到位，" + config_json.PLC_Adapter_Register + "=" + plc_value.ToString());
                    Timer_addapteron_check.Enabled = true;
                }
            }
            catch
            {
                addmemo("夹具检测到位出错！");
            }
        }

        string plc_read(string reg)
        {
            if (plc_inited)
            {
                HslCommunication.OperateResult<byte[]> read = PLC.Read(reg, 1);
                if (read.IsSuccess)
                {
                    short value1 = PLC.ByteTransform.TransInt16(read.Content, 0);
                    return value1.ToString();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// ASCII码转Str
        /// </summary>
        /// <param name="asciiCode"></param>
        /// <returns></returns>
        public static string AsciiToStr(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                throw new Exception("ASCII Code is not valid.");
            }
        }

        /// <summary>
        /// 获取电视机机型信息，读取PLC寄存器5002~5021
        /// </summary>
        String[] TV_type = new String[20];
        private string Get_TV_Type()
        {
            string plc_reg_5002 = config_json.PLC_Type_Register;
            HslCommunication.OperateResult<byte[]> read = PLC.Read(plc_reg_5002, 1);

            if (read.IsSuccess)
            {
                for (int i = 0; i < 20; i++)
                {
                    HslCommunication.OperateResult<byte[]> reg_value = PLC.Read(Convert.ToString(5002 + i), 1);
                    short value1 = PLC.ByteTransform.TransInt16(reg_value.Content, 0);

                    TV_type[i] = AsciiToStr(value1);
                }
                return string.Join("", TV_type);
            }
            else
            {
                MessageBox.Show(read.ToMessageShowString());
                return "错误";
            }
        }

        /// <summary>
        /// 获取电视机SN，读取PLC寄存器5022~5071
        /// </summary>
        String[] TV_SN = new String[50];

        private string Get_TV_SN()
        {
            HslCommunication.OperateResult<byte[]> read = PLC.Read("5022", 1);

            if (read.IsSuccess)
            {
                for (int i = 0; i < 50; i++)
                {
                    HslCommunication.OperateResult<byte[]> reg_value = PLC.Read(Convert.ToString(5022 + i), 1);
                    short value = PLC.ByteTransform.TransInt16(reg_value.Content, 0);

                    TV_SN[i] = AsciiToStr(value);
                }
                return string.Join("", TV_SN);


            }
            else
            {
                MessageBox.Show(read.ToMessageShowString());
                return "错误";
            }
        }

        private void btn_down_Click(object sender, EventArgs e)
        {
            addmemo("下压夹具");
            plc_write(config_json.PLC_Adapter_Register, "4");
            
        }

        private void btn_up_Click(object sender, EventArgs e)
        {
            addmemo("松开夹具");
            plc_write(config_json.PLC_Adapter_Register, "5");
        }
        bool testing = false;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (testing == true)
            {
                DialogResult dr =
                        MessageBox.Show("正在测试中，请确认真的要退出吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    user_logout();
                    if (SerialPort.IsOpen) SerialPort.Close();
                    e.Cancel = false;
                }
            }
           
        }

        private void btn_manual_test_Click(object sender, EventArgs e)
        {
            addmemo("开始手工测试");
            plc_init();
            if (serial_init() == false) return;
            System.Threading.Thread.Sleep(1000);
            Test_Begin();
        }

        /// <summary>
        /// todo 测试结果处理 checkTestResult
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        private void checkTestResult(string str1, string str2)
        {
            if (str1 == "PASS" && str2 == "PASS")  //测试通过
            {
                Test_OK();
            }
            else  //测试不通过
            {
                Test_NG();
            }
        }
        private void checkTestResult()
        {
            if (lb_acw.Text == "FAIL" || lb_ir.Text == "FAIL"|| lb_gb.Text == "FAIL")  
            {
                Test_NG();//测试不通过
            }
            else  //测试通过
            {
                Test_OK();
            }
        }

        void Test_OK() {
            lb_result.Text = "OK";
            lb_result.BackColor = System.Drawing.Color.LimeGreen;
            //两项都为PASS，亮绿灯

            light_green_on();

            if (test_times == 1)  //一次通过
            {
                first_pass_num++;
                pass_num++;   //合格品加1
                addmemo("测试一次通过", 0);
            }

            if (test_times == 2)
            {   //复检通过
                pass_num++;
                faulttest_num++;
                addmemo("复测1次通过", 2);

            }

            if (test_times == 3)  //人工复测通过
            {
                pass_num++;
                faulttest_num++;
                addmemo("复测2次通过(人工复测)", 2);
            }
            sendtomes(); 
            Test_End();
        }
        void Test_NG() {
            System.Threading.Thread.Sleep(1000);
            SerialPort.WriteLine(Serial_Command.停止测试);
            lb_result.Text = "NG";
            light_red_on();
            addmemo("测试不通过", 3);
            lb_result.BackColor = System.Drawing.Color.Red;

            if (test_times == 0||test_times==1)
            {   //  失败重测
                if (cb_prefailsnotomes.Checked)
                {
                    addmemo("系统设置前两次NG不发MES");
                }
                else
                {
                    addmemo("系统设置前两次NG发送MES");
                    sendtomes(); // 打了勾前两次失败不发MES
                }

                test_times=2;
                lb_test_times.Text = test_times.ToString();

                addmemo("失败重测", 3);

                savetodata();
                savetolog();
                Test_Begin();
                return;
            }
            if (test_times == 2)
            {   //  失败重测
                if (cb_prefailsnotomes.Checked)
                {
                    addmemo("系统设置前两次NG不发MES");
                }
                else
                {
                    addmemo("系统设置前两次NG发送MES");
                    sendtomes(); ; // 打了勾前两次失败不发MES
                }

                savetodata();
                savetolog();
                StopTest();
                btn_test_afterNG.Enabled = true;
                addmemo("两次测试失败，待人工处理", 3);
                MessageBox.Show("两次测试失败，待人工处理,点击“两次NG后人工测试”");
                btn_test_afterNG.Focus();
                return;
            }

            if (test_times == 3)  //人工复测不通过
            {
                addmemo("人工复测失败", 3);

                DialogResult dr = MessageBox.Show("是否发送测试结果NG到MES？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    addmemo("发送结果NG到MES");
                    sendtomes();  // 打了勾前两次失败不发MES
                }
                else
                {
                    addmemo("未发送结果NG到MES");
                }
                Test_End();
                return;
            }
        }

        private void Test_End()
        {
            testing = false;
            btn_test_afterNG.Enabled = false;
            savetodata();
            savetolog();
            tb_sn.Text = "";
            TimeSpan ts = DateTime.Now - starttime;
            LabelTime.Text = ts.TotalSeconds.ToString("0.0");

            if (lb_result.Text == "OK") {
                if (test_times == 1)
                {
                    first_pass_num++;
                    pass_num++;
                }
                else
                {
                    faulttest_num++;
                    pass_num++;

                }
            } else
            {
                ng_num++;
            }

            tongji_update();
            addmemo(update_pronum());
            if (config_json.PLC_Used)
            {
                adapter_off();
                System.Threading.Thread.Sleep(200);
                let_tvpass();
                addmemo("plc sn 已清零", 1);
                Clear_TV_SN();
                addmemo("握手信号清零", 1);
                plc_write(config_json.PLC_StartTest_Register, "0");
                addmemo("等待进入下一台测试");
                timer_plc.Enabled = true;  //进入下一次测试
            }
            TestTimeTimer.Enabled = false;
            tb_sn.Focus();
        }

        bool savetodata()
        {
            test_result = "";
            try
            {
                data.WORKSTATIONID = lb_workstationid.Text;
                data.SOFT_VER = SOFT_VER.Text;
                data.testdatetime = DateTime.Now.ToString();
                data.MO = tb_type.Text;
                data.SN = tb_sn.Text;
                data.SOFT_VER = SOFT_VER.Text;
                data.TEST_TIMES = lb_test_times.Text;
                data.ACW_VALUE = lb_acw.Text;
                data.ACW_VOL = tb_acw_vol.Text;
                data.ACW_CUR = tb_acw_curr.Text;
                data.ACW_TIME = "0";// tb_acw_time.Text;
                data.ACW_RESULT = lb_acw.Text;
                data.IR_VALUE = lb_ir.Text;
                data.IR_VOL = tb_ir_vol.Text;
                data.IR_RES = tb_ir_res.Text;
                data.IR_TIME = "0";// tb_ir_time.Text;
                data.IR_RESULT = lb_ir.Text;

                data.GB_RESULT = lb_gb.Text;
                data.GB_CUR = tb_gb_cur.Text;
                data.GB_RES = tb_gb_res.Text;
                data.GB_TIME = "0";//tb_gb_time.Text;


                data.RESULT = lb_result.Text;
                data.mesreply = tb_mesreply.Text;
                data.op_name = config_json.login_name;
                //todo 保存本地数据 Mysql_Class_Laiya.DataInsert
                string insert_result = Class_Mysql.DataInsert_jyny(data);
                if (insert_result == "OK") addmemo("本地数据库保存成功", 1);
                else addmemo("本地数据保存失败："+insert_result);
                return true;
            }
            catch (Exception ex)
            {
                addmemo("本地数据库保存失败:" + ex.Message, 3);
                return false;
            }
        }
       TestData_Jyny data = new TestData_Jyny();
        string mes_data;
        /// <summary>
        /// todo sendtomes 发送数据到MES
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        void sendtomes()
        {
            if (cb_sendtomes.Checked == true)
            {
                data.ACW_VALUE = lb_acw.Text;
                data.IR_VALUE = lb_ir.Text;
                data.GB_RESULT = lb_gb.Text;
                data.MO = tb_type.Text;
                data.SN = tb_sn.Text;
                data.WORKSTATIONID = lb_workstationid.Text;
                data.ERROR_CODE = "";

                lb_result.Text = "OK";
                if (data.ACW_VALUE == "FAIL")
                {
                    lb_result.Text = "NG";
                    data.ERROR_CODE = lb_workstationid.Text+ "-001";
                    data.ERROR_SPOT = "acw fail";
                }
              
                if (data.IR_VALUE == "FAIL")
                {
                    lb_result.Text = "NG";
                    data.ERROR_CODE = lb_workstationid.Text + "-002";
                    data.ERROR_SPOT = "ir fail";
                }
                if (data.GB_RESULT == "FAIL")
                {
                    lb_result.Text = "NG";
                    data.ERROR_CODE = lb_workstationid.Text + "-003";
                    data.ERROR_SPOT = "gb fail";
                }
                data.SOFT_VER = SOFT_VER.Text;
                data.RESULT = lb_result.Text;
                data.TEST_TIMES = lb_test_times.Text;
                mes_data = SendDataToMes.JynyToJson(data);

                addmemo("发送测试结果到MES:" + mes_data,1);

                //todo 113

                string result = SendDataToMes.SendStr(mes_data);
                addmemo("MES 回复:" + result,1);

                if (result.IndexOf("服务器连接失败") > -1)
                {
                    lb_mes_sendresult.Text = "失败";
                    lb_mes_sendresult.ForeColor = Color.Red;
                    if (Sendtomes_failtimes < 10)
                    {
                        addmemo("MES 提交失败，1秒重新提交", 3);
                        Timer_Sendtomes.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
                    }
                    else {
                      
                        StopTest();
                        addmemo("MES已提交10次失败，测试停止", 3);
                        MessageBox.Show("MES已提交10次失败，测试停止，待人工处理");
                    }
                }
                else
                {
                    lb_mes_sendresult.Text = "成功";
                    lb_mes_sendresult.ForeColor = Color.Green;
                    Sendtomes_failtimes = 0;
                    tb_mesreply.Text = tb_sn.Text + ":" + result;
                    if (tb_mesall.Text.Length > 400)
                        tb_mesall.Text = "";
                    tb_mesall.Text += tb_sn.Text + ":" + result + "\r\n";
                }
            }
        }


       int  Sendtomes_failtimes=0;
        private void Timer_Sendtomes_Tick(object sender, ElapsedEventArgs e)
        {
            Timer_Sendtomes.Enabled = false;
            string result = SendDataToMes.SendStr(mes_data);
            addmemo("MES 回复:" + result);
            if (result.IndexOf("服务器连接失败") > -1)
            {
                if (Sendtomes_failtimes == 5) {
                    addmemo("发送数据到mes已失败5次", 1);
                    tb_sn.Focus();
                } else {
                    addmemo("1秒后再次发送数据到mes", 1);
                    Timer_Sendtomes.Enabled = true;
                    Sendtomes_failtimes++;
                }
                
            }
            else
            {
               
                addmemo("mes数据发送成功",1);
                tb_mesreply.Text = tb_sn.Text + ":" + result;
                tb_mesall.Text += tb_sn.Text + ":" + result + "\r\n";
            }
        }

        #region 

        private void addmemo_txt(string memo)
        {
            memo = DateTime.Now.ToString("[yyyy-M-d HH:mm:ss.fff] ") + memo + "\r\n";
            tb_memo.AppendText(memo);
            tb_memo.ScrollToCaret();
            logger.Info(memo);
        }

        void  addmemo(string memo, int i)
        {
            addmemo_txt(memo);
        }
        void addmemo(string memo)
        {
            addmemo_txt(memo);
        }
        //private void addmemo_rich(string memo, int i)

        //{
        //    memo = DateTime.Now.ToString("[yyyy-M-d HH:mm:ss.fff] ") + memo + "\r\n";
        //    try
        //    {
        //        if (i == 0)
        //        {
        //            richTextBox1.SelectionColor = System.Drawing.Color.DarkGreen;
        //            richTextBox1.SelectionFont = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        //        }
        //        if (i == 1) richTextBox1.SelectionColor = System.Drawing.Color.Black;
        //        if (i == 2) richTextBox1.SelectionColor = System.Drawing.Color.Yellow;
        //        if (i == 3)
        //        {
        //            richTextBox1.SelectionColor = System.Drawing.Color.Red;
        //            richTextBox1.SelectionFont = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        //        }

        //        richTextBox1.AppendText(memo);
        //        richTextBox1.ScrollToCaret();
        //    }
        //    catch { }

        //    string path = logdird + "\\" + logfile;
        //    if (!System.IO.File.Exists(path))
        //    {
        //        FileStream stream = System.IO.File.Create(path);
        //        stream.Close();
        //        stream.Dispose();
        //    }
        //    using (StreamWriter writer = new StreamWriter(path, true))
        //    {
        //        writer.Write(memo);
        //        writer.Close();
        //    }

        //}
        #endregion


        private void tb_sn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (tb_sn.Text.Trim() == "") {
                    MessageBox.Show("序列号不能为空");
                    return;
                }
                test_times = 1;
                lb_test_times.Text = test_times.ToString();
                plc_init();
                if (serial_init() == false) return;
                adapter_on();
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            tb_mesall.Text = "";
        }

        void let_tvpass()
        {
            addmemo("TV 放行");
            plc_write(config_json.PLC_LetTVPass_Register, "1");
        }

    public    void window_init()
        {
            lb_jyny_com.Text = config_json.Jyny_Device_PortName;
            lb_workstationid.Text = config_json.Workstationid;
            lb_plcip.Text = config_json.PLC_IP;
            cb_autorun.Checked = Convert.ToBoolean(config_json.auto_run);
            cb_retryafterfail.Checked = Convert.ToBoolean(config_json.retryafterfail);
            cb_stopafterfail.Checked = Convert.ToBoolean(config_json.stopafterfail);
            cb_sendtomes.Checked = Convert.ToBoolean(config_json.sendtomes);
            cb_prefailsnotomes.Checked = Convert.ToBoolean(config_json.prefailsnotomes);
            cb_lettvpassafterfail_notsendng.Checked = Convert.ToBoolean(config_json.lettvpassafterfail_notsendng);
            cb_plc_used.Checked = config_json.PLC_Used;
        }

        private string update_pronum()
        {
            try
            {
                con.Open();
            }
            catch { }

            string updatestr = "UPDATE `" + config_json.mysql_yielddata_table 
                + "` SET `一次直通数量`='" + lb_first_pass_num.Text
                + "',`一次直通率`='" + lb_first_pass_rate.Text
                + "',`通过数量`='" + lb_pass_num.Text
                + "',`通过率`='" + lb_pass_rate.Text
                + "',`误测数量`='" + lb_faulttest_num.Text
                + "',`误测率`='" + lb_faulttest_rate.Text
                + "',`不合格数量`='" + lb_ng_num.Text
                + "',`不合格率`='" + lb_ng_rate.Text
                + "',`备注`='" + ""
                + "' WHERE `测试日期`='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";//插入今天日期并加载默认值0
            MySql.Data.MySqlClient.MySqlCommand insertcmd = new MySql.Data.MySqlClient.MySqlCommand(updatestr, con);
            try
            {
                insertcmd.ExecuteNonQuery();
                con.Close();
                return "生产数据更新OK";
            }
            catch (Exception ex)
            {
                con.Close();
                return ex.Message;
            }

        }

        void savetolog()
        {
            string dir = config_json.log_dir + DateTime.Now.ToString("yyyyMMdd") + "\\" + lb_result.Text + "\\";

            System.IO.DirectoryInfo di = new DirectoryInfo(dir);
            if (di.Exists == false) di.Create();
            string filename = tb_sn.Text.Replace("/","") + "_" + DateTime.Now.ToString("HHmmss") + ".txt";
            string path = dir + "\\" + filename;
            //  richTextBox1.SaveFile(path,RichTextBoxStreamType.RichText);
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(tb_memo.Text);
                sw.Close();
            }

        }

        private void btn_pass_Click(object sender, EventArgs e)
        {
            //sendtomes(lb_acw.Text, lb_ir.Text);
            //ng_num++;
            //tongji_update();
            adapter_off();
            System.Threading.Thread.Sleep(1000);
            let_tvpass();
            //fail_times = 0;
            //addmemo("绝缘电阻测试失败！");
            //savetodata();
            //savetolog();
        }

        void tongji_update()
        {
            lb_first_pass_num.Text = first_pass_num.ToString();
            lb_pass_num.Text = pass_num.ToString();
            lb_faulttest_num.Text = faulttest_num.ToString();
            lb_ng_num.Text = ng_num.ToString();

            double totalnum = pass_num + ng_num;
            double rate = (first_pass_num / totalnum) * 100;
            lb_first_pass_rate.Text = Convert.ToString(Math.Round(rate, 2));

            rate = (pass_num / totalnum) * 100;
            lb_pass_rate.Text = Convert.ToString(Math.Round(rate, 2));

            rate = (faulttest_num / totalnum) * 100;
            lb_faulttest_rate.Text = Convert.ToString(Math.Round(rate, 2));

            rate = (ng_num / totalnum) * 100;
            lb_ng_rate.Text = Convert.ToString(Math.Round(rate, 2));
        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            first_pass_num++;
            pass_num++;
            tongji_update();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            faulttest_num++;
            pass_num++;
            tongji_update();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            ng_num++;
            tongji_update();
        }

        #region 菜单
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.ShowDialog();
        }

        private void 数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pLC测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_PLC formplc = new Form_PLC();
            formplc.Show();
        }

        private void 绝缘耐压仪测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion


        #region PLC 操作

        void light_green_on()
        {
            if (config_json.PLC_Used && plc_inited)
            {
                addmemo("亮绿灯");
                plc_write(config_json.PLC_Light_Register, config_json.PLC_Light_GREEN);
            }
        }

        void light_red_on()
        {
            if (config_json.PLC_Used && plc_inited)
            {
                addmemo("亮红灯");
                plc_write(config_json.PLC_Light_Register, config_json.PLC_Light_RED);
            }
        
        }

        void light_yellow_on()
        {
            if (config_json.PLC_Used && plc_inited)
            {
                addmemo("亮黄灯");
                plc_write(config_json.PLC_Light_Register, config_json.PLC_Light_YELLOW);
            }
        }

        string plc_write(string addr, string value)
        {
            if (config_json.PLC_Used && plc_inited)
            {
                addmemo("PLC 写入" + addr + "=" + value);
                string str = "";
                HslCommunication.OperateResult opr = PLC.Write(addr, Convert.ToInt16(value));
                if (opr.IsSuccess)
                {
                    addmemo("PLC写入OK", 0);
                }
                else
                {
                    addmemo("PLC写入失败，再次写入", 3);
                    addmemo("PLC 写入" + addr + "=" + value);
                    HslCommunication.OperateResult opr2 = PLC.Write(addr, Convert.ToInt16(value));
                    if (opr2.IsSuccess)
                    {
                        addmemo("PLC写入OK", 0);
                    }
                    else
                    {
                        addmemo("PLC再次写入失败", 3);
                    }
                }
                return str;
            }
            return "";
        }


        #endregion

        private void 测试数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Mysql_laiya fm = new Form_Mysql_laiya();
            fm.Show();
        }

        private void 生产统计ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_tongji_jyny ft = new Form_tongji_jyny();
            ft.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            tb_sn.Text = Get_TV_SN();
            tb_type.Text = Get_TV_Type();
        }

        private void 使用浏览器查询数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://localhost/phpmyadmin");
        }

        private void 操作手册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("绝缘耐压测试系统操作手册.pdf");
        }

        private void 注销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确认要注销吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                user_logout();
                config_json.login_id = "";
                Form_Login flogin = new Form_Login();
                this.Visible = false;
                flogin.ShowDialog();
                this.Close();
            }
            else
            {

            }
        }

        void user_logout()
        {
            if (config_json.login_id!="")
            {
                TimeSpan ts = DateTime.Now - config_json.login_datetime;
                Class_Mysql.add_logout_log(config_json.login_id, ts.TotalHours.ToString());
            }
        }

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_password_change fpass = new Form_password_change();
            fpass.ShowDialog();
        }

        private void testToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            user_logout();
        }
      
        private void btn_test_afterNG_Click(object sender, EventArgs e)
        {
            testing = true;
            btn_test_afterNG.Enabled = false;
            light_green_on();
            test_times++;
            lb_test_times.Text = test_times.ToString();
            lb_mes_status.Text = "OK";
            addmemo("开始NG后人工复测");
            Test_Begin();
        }

        private void lb_test_info_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
          addmemo( Class_Mysql. DataInsert_TestData());
        }

        private void 手动测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Test_Begin();
        }

        private void cb_plc_used_CheckedChanged(object sender, EventArgs e)
        {
            config_json.PLC_Used = cb_plc_used.Checked;
        }

        private void tb_sn_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_releasealert_Click(object sender, EventArgs e)
        {
            Release_Alert();
        }
        void Release_Alert() {
            if (config_json.PLC_Used)
            {
                addmemo("解除报警");
                plc_write(config_json.PLC_Light_Register, "4");
            }
        }

        private void 升级数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 软件注册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_SoftRegister fsr = new Form_SoftRegister();
            fsr.ShowDialog();
        }

        private void 初始化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serial_init();
        }

        private void deviceCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Device_Check();
        }

        private void 启动测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Test_Begin();
        }

        private void 调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SerialPort.IsOpen) SerialPort.Close();
            Form_serial fserial = new Form_serial();
            fserial.Show(this);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void 手动设置仪表自检测通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serial_inited = true;
        }
    }
}

/*
GPT-9804 ,EM110867    ,v2.08,
ACW,PASS ,2.498kV,00.17 mA ,T=003.0S
IR ,PASS ,0.497kV,0015M ohm,T=001.0S
OK
*/
//UPDATE `tongji` SET `first_pass_num`='20',`first_pass_rate`='',`pass_num`='30',pass_rate`='',faulttest_num`='20',faulttest_rate`='',ng_num`='9',ng_rate`='',memo`='', WHERE `testdate`='2020-01-13'