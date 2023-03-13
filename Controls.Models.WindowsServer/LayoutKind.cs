using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;

namespace WindowApi
{
    public class LayoutKind
    {
        // 创建结构体用于返回捕获时间

        struct LASTINPUTINFO
        {
            // 设置结构体块容量
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            // 捕获的时间
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }
        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        // 获取键盘和鼠标没有操作的时间，返回值为毫秒
        public static long GetLastInputTime()
        {
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
            // 捕获时间
            if (!GetLastInputInfo(ref vLastInputInfo))
                return 0;
            else
                return Environment.TickCount - (long)vLastInputInfo.dwTime;
        }
        #region 达成目标逻辑
        public class CheckTable{
            public bool IsAchieve { get; set; }
            public int TimeGOlal { get; set; }
        }
        public static List<CheckTable> checkTables = new List<CheckTable>() 
        {
            new CheckTable(){IsAchieve =false,TimeGOlal=5 * 1000},
            new CheckTable(){IsAchieve =false,TimeGOlal=10 * 1000},
            new CheckTable(){IsAchieve =false,TimeGOlal=30 * 1000},
            new CheckTable(){IsAchieve =false,TimeGOlal=1*60 * 1000},
            new CheckTable(){IsAchieve =false,TimeGOlal=5*60 * 1000},
            new CheckTable(){IsAchieve =false,TimeGOlal=10*60 * 1000},
            new CheckTable(){IsAchieve =false,TimeGOlal=30*60 * 1000},
            new CheckTable(){IsAchieve =false,TimeGOlal=1*60*60 * 1000},
            new CheckTable(){IsAchieve =false,TimeGOlal=2*60*60 * 1000},
            new CheckTable(){IsAchieve =false,TimeGOlal=3*60*60 * 1000},
            new CheckTable(){IsAchieve =false,TimeGOlal=4*60*60 * 1000},
            new CheckTable(){IsAchieve =false,TimeGOlal=5*60*60 * 1000},
        };
        public static bool check_is_ready(long dd)
        {
            for (int i = 0; i < checkTables.Count; i++)
            {
                if (!checkTables[i].IsAchieve && dd > checkTables[i].TimeGOlal) 
                { 
                    checkTables[i].IsAchieve = true; return true; 
                }
            }
            return false;
        }
        public static bool ListInputTimeCheck(out long dd) 
        {
            dd = GetLastInputTime();
            return true;
              //  if(check_is_ready(dd)) return true; 
            //if (dd == 0)
            //{
            //    for (int j = 0; j < checkTables.Count; j++)
            //        checkTables[j].IsAchieve = false;
            //}
            //return false;
         }
        #endregion
        public static string GetInfor() {
          
            long dd;
            double hour;
            try
            {
                double.TryParse(File.ReadAllLines("config.ini")[0], out hour);
            }
            catch
            {
                hour = 5;
            }
            if (ListInputTimeCheck(out dd))
            {
              
               // File.AppendAllText(AppContext.BaseDirectory + "log.txt", DateTime.Now.ToString()+"时长达到：" + LayoutKind.formatLongToTimeStr(dd) + "。达到"+ hour.ToString()+"小时关机\r\n");
               double time= hour * 60 * 60 * 1000;
                if (dd > 0 && dd > time) 
                {

                    LayoutKind.SendEmailTo("关机原因：未操作时长达到："+LayoutKind.formatLongToTimeStr(dd)+"关机");
                    File.AppendAllText(AppContext.BaseDirectory + "log.txt", DateTime.Now.ToString() + "执行关机\r\n");
                    Thread.Sleep(1000);
                    Process.Start("shutdown", "/s /t 0");
                    return "执行关机;闲时时间：" + formatLongToTimeStr(dd) + "需闲时时间:" + hourtototal(hour) + "小时";

                }
                return "闲时时间："+ formatLongToTimeStr(dd) + "需闲时时间:" + hourtototal(hour) + "小时";

            };
            return "闲时时间：" + formatLongToTimeStr(dd) + "需闲时时间:" + hourtototal(hour) + "小时"; ;
        }
        public static string hourtototal(double a) {
        return $"{(int)a}小时 {(int)((a * 3600) % 3600 / 60)}分 {(int)((a * 3600) % 60)}秒" ;
        }
        public static string formatLongToTimeStr(long l)
        {
            int hour = 0;
            int minute = 0;
            int second = (int)l / 1000;

            if (second > 60)
            {
                minute = second / 60;
                second = second % 60;
            }
            if (minute > 60)
            {
                hour = minute / 60;
                minute = minute % 60;
            }
            return (hour.ToString() + "小时" + minute.ToString() + "分钟"
                + second.ToString() + "秒");
        }
        public static int HttpGet(string url, out string reslut, string token = "")
        {
            reslut = "";
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
                wbRequest.UserAgent = Environment.MachineName +" " + Environment.UserName;
                wbRequest.Proxy = null;
                wbRequest.Method = "GET";
                wbRequest.Headers.Add("Authorization",  token);
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sReader = new StreamReader(responseStream))
                    {
                        reslut = sReader.ReadToEnd();
                    }
                }
            }
            //catch (WebException e)
            //{
            //    using (WebResponse response = e.Response)
            //    {
            //        HttpWebResponse httpResponse = (HttpWebResponse)response;
            //        Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
            //        using (Stream data = response.GetResponseStream())
            //        using (var reader = new StreamReader(data))
            //        {
            //            // text is the response body
            //            string text = reader.ReadToEnd();
            //        }
            //    }
            //}
            catch (Exception e)
            {
                reslut = e.Message;     //输出捕获到的异常，用OUT关键字输出
                return -1;              //出现异常，函数的返回值为-1
            }
            return 0;
        }
        public static int HttpPost(string url, string sendData, out string reslut, string token = "")
        {
            reslut = "";
            try
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(sendData);
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);  // 制备web请求
                wbRequest.Proxy = null;     //现场测试注释掉也可以上传
                wbRequest.Method = "POST";
                wbRequest.ContentType = "application/json";
                wbRequest.ContentLength = data.Length;
                wbRequest.Headers.Add("Authorization", token);

                //api/Token/GetToken
                //#region //【1】获得请求流，OK
                //Stream newStream = wbRequest.GetRequestStream();
                //newStream.Write(data, 0, data.Length);
                //newStream.Close();//关闭流
                //newStream.Dispose();//释放流所占用的资源
                //#endregion

                #region //【2】将创建Stream流对象的过程写在using当中，会自动的帮助我们释放流所占用的资源。OK
                using (Stream wStream = wbRequest.GetRequestStream())         //using(){}作为语句，用于定义一个范围，在此范围的末尾将释放对象。
                {
                    wStream.Write(data, 0, data.Length);
                }
                #endregion

                //获取响应
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sReader = new StreamReader(responseStream, Encoding.UTF8))      //using(){}作为语句，用于定义一个范围，在此范围的末尾将释放对象。
                    {
                        reslut = sReader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                reslut = e.Message;     //输出捕获到的异常，用OUT关键字输出
                return -1;              //出现异常，函数的返回值为-1
            }
            return 0;
        }
        public static int HttpPost2(string url, string sendData, out string reslut, string token = "")
        {
            reslut = "";
            try
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(sendData);
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);  // 制备web请求
                wbRequest.Proxy = null;     //现场测试注释掉也可以上传
                wbRequest.Method = "POST";
                wbRequest.ContentType = "application/json";
                wbRequest.ContentLength = data.Length;
                wbRequest.Headers.Add("Authorization", token);

                //api/Token/GetToken
                //#region //【1】获得请求流，OK
                //Stream newStream = wbRequest.GetRequestStream();
                //newStream.Write(data, 0, data.Length);
                //newStream.Close();//关闭流
                //newStream.Dispose();//释放流所占用的资源
                //#endregion

                #region //【2】将创建Stream流对象的过程写在using当中，会自动的帮助我们释放流所占用的资源。OK
                using (Stream wStream = wbRequest.GetRequestStream())         //using(){}作为语句，用于定义一个范围，在此范围的末尾将释放对象。
                {
                    wStream.Write(data, 0, data.Length);
                }
                #endregion

                //获取响应
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sReader = new StreamReader(responseStream, Encoding.UTF8))      //using(){}作为语句，用于定义一个范围，在此范围的末尾将释放对象。
                    {
                        reslut = sReader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                reslut = e.Message;     //输出捕获到的异常，用OUT关键字输出
                return -1;              //出现异常，函数的返回值为-1
            }
            return 0;
        }

        public static void SendEmailTo(string text)
        {
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                using (SmtpClient smtpClient = new SmtpClient("smtp.qq.com", 587))
                {
                    mailMessage.To.Add("799942292@qq.com");//多个账号用逗号隔开
                    mailMessage.Body = text;
                    //设置邮件内容是否是 HTML 格式
                    mailMessage.IsBodyHtml = true;
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.From = new MailAddress("1243500742@qq.com", "给您关机啦！");
                    mailMessage.Subject = "计算机名" + System.Environment.MachineName + DateTime.Now.ToString();
                    mailMessage.SubjectEncoding = Encoding.UTF8;
                    smtpClient.EnableSsl = true;
                    //qq启用了“客户端授权码”，要用授权码代替密码
                    smtpClient.Credentials = new NetworkCredential("1243500742@qq.com", "wflauoxcgqsvbaei");
                    smtpClient.Send(mailMessage);
                }
            }catch (Exception ex) { File.AppendAllText(AppContext.BaseDirectory + "log.txt", DateTime.Now.ToString() + ex.Message+"\r\n"); }

        }

    }
}
