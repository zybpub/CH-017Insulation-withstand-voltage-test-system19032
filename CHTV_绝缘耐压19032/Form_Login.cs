using System;
using System.Windows.Forms;

namespace Test_Automation
{
    public partial class Form_Login : Form
    {
        public Form_Login()
        {
            InitializeComponent();
            this.CenterToScreen();
        }
        private void Form_Login_Load(object sender, EventArgs e)
        {
           // SkinEngine.SkinFile = "DiamondOlive.ssk";//加皮肤step2
           // SkinEngine.Active = true;//加皮肤step3
            config_json.config_json_readall();

            if (config_json.Workstationid == "") {
                MessageBox.Show("工作站还没有进行设置，请在弹出的窗体中设置工作站名称");
                Setup.Form_Setup_System f = new Setup.Form_Setup_System();
                f.ShowDialog();
            }

            if (config_json.login_remember)
            {
                cb_remember.Checked = true;
                textBox1.Text = config_json.login_code;
                textBox2.Text = config_json.login_pass;
            }
            this.Text = config_json.soft_name;
            ts_softver.Text ="软件版本："+ System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
           string result= Class_Mysql.check_and_start_mysql_service();
            if (ts_status.Text == "服务不存在")
            {
                MessageBox.Show("Mysql数据库未安装，请先安装Mysql数据库");
            }
            else {
                ts_status.Text = "数据库服务启动成功";
            }

            // System.Threading.Thread reg = new System.Threading.Thread(Register_Check);
            //  reg.Start();
         

            result = Class_Mysql.check_mysql();
            if (result != "OK") MessageBox.Show(result);

           // Register_Check();

        }

        bool Register_Check()
        {
            int res = Class_SoftRegister.InitRegedit();
           
            if (res == 0)
            {
                return true;
            }
             if (res == 1)
            {
                MessageBox.Show("软件尚未注册，请注册软件！");
            }
            if (res == 2)
            {
                MessageBox.Show("注册信息有误,请重新注册！");
                Form_SoftRegister f = new Form_SoftRegister();
                f.ShowDialog();
                this.Close();
            }
            if (res == 3) //软件已过期
            {
                MessageBox.Show("软件已过期，请注册！");
                Form_SoftRegister f = new Form_SoftRegister();
                f.ShowDialog();
                this.Close();
            }
             if (res >= 100 && res < 131)
            {
                MessageBox.Show("软件剩余天数：" + ((int)res - 100).ToString());
                // this.Close();
            }
            return false;
        }

        bool cb_login_changed = false;

        private void button1_Click(object sender, EventArgs e)
        {
            System.Threading.Thread th = new System.Threading.Thread(ReportIP);
            th.IsBackground = true;
            th.Start();
        

            if (cb_login_changed)
            {
                if (cb_remember.Checked)
                {
                    config_json.save("login_remember", "true");
                    config_json.save("login_code", textBox1.Text);
                    config_json.save("login_pass", textBox2.Text);
                }
                else
                {
                    config_json.save("login_remember", "false");
                }
            }

            string result;
           // 
            config_json.login_name = textBox1.Text;
            switch (comboBox1.Text) {
                case "操作员":
                    result = Class_Mysql.login_operater(textBox1.Text, textBox2.Text);
                    if (result == "OK")
                    {
                        config_json.login_code = textBox1.Text;
                        config_json.login_id=   Class_Mysql.add_login_login("操作员登录", textBox1.Text);
                        config_json.login_datetime = DateTime.Now;
                        this.Visible = false;
                        Form_Main fm = new Form_Main();
                        fm.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("登录失败:"+ result);
                    }
                    break;
                case "工艺员":
                    result = Class_Mysql.login_technician(textBox1.Text, textBox2.Text);
                    if (result == "OK")
                    {
                        config_json.login_code = textBox1.Text;
                        config_json.login_id = Class_Mysql.add_login_login("工艺员登录", textBox1.Text);
                        config_json.login_datetime = DateTime.Now;
                        this.Visible = false;
                        var fm = new Setup.Form_Setup();
                        fm.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("登录失败:" + result);
                    }
                    break;
                case "超级管理员":
                    result = Class_Mysql.login_admin(textBox1.Text, textBox2.Text);
                    if (result == "OK")
                    {
                        config_json.login_code = textBox1.Text;
                        config_json.login_id = Class_Mysql.add_login_login("超级管理员登录", textBox1.Text);
                        config_json.login_datetime = DateTime.Now;
                        this.Visible = false;
                        var fm = new admin.Form_admin();
                        fm.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("登录失败:" + result);
                    }
                    break;
            }
        }

