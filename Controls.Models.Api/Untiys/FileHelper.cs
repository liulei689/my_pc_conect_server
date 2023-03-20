using Controls.Models;
using System.Reflection;

namespace Controls.Net7.Api.Untiys
{
    public class FileHelper
    {
        public static string GetBasePath()
        {
          return  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");
        }
        public static ResponseFileList GetFilelist()
        {
            var filePath = GetBasePath();
           var filelist = Directory.GetFiles(filePath);
            var rps = new ResponseFileList();
            List<ResponseFile> lists = new List<ResponseFile>();
            for (int i = 0; i < filelist.Length; i++)
            {
                ResponseFile responseFileList = new ResponseFile();
                responseFileList.FileUrl = "http://124.221.160.244/" + filelist[i].Replace(filePath, "").Replace("\\", "");
                responseFileList.CreateTime = new FileInfo(filelist[i]).CreationTime; ;
                responseFileList.FileName = Path.GetFileName(filelist[i]);
                lists.Add(responseFileList);
            }
            rps.FileList = lists;
            rps.Counts = filelist.Length;
            return rps;
        }
    }
}
