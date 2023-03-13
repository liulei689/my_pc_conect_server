namespace Controls.Models
{
    public class BaseResult 
    {
        public int Code { get; set; }
        /// <summary>
        /// 返回提示信息
        /// </summary>
        public string Message { get; set; }
    }
    public class PcStatuResult:BaseResult
    {
        public PcStatus Data { get; set; }
    }
    /// <summary>
    /// Api 消息返回类基础模板
    /// </summary>
    public class ApiResult: BaseResult
    {
        public ApiResult(int code, string message, object data)
        {
            Code = code;
            Message = message;
            Data = data;
        }
        public ApiResult()
        {
        }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 消息返回码
        /// </summary>
        public enum ApiResultCodeEnum
        {
            /// <summary>
            /// 接口不存在
            /// </summary>
            NotFount = -3,

            /// <summary>
            /// 程序错误
            /// </summary>
            Error=400,

            /// <summary>
            /// 未授权
            /// </summary>
            UnAuth=401,

            /// <summary>
            /// 警告
            /// </summary>
            Warn=402,
            /// <summary>
            /// 无权限
            /// </summary>
            Forbidden = 403,
            /// <summary>
            /// 成功
            /// </summary>
            Ok =200,
        }

        #region 返回数据

        /// <summary>
        /// 只返回消息提醒
        /// </summary>
        /// <param name="apiResultCodeEnum"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult ResultMessage(ApiResultCodeEnum apiResultCodeEnum, string message)
            => new ApiResult((int) apiResultCodeEnum, message, null);

        /// <summary>
        /// 只返回数据
        /// </summary>
        /// <param name="apiResultCodeEnum"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult ResultData(ApiResultCodeEnum apiResultCodeEnum, object data)
            => new ApiResult((int) apiResultCodeEnum, null, data);

        /// <summary>
        /// 可返回消息和数据
        /// </summary>
        /// <param name="apiResultCodeEnum"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult Result(ApiResultCodeEnum apiResultCodeEnum, string message, object data)
            => new ApiResult((int) apiResultCodeEnum, message, data);

        #endregion

        #region result code 可传入 int

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult ResultMessage(int code, string message)
            => new ApiResult(code, message, null);

        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult ResultData(int code, object data)
            => new ApiResult(code, null, data);

        /// <summary>
        /// 可返回消息和数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult Result(int code, string message, object data)
            => new ApiResult(code, message, data);

        #endregion

        #region 返回成功带数据

        /// <summary>
        /// 成功 可返回消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult OkMessage(string message)
            => ResultMessage(ApiResultCodeEnum.Ok, message);

        /// <summary>
        /// 成功 可返回数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult OkData(object data)
            => ResultData(ApiResultCodeEnum.Ok, data);

        /// <summary>
        /// 成功 可返回 消息和数据
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult Ok(string message, object data)
            => Result(ApiResultCodeEnum.Ok, message, data);

        #endregion

        #region 返回提醒

        /// <summary>
        /// 警告 可返回消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult WarnMessage(string message)
            => ResultMessage(ApiResultCodeEnum.Warn, message);

        /// <summary>
        /// 警告 可返回数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult WarnData(object data)
            => ResultData(ApiResultCodeEnum.Warn, data);

        /// <summary>
        /// 警告 可返回 消息和数据
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult Warn(string message, object data)
            => Result(ApiResultCodeEnum.Warn, message, data);

        #endregion
    }
}