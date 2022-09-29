namespace Test_Automation
{
    class config_json
    {
        #region  变量

        public static string soft_name = "绝缘耐压测试系统(Chroma19032)";
        public static string save_file_drive = "d:";
        public static string save_config_file_dir = "d:\\软件配置\\";
        public static string config_file_name= save_config_file_dir+"jyny19032_config.json";
        public static string reportip_url = "http://10.3.15.213/reportip.ashx";

        public static string log_dir = "d:\\日志\\绝缘耐压\\";
        public static string debug_dir = "d:\\logs\\";

        public static string config_memo = "绝缘耐压配置文件";
        public static string pass = "QWQ/r3FGnyU=";
        public static string start_services = "wampmysqld64";
        //public static string start_services = "wampapache64,wampmysqld64";

        //Login
        public static string login_name = "";
        public static string login_id = "";
        public static System.DateTime login_datetime;
        internal static string form_title = "";

        public static bool auto_run = false;            //是否运行程序后自动开始测试
        public static bool retryafterfail = true;
        public static bool stopafterfail = false;
        public static bool prefailsnotomes = true;
        public static bool sendtomes = true;
        public static bool lettvpassafterfail_notsendng = false;

        public static string Workstationid="";
        public static bool JYNY_SerialPort_Used = true;
        public static string Jyny_Device_PortName="COM1";           //串口端口号
        public static string Jyny_Device_BaudRate="9600";           //串口波特率
        public static string Jyny_Device_DataBits="8";           //串口数据位
        public static string Jyny_Device_StopBits="1";           //串口数据位
        public static string Jyny_Device_Parity="None";           //串口数据位

        public static bool PLC_Used = true;
        public static string PLC_IP="192.168.1.2";              //PLC IP地址
        public static string PLC_Port="502";             //PLC 端口 默认：502
        public static string PLC_Station="1";             //PLC 站号  默认：1
        public static string PLC_Adapter_Register= "4099";   //PLC线体对接控制寄存器地址
        public static string PLC_Light_Register= "4119";     //PLC红绿黄灯控制寄存器地址
        public static string PLC_StartTest_Register = "5000";   //PLC控制是否测试可以开始 0不测试 1 测试
        public static string PLC_LetTVPass_Register = "5001";  //发送TV放行信号给scada
        public static string PLC_Type_Register = "5002";    //PLC读取sn地址
        public static string PLC_SN_Register = "5020";    //PLC读取sn地址
        public static string PLC_Light_GREEN = "2";
        public static string PLC_Light_RED = "1";
        public static string PLC_Light_YELLOW = "3";
        public static string ShakeHand_OK_Code = "2";  //启动测试信号值
        public static bool Adapter_on_check = true;
        public static bool Adapter_off_check = true;

        public static bool mysql_used=true;    //是否启用数据库
        public static string Mysql_IP="127.0.0.1";       //Mysql IP 地址
        public static string Mysql_Port="3306";      //Mysql 端口号
        public static string Mysql_User="root";      //Mysql 用户名 需要有写入权限
        public static string Mysql_Pass="jczx";      //Mysql 密码
        public static string mysql_database_name= "jczx_mysql_dbV3";  //数据库名
        public static string mysql_testdata_table = "jyny_testdataV4"; //统计表名
        public static string mysql_yielddata_table = "tongji_jynyV3"; //统计表名

        public static string MES_URL = "http://10.52.88.17:8080/N2/http/interface.ms?model=IntoScgz&method=intoScgzSaveInfo";
        internal static string MES_Abnormal_Keywords = "接口异常";
        internal static string TestTimeOutSeconds="120";

        public static string login_code = "";
        public static dynamic login_pass = "";
        public static dynamic login_remember = false;

        #endregion

        public static MySql.Data.MySqlClient.MySqlConnection get_mysqlconn()
        {
               MySql.Data.MySqlClient.MySqlConnection mySqlConnection =
                      new MySql.Data.MySqlClient.MySqlConnection(
                          "Database=" + config_json.mysql_database_name
                          + ";Data Source=" + config_json.Mysql_IP
                          + ";User Id=" + config_json.Mysql_User
                          + ";Password=" + config_json.Mysql_Pass + ";charset=utf8");
            return mySqlConnection;
    }
        public static bool create_dafault_configfile()
        {
            if (!System.IO.File.Exists(config_json.config_file_name))
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(config_json.save_config_file_dir);
                if (di.Exists == false) di.Create();

                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(config_json.config_file_name, true))
                {
                    writer.WriteLine("{}");
                    writer.Close();
                }
                string json = System.IO.File.ReadAllText(config_json.config_file_name);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                jsonObj["login_code"] = config_json.login_code;
                jsonObj["login_pass"] = config_json.login_pass;
                jsonObj["login_remember"] = config_json.login_remember;

                jsonObj["mysql_used"] = config_json.mysql_used;
                jsonObj["Mysql_IP"] = config_json.Mysql_IP;
                jsonObj["Mysql_Port"] = config_json.Mysql_Port;
                jsonObj["Mysql_User"] = config_json.Mysql_User;
                jsonObj["Mysql_Pass"] = config_json.Mysql_Pass;
                jsonObj["mysql_database_name"] = config_json.mysql_database_name;
                jsonObj["mysql_testdata_table"] = config_json.mysql_testdata_table;
                jsonObj["mysql_yielddata_table"] = config_json.mysql_yielddata_table;

                jsonObj["start_services"] = config_json.start_services;
                jsonObj["Workstationid"] = config_json.Workstationid;

                jsonObj["PLC_IP"] = config_json.PLC_IP;
                jsonObj["ShakeHand_OK_Code"] = config_json.ShakeHand_OK_Code;

                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(config_json.config_file_name, output);
            }

         
            return true;
        }
        public static bool config_json_readall()
        {
            if (System.IO.File.Exists(config_file_name) == false)
            {
                create_dafault_configfile();
                return false;
            }
            
                System.IO.StreamReader file = System.IO.File.OpenText(config_file_name);
                Newtonsoft.Json.JsonTextReader reader = new Newtonsoft.Json.JsonTextReader(file);
                Newtonsoft.Json.Linq.JObject jsonObject =
                                (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.Linq.JToken.ReadFrom(reader);
            if (jsonObject["Workstationid"] != null) Workstationid = (string)jsonObject["Workstationid"];
            if (jsonObject["start_services"] != null) start_services = (string)jsonObject["start_services"];

            if (jsonObject["login_code"] != null) login_code = (string)jsonObject["login_code"];
            if (jsonObject["login_pass"] != null) login_pass = (string)jsonObject["login_pass"];
            if (jsonObject["login_remember"] != null) login_remember = (bool)jsonObject["login_remember"];

            if (jsonObject["Jyny_Device_PortName"] != null) Jyny_Device_PortName = (string)jsonObject["Jyny_Device_PortName"];
            if (jsonObject["Jyny_Device_BaudRate"] != null) Jyny_Device_BaudRate = (string)jsonObject["Jyny_Device_BaudRate"];
            if (jsonObject["Jyny_Device_DataBits"] != null) Jyny_Device_DataBits = (string)jsonObject["Jyny_Device_DataBits"];
            if (jsonObject["Jyny_Device_StopBits"] != null) Jyny_Device_StopBits = (string)jsonObject["Jyny_Device_StopBits"];
            if (jsonObject["Jyny_Device_Parity"] != null) Jyny_Device_Parity = (string)jsonObject["Jyny_Device_Parity"];

            if (jsonObject["PLC_Used"] != null) PLC_Used = (bool)jsonObject["PLC_Used"];

            if (jsonObject["PLC_IP"] != null) PLC_IP = (string)jsonObject["PLC_IP"];
            if (jsonObject["PLC_Port"] != null) PLC_Port = (string)jsonObject["PLC_Port"];
            if (jsonObject["PLC_Station"] != null) PLC_Station = (string)jsonObject["PLC_Station"];
            if (jsonObject["PLC_Adapter_Register"] != null) PLC_Adapter_Register = (string)jsonObject["PLC_Adapter_Register"];
            if (jsonObject["PLC_Light_Register"] != null) PLC_Light_Register = (string)jsonObject["PLC_Light_Register"];
            if (jsonObject["PLC_StartTest_Register"] != null) PLC_StartTest_Register = (string)jsonObject["PLC_StartTest_Register"];
            if (jsonObject["PLC_LetTVPass_Register"] != null) PLC_LetTVPass_Register = (string)jsonObject["PLC_LetTVPass_Register"];
            if (jsonObject["Adapter_on_check"] != null) Adapter_on_check = (bool)jsonObject["Adapter_on_check"];
            if (jsonObject["Adapter_off_check"] != null) Adapter_off_check = (bool)jsonObject["Adapter_off_check"];

            if (jsonObject["PLC_Type_Register"] != null) PLC_Type_Register = (string)jsonObject["PLC_Type_Register"];
            if (jsonObject["PLC_SN_Register"] != null) PLC_SN_Register = (string)jsonObject["PLC_SN_Register"];
            if (jsonObject["PLC_Light_GREEN"] != null) PLC_Light_GREEN = (string)jsonObject["PLC_Light_GREEN"];
            if (jsonObject["PLC_Light_RED"] != null) PLC_Light_RED = (string)jsonObject["PLC_Light_RED"];
            if (jsonObject["PLC_Light_YELLOW"] != null) PLC_Light_YELLOW = (string)jsonObject["PLC_Light_YELLOW"];
            if (jsonObject["PLC_Type_Register"] != null) PLC_Type_Register = (string)jsonObject["PLC_Type_Register"];

            if (jsonObject["ShakeHand_OK_Code"] != null) ShakeHand_OK_Code = (string)jsonObject["ShakeHand_OK_Code"];

            if (jsonObject["mysql_used"] != null)
                if (((string)jsonObject["mysql_used"]).ToUpper() == "TRUE")
                    mysql_used = true;
                else mysql_used = false;

            if (jsonObject["Mysql_IP"] != null) Mysql_IP = (string)jsonObject["Mysql_IP"];
            if (jsonObject["Mysql_Port"] != null) Mysql_Port = (string)jsonObject["Mysql_Port"];
            if (jsonObject["Mysql_User"] != null) Mysql_User = (string)jsonObject["Mysql_User"];
            if (jsonObject["Mysql_Pass"] != null) Mysql_Pass = (string)jsonObject["Mysql_Pass"];
            if (jsonObject["mysql_database_name"] != null) mysql_database_name = (string)jsonObject["mysql_database_name"];
            if (jsonObject["mysql_testdatatable_name"] != null) mysql_testdata_table = (string)jsonObject["mysql_testdatatable_name"];
            if (jsonObject["mysql_tongjitable_name"] != null) mysql_yielddata_table = (string)jsonObject["mysql_tongjitable_name"];

            if (jsonObject["auto_run"] != null) auto_run = (bool)jsonObject["auto_run"];
            if (jsonObject["sendtomes"] != null) sendtomes = (bool)jsonObject["sendtomes"];
            if (jsonObject["prefailsnotomes"] != null) prefailsnotomes = (bool)jsonObject["prefailsnotomes"];
            if (jsonObject["stopafterfail"] != null) stopafterfail = (bool)jsonObject["stopafterfail"];
            if (jsonObject["retryafterfail"] != null) retryafterfail = (bool)jsonObject["retryafterfail"];
            if (jsonObject["lettvpassafterfail_notsendng"] != null) lettvpassafterfail_notsendng = (bool)jsonObject["lettvpassafterfail_notsendng"];

            if (jsonObject["MES_URL"] != null) MES_URL = (string)jsonObject["MES_URL"];
            if (jsonObject["pass"] != null) pass = (string)jsonObject["pass"];

            if (jsonObject["login_code"] != null) login_code = (string)jsonObject["login_code"];
            if (jsonObject["login_pass"] != null) login_pass = (string)jsonObject["login_pass"];
            if (jsonObject["login_remember"] != null) login_remember = (bool)jsonObject["login_remember"];

            file.Close();
      
            return true;
        }


        public static bool save(string key, string value)
        {
            try
            {
                string json = System.IO.File.ReadAllText(config_file_name);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                jsonObj[key] = value;
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(config_file_name, output);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 读取指定键名的值
        /// </summary>
        /// <param name="key">输入要获取值的键名</param>
        /// <returns></returns>
        public static string config_json_read(string key)
        {
            string result = "";
            try
            {
                System.IO.StreamReader file = System.IO.File.OpenText(config_file_name);
                Newtonsoft.Json.JsonTextReader reader = new Newtonsoft.Json.JsonTextReader(file);
                Newtonsoft.Json.Linq.JObject jsonObject =
                                (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.Linq.JToken.ReadFrom(reader);
                if (jsonObject[key] != null)
                    result = (string)jsonObject[key];
                file.Close();
            }
            catch { }
            return result;
        }
       

    }
}
