using System;

namespace Test_Automation
{
    class SendDataToMes
   {
        public static string JynyToJson(TestData_Jyny jyny) {
                //整理将要提交的数据data
                System.IO.StringWriter sw = new System.IO.StringWriter();
                using (Newtonsoft.Json.JsonWriter writer = new Newtonsoft.Json.JsonTextWriter(sw))
                {
                    writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                    writer.WriteStartObject();

                    writer.WritePropertyName("TESTS");
                    writer.WriteStartArray();

                    writer.WriteStartObject();
                    writer.WritePropertyName("KEY");
                    writer.WriteValue("ACW");
                    writer.WritePropertyName("VALUE");
                    writer.WriteValue(jyny.ACW_VALUE);
                    writer.WriteEndObject();

                    writer.WriteStartObject();
                    writer.WritePropertyName("KEY");
                    writer.WriteValue("IR");
                    writer.WritePropertyName("VALUE");
                    writer.WriteValue(jyny.IR_VALUE);
                    writer.WriteEndObject();

                writer.WriteStartObject();
                writer.WritePropertyName("KEY");
                writer.WriteValue("GB");
                writer.WritePropertyName("VALUE");
                writer.WriteValue(jyny.GB_RESULT);
                writer.WriteEndObject();

                writer.WriteEndArray();

                    writer.WritePropertyName("SOFT_VER");
                    writer.WriteValue(jyny.SOFT_VER);

                    writer.WritePropertyName("SN");
                    writer.WriteValue(jyny.SN);

                writer.WritePropertyName("MO");
                writer.WriteValue(jyny.MO);

                writer.WritePropertyName("WORKSTATIONID");
                    writer.WriteValue(jyny.WORKSTATIONID);
                    writer.WritePropertyName("RESULT");
                    writer.WriteValue(jyny.RESULT);

                    writer.WritePropertyName("TEST_TIMES");
                    writer.WriteValue(jyny.TEST_TIMES);

                    writer.WritePropertyName("ERRORS");

                    if (jyny.ERROR_CODE != "") { 
                        writer.WriteStartArray();

                        writer.WriteStartObject();
                        writer.WritePropertyName("ERROR_CODE");
                        writer.WriteValue(jyny.ERROR_CODE);
                        writer.WritePropertyName("ERROR_SPOT");
                        writer.WriteValue("");
                        writer.WriteEndObject();

                        writer.WriteEndArray();
                    }

                    writer.WriteEndObject();
                    writer.Flush();
                }
                sw.Close();
                return sw.ToString();
            }

       
        public static string SendStr(string data) {
            try
            {
                string url = config_json.MES_URL;

                var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);    //创建了一个http请求
                request.ContentType = "application/json;charset=UTF-8";
                request.Method = "POST";     //请求方式Post

                byte[] payload = System.Text.Encoding.UTF8.GetBytes(data);

                //设置请求的 ContentLength 
                request.ContentLength = payload.Length;
                using (System.IO.Stream streamWriter = request.GetRequestStream())
                {
                    streamWriter.Write(payload, 0, payload.Length);
                }

                var response = (System.Net.HttpWebResponse)request.GetResponse();
                using (var streamReader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return result.ToString();
                }
            }
            catch (Exception ex) {
                return "服务器连接失败：" + ex.Message;
            }
        }
    }
}
