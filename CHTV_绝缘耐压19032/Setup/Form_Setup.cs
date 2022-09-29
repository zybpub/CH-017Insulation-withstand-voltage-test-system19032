using System;
using System.Windows.Forms;

namespace Test_Automation.Setup
{
    public partial class Form_Setup : Form
    {
        public Form_Setup()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Setup.Form_Setup_System fs = new Form_Setup_System();
            fs.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Setup.Form_Setup_19032 fjy = new Form_Setup_19032();
            fjy.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Setup.Form_Setup_Mysql fmysql = new Form_Setup_Mysql();
            fmysql.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Setup.Form_Setup_PLC fplc = new Form_Setup_PLC();
            fplc.ShowDialog();
        }

        private void Form_Setup_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form_password_change_technician fpass = new Form_password_change_technician();
            fpass.ShowDialog();
        }
    }
}
