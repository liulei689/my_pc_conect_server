using Controls.Models;
using Controls.Net7.Api.Commons.Jwt;
using Controls.Net7.Api.Services;
using System.Text.Json;

namespace Controls.Net7.Api.Untiys
{
    public static class Common
    {
        public static ApiResult SetPcSatusReponse(string PcStatu, HttpContext httpcontext,IJWTManager _iJWTManager, HttpRequest httpRequest,IRedisService _redisService, string Cmd = "检查开机") {

            //获取JWT
            string AuthorizationToken = httpcontext.Request.Headers["Authorization"].FirstOrDefault();
            string JwtToken = AuthorizationToken!.Split(' ')[1];
            var u = _iJWTManager.SerializeJwt(JwtToken);
            try
            {
                var rpd = new PcStatus()
                {
                    PcStatu = PcStatu,
                    PcName = "",
                    PcCmd= Cmd,
                    PcLoginName = u.User.UserName,
                    PcIp = httpRequest?.HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString(),
                    Time =DateTime.Now,
                    Other = httpRequest.Headers.UserAgent
                };
                _redisService.Database.StringSet($"家-台式电脑-状态", JsonSerializer.Serialize(rpd));
                return ApiResult.OkData(rpd);
            }
            catch (Exception ex)
            {
                return ApiResult.WarnData(ex);

            }
        }
    }
}
