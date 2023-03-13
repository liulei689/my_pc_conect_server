using Controls.Models;
using Controls.Net7.Api.Commons.Jwt;
using Controls.Net7.Api.Services;
using System.Text.Json;

namespace Controls.Net7.Api.Untiys
{
    public static class Common
    {
        public static ApiResult SetPcSatusReponse( HttpContext httpcontext, IJWTManager _iJWTManager, HttpRequest httpRequest, IRedisService _redisService, PcCmd Cmd = PcCmd.Check, PcStatus pcStatus = null) {

            //获取JWT
      
            try
            {
                string username = "";
                try
                {
                    string AuthorizationToken = httpcontext.Request.Headers["Authorization"].FirstOrDefault();
                    string JwtToken = AuthorizationToken!.Split(' ')[1];
                    username = _iJWTManager.SerializeJwt(JwtToken).User.UserName;
                }
                catch { username = "lllleeee"; }
                PcStatus rpd;
                if (pcStatus!=null)
                    rpd = pcStatus;
                else
                {
                    rpd = new PcStatus()
                    {
                        PcStatu = Cmd,
                        PcName = "",
                        PcCmd = Cmd,
                        PcLoginName = username,
                        PcIp = httpRequest?.HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString(),
                        Time = DateTime.Now.ToString(),
                        Other = httpRequest.Headers.UserAgent
                    };
                }
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
