﻿using System;
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

namespace WindowApi
{
    public class Program
    {
        public static string filename = "config.ini";
        static void Main(string[] args)
        {
            LayoutKind.HttpGet("http://140.246.128.207:82/SetRedisPcOpen", out string reslut);
            Thread thread = new Thread(new ThreadStart(start));
            thread.IsBackground = false;
            thread.Start();
            if (!File.Exists(filename)) 
            File.WriteAllText(filename, "5");
           
        }
       static int cs1 = 0;
        private static void start()
        {
            while (true)
            {
                LayoutKind.GetInfor();
               // LayoutKind.HttpGet("http://140.246.128.207:82/GetRedisPcStatus", out string reslut1);
                LayoutKind.HttpGet("http://localhost:5000/GetRedisPcStatus", out string reslut1);
                if (reslut1.Contains("关机"))
                {
                    if (cs1 == 0 || cs1 % 10 == 0)
                    {

                        LayoutKind.SendEmailTo("执行了远程关机操作");
                        File.AppendAllText(AppContext.BaseDirectory + "log.txt", DateTime.Now.ToString() + "执行关机\r\n");
                        Thread.Sleep(1000);
                        Process.Start("shutdown", "/s /t 0");

                    }
                    cs1++;
                }
                else
                {
                    LayoutKind.HttpGet("http://140.246.128.207:82/SetRedisPcOpen", out string reslut);
                    try
                    {
                        runtimeout++;
                        if (runtimeout > 2)
                        {
                            Tongbu();
                            runtimeout= 0;
                        }
                    }
                    catch(Exception ex)
                    {
                        File.AppendAllText(AppContext.BaseDirectory + "log.txt", DateTime.Now.ToString() + ex.Message+"\r\n");
                    }
                }
                Thread.Sleep(10000);
            }

        }
        #region 文件同步
        public static void Tongbu() {
            LayoutKind.HttpPost("http://140.246.128.207:82/FilePc/GetFileList", "", out string reslutf);
            var value = new JavaScriptSerializer().Deserialize<ResponseFileList>(reslutf);
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
        #endregion

    }
}
