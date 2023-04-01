using Controls.Models;
using Controls.Net7.Api.Commons.Jwt;
using Controls.Net7.Api.Untiys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controls.Net7.Api.Controllers
{
    /// <summary>
    /// �ļ�����
    /// </summary>
    [Route("[controller]")]
    [Authorize("�����ӿ�")]

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
        /// �ϴ��ļ�
        /// </summary>
        /// <param name="files">�ļ�</param>
        /// <param name="filename">�ļ���</param>
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
                        if (filename == "ԭ��") filename = Path.GetFileNameWithoutExtension(formFile.FileName);
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
        /// ��ȡ�ļ��б�
        /// </summary>
        /// <returns></returns>
        [Route("GetFileList")]
        [HttpPost]
        public ApiResult GetFileListAsync()
        {
            //��ȡJWT
            string AuthorizationToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            string JwtToken = AuthorizationToken!.Split(' ')[1];

            //����JWT�е��û���Ϣ
            var u = _iJWTManager.SerializeJwt(JwtToken);
            var filePath = FileHelper.GetBasePath();
            if (!System.IO.Directory.Exists(filePath)) return ResultWarn("δ�ҵ���վ��Ŀ¼");
            return ResultOk(FileHelper.GetFilelist());
        }
        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="filename">�ļ��� 9527ȫɾ</param>
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
        /// �޸�
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