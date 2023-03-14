using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using System.Net;
using System.Xml.Linq;

namespace WindowApi
{
    public class Program
    {

        public static string filename = "config.ini";
        public static string _token="";
        public static UserDto ud=null;
        static void Main(string[] args)
        {
   
            if (!File.Exists(filename))
                File.WriteAllText(filename, "2\r\nliu 1234567 ");
            ud = new UserDto();
            try
            {
                string[] datas = File.ReadAllLines("config.ini")[1].Split(' ');
                ud.UserName = datas[0];

                ud.Password = datas[1];
            }
            catch
            {
                ud.UserName = "liu";

                ud.Password = "1234567";
            }
            LayoutKind.HttpGet("http://140.246.128.207:82/SetRedisPcOpen", out string reslut, _token);           
           
                if (reslut.Contains("401"))
                {
                string to = new JavaScriptSerializer().Serialize(new { Password = ud.Password, UserName = ud.UserName });

                LayoutKind.HttpPost("http://140.246.128.207:82/api/Token/GetToken", to, out string reslut112);
                    _token = reslut112;
                    LayoutKind.HttpGet("http://140.246.128.207:82/SetRedisPcOpen", out string reslut445, _token);
              

            }
            
            Thread thread = new Thread(new ThreadStart(start));
            thread.IsBackground = false;
            thread.Start();
         
           
        }
       static int cs1 = 0;
        private static void start()
        {
            while (true)
            {
                string message = LayoutKind.GetInfor();
                LayoutKind.HttpGet("http://140.246.128.207:82/GetRedisPcStatus", out string reslut1, _token);
                if (reslut1.Contains("401"))
                {
                    string to = new JavaScriptSerializer().Serialize(new { Password = ud.Password, UserName = ud.UserName });

                    LayoutKind.HttpPost("http://140.246.128.207:82/api/Token/GetToken", to, out string reslut112);
                    ApiResult datoken = new JavaScriptSerializer().Deserialize<ApiResult>(reslut112);

                    _token = datoken.Data.ToString();
                    LayoutKind.HttpGet("http://140.246.128.207:82/GetRedisPcStatus", out string reslut13, _token);
                }
                try
                {
                    ApiResultD pcStatus = new JavaScriptSerializer().Deserialize<ApiResultD>(reslut1);
                    
                    if (pcStatus.Data.PcCmd == PcCmd.AddTime)
                    {
                        File.WriteAllText(filename, pcStatus.Data.TimeAdd + "\r\nliu 123456 ");
                        pcStatus.Data.PcCmd = PcCmd.Check;
                        pcStatus.Data.TimeAdd = "0";
                        pcStatus.Data.Time=DateTime.Now.ToString();
                        pcStatus.Data.Other = message;
                        string to = new JavaScriptSerializer().Serialize(pcStatus.Data);

                        LayoutKind.HttpPost("http://140.246.128.207:82/SetRedisPcCmd", to, out string reslut13, _token);
                        
                    }
                    else if (pcStatus.Data.PcCmd == PcCmd.Check)
                    {
                        pcStatus.Data.PcCmd = PcCmd.Check;
                        pcStatus.Data.Other = message;
                        pcStatus.Data.Time=DateTime.Now.ToString();   
                        string to2 = new JavaScriptSerializer().Serialize(pcStatus.Data);

                        LayoutKind.HttpPost("http://140.246.128.207:82/SetRedisPcCmd", to2, out string reslut134, _token);

                    }

                    if (pcStatus.Data.PcStatu == PcCmd.TurnOff)
                    {
                        if (cs1 == 0 || cs1 % 10 == 0)
                        {

                            LayoutKind.SendEmailTo("关机原因：上位机下发指令");
                            File.AppendAllText(AppContext.BaseDirectory + "log.txt", DateTime.Now.ToString() + "执行关机\r\n");
                            Thread.Sleep(1000);
                            Process.Start("shutdown", "/s /t 0");

                        }
                        cs1++;
                    }
                    else
                    {
                        try
                        {
                            runtimeout++;
                            if (runtimeout > 2)
                            {
                                Tongbu();
                                runtimeout = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            File.AppendAllText(AppContext.BaseDirectory + "log.txt", DateTime.Now.ToString() + ex.Message + "\r\n");
                        }
                    }
                    Thread.Sleep(10000);
                }
                catch { Thread.Sleep(10000); }
                }
        }
        public class PcStatus
        {
            public PcCmd PcStatu { get; set; } = PcCmd.TurnOn;
            public PcCmd PcCmd { get; set; } = PcCmd.Check;
            public string TimeAdd { get; set; } 

            public string PcIp { get; set; } = "127.0.0.1";
            public string PcName { get; set; } = "";
            public string PcLoginName { get; set; } = "";
            public string Time { get; set; }
            public string Other { get; set; } = "";
        }
        public enum PcCmd
        {
            Check,
            ServerUpdate,
            TurnOn,
            TurnOff,
            AddTime
        }
        public class ApiResult
        {
            /// <summary>
            /// 返回码
            /// </summary>
            public int Code { get; set; }
            /// <summary>
            /// 返回提示信息
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 返回数据
            /// </summary>
            public object Data { get; set; }
        }
        public class ApiResultD
        {
            /// <summary>
            /// 返回码
            /// </summary>
            public int Code { get; set; }
            /// <summary>
            /// 返回提示信息
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 返回数据
            /// </summary>
            public PcStatus Data { get; set; }
        }
        public class ApiResultF
        {
            /// <summary>
            /// 返回码
            /// </summary>
            public int Code { get; set; }
            /// <summary>
            /// 返回提示信息
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 返回数据
            /// </summary>
            public ResponseFileList Data { get; set; }
        }
        #region 文件同步
        public static void Tongbu() {
            LayoutKind.HttpPost("http://140.246.128.207:82/FilePc/GetFileList", "", out string reslutf, _token);
            var value2 = new JavaScriptSerializer().Deserialize<ApiResultF>(reslutf);
        
            ResponseFileList value = value2.Data;
            if (value != null && value.Counts > 0)
            {
                for (int i = 0; i < value.FileList.Count; i++)
                {
                    if (!findfile(value.FileList[i].FileName))
                    {
                        WebClient webClient = new WebClient();
                        webClient.DownloadFile(value.FileList[i].FileUrl, Path.Combine(filePath, value.FileList[i].FileName));

                    }
                }
            }
        }
        public static int runtimeout = 1;
        public static string filePath = "";
        public static bool findfile(string filename) {
             filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "云同步");
            if (!System.IO.Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

            var myfilelist = Directory.GetFiles(filePath);

            for (int j = 0; j < myfilelist.Length; j++)
            {
                if (filename == Path.GetFileName(myfilelist[j]))
                {
                    return true;
                }

            }
            return false;
        }
        public class ResponseFileList
        {
            public List<ResponseFile> FileList { get; set; }
            public int Counts { get; set; }

        }
        public class ResponseFile
        {
            public string FileUrl { get; set; }
            public string FileName { get; set; }
            public DateTime? CreateTime { get; set; }

        }
        public class UserDto
        {
            public string Password { get; set; }
            public string UserName { get; set; }
            public string Role { get; set; }

        }
        #endregion

    }
}
