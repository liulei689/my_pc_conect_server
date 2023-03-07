﻿using System;
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
                if (!checkTables[i].IsAchieve && dd > checkTables[i].TimeGOlal) { checkTables[i].IsAchieve = true; return true; }
            }
            return false;
        }
        public static bool ListInputTimeCheck(out long dd) 
        {
            dd = GetLastInputTime();     
                if(check_is_ready(dd)) return true; 
            if (dd == 0)
            {
                for (int j = 0; j < checkTables.Count; j++)
                    checkTables[j].IsAchieve = false;
            }
            return false;
         }
        #endregion
        static bool  a,b,c,d,e,f,g,h,i,j,k,l,m,n =false;
        public static bool lastinputinfi(out long dd) {
            dd = LayoutKind.GetLastInputTime();
       
            //5小时
            if (!a &&dd > 5 * 60 * 60 * 1000) {  a = true; return true; }
            //4小时
            if (!b && dd > 4 * 60 * 60 * 1000) { b = true; return true; }
            //3小时
            if (!c && dd > 3 * 60 * 60 * 1000) { c = true; return true; }
            //2小时
            if (!d && dd > 2 * 60 * 60 * 1000) { d = true; return true; }
            //1小时
            if (!e && dd > 60 * 60 * 1000) {  e = true; return true; }
            //30分钟
            if (!h && dd > 30 * 60 * 1000) { h = true; return true; }
            //10分钟
            if (!i && dd > 10 * 60 * 1000) { i = true; return true; }
            //5分钟
            if (!j && dd > 5 * 60 * 1000) { j = true; return true; }
            //1分钟
            if (!k && dd > 1 * 60 * 1000)
            {
                k = true;
                return true;
            }
            if (!l  && dd > 30 * 1000)
            {
               
                l = true;
                return true;
            }
            if (!m && dd > 10 * 1000)
            {
                m = true;
                return true;
            }
            if (!n && dd > 5 * 1000)
            {
                n = true;
                return true;
            }
            if (dd == 0)
            {
                a = false;
                b = false;
                c = false;
                d = false;
                e = false;
                f = false;
                g = false;
                h = false;
                i = false;
                j = false;
                k = false;
                l = false;
                m = false;
                n = false;
            }
           
            return false;
        }
        public static void GetInfor() {
          
            long dd;
            if (ListInputTimeCheck(out dd))
            {
                double hour;
                try
                {
                    double.TryParse(File.ReadAllLines("config.ini")[0], out hour);
                }
                catch
                {
                    hour = 5;
                }
                File.AppendAllText(AppContext.BaseDirectory + "log.txt", DateTime.Now.ToString()+"时长达到：" + LayoutKind.formatLongToTimeStr(dd) + "。达到"+ hour.ToString()+"小时关机\r\n");
               double time= hour * 60 * 60 * 1000;
                if (dd > 0 && dd > time) 
                {
                    LayoutKind.SendEmailTo(LayoutKind.formatLongToTimeStr(dd));
                    File.AppendAllText(AppContext.BaseDirectory + "log.txt", DateTime.Now.ToString() + "执行关机\r\n");
                    Thread.Sleep(1000);
                    Process.Start("shutdown", "/s /t 0"); 
                }
            };
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
        public static int HttpGet(string url, out string reslut)
        {
            reslut = "";
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
                wbRequest.UserAgent = Environment.MachineName +" " + Environment.UserName;
                wbRequest.Proxy = null;
                wbRequest.Method = "GET";
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sReader = new StreamReader(responseStream))
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
        public static int HttpPost(string url, string sendData, out string reslut)
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