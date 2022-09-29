using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Automation
{
    class Serial_Command
    {
        public static string 获取装置基本资料 = "*IDN?";  
                                //Chroma,19032-P,19032P101788,6.04
        public static string 启动测试 = "SOUR:SAFE:START";
        public static string 停止测试 = "SOUR:SAFE:STOP";
        public static string 设定自动测试回报资料1 = "SAFE:RES:AREP:ITEM STAT,MODE,OMET,MMET";
        public static string 设定自动测试回报资料2 = "SAFE:RES:AREP:ITEM STAT,MODE,OMET,MMET";
                             //AC,+3.000000E+03,+1.400000E-04,34,IR,+9.910000E+37,+9.910000E+37,112
        public static string 设定自动测试回报资料3 = "SAFE:RES:AREP:ITEM STAT,MODE,OMET,MMET,TELA";
                             //AC,+3.004000E+03,+1.200000E-04,+2.000000E+00,34,IR,+9.910000E+37,+9.910000E+37,+9.910000

        public static string 设置自动回报测试结果 = "SAFE:RES:AREP ON";
        public static string 设置自动回报功能保持到下次开机 = "SOUR:SAFE:RES:ASAV ON";

        public static string 查看所有测试结果 = "SAFE:RES:ALL?"; //34,112
        public static string 装置重置 = "*RST";

        public static string 查询测试项目数 = "SOUR:SAFE:SNUMB?";  //返回2
        public static string 查询测试项目1 = "SOUR:SAFE:STEP 1:SET?";  
                                //返回1,AC,+3.000000E+03,+1.000000E-02,+3.000000E-04,+0.000000E+00,+2.300000E+05,+2.000000E+00,+0.000000E+00,+0.000000E+00,+0.000000E+00,(0),(0)
                                //          3000         HIGH: 0.01    LOW: 0.0003    ARC:0        ARC FILTER:230k  TIME:2     RAMP:0         FALL:0        SCAN BOX:OFF
        public static string 查询测试项目2 = "SOUR:SAFE:STEP 2:SET?";
        //返回2,IR,+5.000000E+02,+4.000000E+06,+0.000000E+00,+2.000000E+00,+0.000000E+00,+0.000000E+00,+3.000000E-03,0,(0),(0)
        //          500         HIGH: 4M      LOW: 0          TIME:2                                    0.003         FALL:0        SCAN BOX:OFF

    }
}