        private void ReportIP()
        {
            try
            {
                string url = config_json.reportip_url;
                string ip = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[1].ToString();
                string data = "?workid=" + config_json.Workstationid + "&ip=" + ip + "&memo=Jyny_login," + ts_softver.Text;
                System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url + data);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Timeout = 2000;
                System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(httpWebResponse.GetResponseStream());
                string responseContent = streamReader.ReadToEnd();
                httpWebResponse.Close();
                streamReader.Close();
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                button1_Click(null, null);
            }
        }

        private const int WM_HOTKEY = 0x312; //窗口消息：热键
        private const int WM_CREATE = 0x1; //窗口消息：创建
        private const int WM_DESTROY = 0x2; //窗口消息：销毁

        private const int HotKeyID = 1; //热键ID（自定义）

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, System.Windows.Forms.Keys vk);

        /// <summary>
        /// 注销热键
        /// </summary>
        /// <param name="hWnd">要取消热键的窗口的句柄</param>
        /// <param name="id">要取消热键的ID</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// 辅助键名称。
        /// Alt, Ctrl, Shift, WindowsKey
        /// </summary>
        [Flags()]
        public enum KeyModifiers { None = 0, Alt = 1, Ctrl = 2, Shift = 4, WindowsKey = 8 }

        /// <summary>
        /// 注册热键
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="hotKey_id">热键ID</param>
        /// <param name="keyModifiers">组合键</param>
        /// <param name="key">热键</param>
        public static string RegHotKey(IntPtr hwnd, int hotKeyId, KeyModifiers keyModifiers, System.Windows.Forms.Keys key)
        {
            if (!RegisterHotKey(hwnd, hotKeyId, keyModifiers, key))
            {
                int errorCode = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                if (errorCode == 1409)
                {
                    return "热键被占用 ！";
                }
                else
                {
                    return "注册热键失败！错误代码：" + errorCode;
                }
            }
            return "";
        }

        /// <summary>
        /// 注销热键
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="hotKey_id">热键ID</param>
        public static void UnRegHotKey(IntPtr hwnd, int hotKeyId)
        {
            //注销指定的热键
            UnregisterHotKey(hwnd, hotKeyId);
        }

        protected override void WndProc(ref Message msg)
        {
            base.WndProc(ref msg);
            switch (msg.Msg)
            {
                case WM_HOTKEY: //窗口消息：热键
                    int tmpWParam = msg.WParam.ToInt32();
                    if (tmpWParam == HotKeyID)
                    {
                        //System.Windows.Forms.SendKeys.Send("^v");
                        //MessageBox.Show("F7按下");
                        //按下热键后要执行的代码
                        //button3.Visible = true;
                        // MessageBox.Show("Hot Key Pressed!");
                        var f = new Setup.Form_Setup_Mysql();
                        f.ShowDialog();
                        this.Close();
                    }
                    break;
                case WM_CREATE: //窗口消息：创建 下面使用F6作热键
                    RegHotKey(this.Handle, HotKeyID, KeyModifiers.None, Keys.F6);
                    break;
                case WM_DESTROY: //窗口消息：销毁
                    UnRegHotKey(this.Handle, HotKeyID); //销毁热键
                    break;
                default:
                    break;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (Class_Mysql.check_table_column_isexist(config_json.mysql_testdata_table, "operator") == false)
            {
                Class_Mysql.add_column(config_json.mysql_testdata_table, "operator", "VARCHAR(50)");
            }
        }

        private void cb_remember_CheckedChanged(object sender, EventArgs e)
        {
            cb_login_changed = true;
        }
    }
}
