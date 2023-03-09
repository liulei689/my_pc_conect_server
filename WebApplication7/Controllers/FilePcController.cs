using Controls.Models;
using Controls.Net7.Api.Jwt;
using Controls.Net7.Api.Model;
using Controls.Net7.Api.Redis;
using Controls.Net7.Api.Untiys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Controls.Net7.Api.Controllers
{
    /// <summary>
    /// �ļ�����
    /// </summary>
    [Authorize("�����ӿ�")]
    [ApiController]
    [Route("[controller]")]
    public class FilePcController : ControllerBase
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
        public async Task<IActionResult> ImageAsync(List<IFormFile> files,string filename)
        {
            var filePath = FileHelper.GetBasePath();
            if (!System.IO.Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    if (filename == "ԭ��") filename = Path.GetFileNameWithoutExtension(formFile.FileName);
                    var path = Path.Combine(filePath, filename+System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + Path.GetExtension(formFile.FileName.ToString()));

                    using (var stream = System.IO.File.Create(path))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                }
            }     
            return new JsonResult(FileHelper.GetFilelist());
        }
        /// <summary>
        /// ��ȡ�ļ��б�
        /// </summary>
        /// <returns></returns>
        [Route("GetFileList")]
        [HttpPost]
        public IActionResult GetFileListAsync()
        {
            //��ȡJWT
            string AuthorizationToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            string JwtToken = AuthorizationToken!.Split(' ')[1];

            //����JWT�е��û���Ϣ
            var u = _iJWTManager.SerializeJwt(JwtToken);
            var filePath = FileHelper.GetBasePath();
            if (!System.IO.Directory.Exists(filePath)) return Ok(null);
            return new JsonResult(FileHelper.GetFilelist());
        }
        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="filename">�ļ��� 9527ȫɾ</param>
        /// <returns></returns>
        [Route("Delete")]
        [HttpPost]
        public  IActionResult DeleteAsync(string filename)
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
            return Ok(new { counts = counts, data = filelist });
        }
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="oldfilename"></param>
        /// <param name="newfilename"></param>
        /// <returns></returns>
        [Route("Update")]
        [HttpPost]
        public IActionResult UpdateAsync(string oldfilename,string newfilename)
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
  
            return new JsonResult(FileHelper.GetFilelist());
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return id == 1 ? NoContent(): Ok("1212");
        }
    }
}