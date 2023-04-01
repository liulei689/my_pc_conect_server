using Controls.Models;
using Controls.Net7.Api.Commons.Jwt;
using Controls.Net7.Api.Untiys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controls.Net7.Api.Controllers
{
    /// <summary>
    /// 文件管理
    /// </summary>
    [Route("[controller]")]
    [Authorize("公共接口")]

    public class FilePcController : AppBaseController
    {
        private readonly ILogger<FilePcController> _logger;
        private readonly IJWTManager _iJWTManager;
        public FilePcController(ILogger<FilePcController> logger ,IJWTManager iJWTManager)
        {
            _logger = logger;
            _iJWTManager = iJWTManager;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="files">文件</param>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        [Route("UploadImage")]
        [HttpPost]
        public async Task<ApiResult> ImageAsync(List<IFormFile> files,string filename)
        {
            try
            {
                var filePath = FileHelper.GetBasePath();
                if (!System.IO.Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        if (filename == "原名") filename = Path.GetFileNameWithoutExtension(formFile.FileName);
                        var path = Path.Combine(filePath, filename + System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + Path.GetExtension(formFile.FileName.ToString()));

                        using (var stream = System.IO.File.Create(path))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                    }
                }
              
            }
            catch(Exception e) { ResultWarn(e.Message); }
            return ResultOk(FileHelper.GetFilelist());
        }
        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <returns></returns>
        [Route("GetFileList")]
        [HttpPost]
        public ApiResult GetFileListAsync()
        {
            //获取JWT
            string AuthorizationToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            string JwtToken = AuthorizationToken!.Split(' ')[1];

            //解析JWT中的用户信息
            var u = _iJWTManager.SerializeJwt(JwtToken);
            var filePath = FileHelper.GetBasePath();
            if (!System.IO.Directory.Exists(filePath)) return ResultWarn("未找到网站根目录");
            return ResultOk(FileHelper.GetFilelist());
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filename">文件名 9527全删</param>
        /// <returns></returns>
        [Route("Delete")]
        [HttpPost]
        public ApiResult DeleteAsync(string filename)
        {
            var filePath = FileHelper.GetBasePath();
            var filelist = Directory.GetFiles(filePath);
            int counts = 0;
            for (int i = 0; i < filelist.Length; i++)
            {
                if (Path.GetFileName(filelist[i])==filename || filename=="9527")
                {
                   System.IO.File.Delete(filelist[i]);
                    counts++;
                }
                   
            }
            return ResultOk(new { counts = counts, data = filelist });
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="oldfilename"></param>
        /// <param name="newfilename"></param>
        /// <returns></returns>
        [Route("Update")]
        [HttpPost]
        public ApiResult UpdateAsync(string oldfilename,string newfilename)
        {
            var filePath = FileHelper.GetBasePath();
            var filelist = Directory.GetFiles(filePath);
            for (int i = 0; i < filelist.Length; i++)
            {
                if (Path.GetFileName(filelist[i]) == oldfilename)
                {
                    if (!System.IO.File.Exists(newfilename))
                    {
                        Microsoft.VisualBasic.FileIO.FileSystem.RenameFile(filelist[i], newfilename);
                    }
                }
            }
  
            return ResultOk(FileHelper.GetFilelist());
        }

    }
}