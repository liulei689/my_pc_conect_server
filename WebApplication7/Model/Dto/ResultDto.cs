using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.Net7.Api.Model.Dto
{
    /// <summary>
    /// 结果模型
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public class ResultDto<T>:ResultDto where T : class
    {
        public ResultDto()
        {

        }
        public ResultDto(T data, string message, bool statusFlag)
        {
            this.Data = data;
            this.Message = message;
            this.Status = statusFlag;
        }
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    
    }

    public class ResultDto
    {
        public ResultDto()
        {

        }

        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; } = true;
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        [Description("接口返回值")]
        public int ResposeCode { get; set; } = 200;

        //[Description("异常信息")]
        //public Exception Exception { get; set; }
    }
}
