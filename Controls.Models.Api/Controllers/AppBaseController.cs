
using Controls.Models;
using Microsoft.AspNetCore.Mvc;

namespace Controls.Net7.Api.Controllers
{
    ///// <summary>
    ///// 注入服务基础控制器
    ///// </summary>
    ///// <typeparam name="TDefaultService"></typeparam>
    //public class AppBaseController<TDefaultService> : AppBaseController
    //    where TDefaultService : class
    //{
    //    protected readonly TDefaultService _service;

    //    public AppBaseController(TDefaultService service)
    //    {
    //        _service = service;
    //    }

    //}

    /// <summary>
    /// 基础控制器
    /// </summary>
    [ApiController]
    //[Route("controller")]
    public class AppBaseController : ControllerBase
    {

        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="apiResultCodeEnum"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [NonAction]
        public ApiResult Result(ApiResult.ApiResultCodeEnum apiResultCodeEnum, string message, object data)
            => ApiResult.Result(apiResultCodeEnum, message, data);

        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [NonAction]
        public ApiResult Result(int code, string message, object data)
            => ApiResult.Result(code, message, data);

        #region Ok

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        public ApiResult ResultOk(string message) => ApiResult.OkMessage(message);

        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [NonAction]
        public ApiResult ResultOk(object data) => ApiResult.OkData(data);

        /// <summary>
        /// 返回消息和数据
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [NonAction]
        public ApiResult ResultOk(string message, object data) => ApiResult.Ok(message, data);

        #endregion

        #region 警告

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        public ApiResult ResultWarn(string message) => ApiResult.WarnMessage(message);

        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [NonAction]
        public ApiResult ResultWarn(object data) => ApiResult.WarnData(data);

        /// <summary>
        /// 返回消息和数据
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [NonAction]
        public ApiResult ResultWarn(string message, object data) => ApiResult.Warn(message, data);

        #endregion

    }









}
