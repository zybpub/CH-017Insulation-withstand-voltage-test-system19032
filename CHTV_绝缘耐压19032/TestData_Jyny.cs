using System;

namespace Test_Automation
{
    public  class TestData_Jyny
    {
        public string op_name { get; set; }//操作员代码
        public string testdatetime { get; set; }
        public string WORKSTATIONID { get; internal set; }
        public string SOFT_VER { get; internal set; }
        public string MO { get; set; }
        public string SN { get; set; }

        public string ACW_VALUE { get; set; }
        public string ACW_VOL { get; set; }
        public string ACW_CUR { get; set; }
        public string ACW_TIME { get; set; }
        public string ACW_RESULT { get; set; }
        public string IR_VALUE { get; set; }
        public string IR_VOL { get; set; }
        public string IR_RES { get; set; }
        public string IR_TIME { get; set; }
        public string IR_RESULT { get; set; }

        public string TEST_TIMES { get; set; }
        public string RESULT { get; set; }
        public string ERROR_CODE = "";       //错误代码
        public string ERROR_SPOT = "";   //错误点
    
        public string uploaddatetime { get; set; }
        public string memo { get; set; }
        public string mesreply { get; set; }
        public DateTime 调试时间 { get; internal set; }

        public string testseconds { get; internal set; }
        public string GB_RESULT { get; set; }
        public string GB_CUR { get; set; }
        public string GB_RES { get; set; }
        public string GB_TIME { get; set; }
    }
}
