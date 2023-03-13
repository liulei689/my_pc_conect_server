using Controls.Models;
using Controls.Net7.Api.Model.Dto;
using Controls.Net7.App.Views;
using Flurl.Http;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Controls.Net7.App
{
    public partial class MainPage : ContentPage
    {
        public static string username = "1";
        public static string passwword = "1";
        public static string role = "1";
        public static MainPage mainPage;
        public MainPage()
        {
            InitializeComponent();
#if ANDROID
            Microsoft.Maui.Handlers.EntryHandler.Mapper.ModifyMapping("NoUnderline", (h, v, t) => { 
                h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform()); 
            });
#endif
            mainPage = this;
            FlurlHttp.GlobalSettings.OnError = MyFlurlErrorHandler;
        }

        private async void MyFlurlErrorHandler(FlurlCall httpCall)
        {
            if (httpCall.Response.StatusCode == 401)       //401
            {
                var repons = await(DefalutConfig.BaseUrl + "api/Token/GetToken").PostJsonAsync(new { Password = passwword, UserName = username }).ReceiveJson<ApiResult>();
                _token = repons.Data.ToString().Replace("Bearer ", "");
               //  await (DefalutConfig.BaseUrl + "GetRedisPcStatus").WithOAuthBearerToken(_token).GetAsync().ReceiveJson<PcStatuResult>();

                httpCall.ExceptionHandled = true;
            }
        }

        bool isopen = false;
        public static string _token = "";
        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(500));
            while (await timer.WaitForNextTickAsync())
            {
                PcStatuResult status =new PcStatuResult() ;
                try
                {
                    status = await (DefalutConfig.BaseUrl + "GetRedisPcStatus").WithOAuthBearerToken(_token).GetAsync().ReceiveJson<PcStatuResult>();
                 }
                catch(FlurlHttpException ex)
                {
                    var dd = await ex.GetResponseJsonAsync<ApiResult>(); ;
                }
              
                {
            }

            if (content.Text != null && content.Text.Contains('\n') && content.Text.Split('\n').Length > 20) content.Text = "";
                var pcstauts =  status.Data;
                if (pcstauts != null) {
                        content.Text = pcstauts.PcStatu+"|"+pcstauts.PcCmd + "|" + pcstauts.PcName + "|" + pcstauts.TimeAdd + "|"+pcstauts.PcLoginName + "|" + pcstauts.PcIp + "|" + pcstauts.Time + "|" + pcstauts.Other +"\n" + content.Text;
                    if (pcstauts.PcStatu == PcCmd.TurnOff)
                    {
                        imgpicstatus.Source = "state_3.png";
                        isopen = false;
                    }
                    else
                    {
                        imgpicstatus.Source = "state_0.png";
                        isopen = true;
                    }
                }
            }
        }
        private async void imgpicstatus_Clicked(object sender, EventArgs e)
        {
            if (isopen)
            {
                bool answer = await DisplayAlert("关机", "确认关机", "是", "否");
                if (answer)
                {
                    try
                    {
                        PcStatuResult ddd = await (DefalutConfig.BaseUrl + "SetRedisPcClose").WithOAuthBearerToken(_token).GetAsync().ReceiveJson<PcStatuResult>();
                        if (ddd.Data.PcStatu == PcCmd.TurnOff)
                        {
                            // await DisplayAlert("下发成功", "关机", "ok");
                        }
                        else await DisplayAlert("下发失败", "关机", "ok");
                    }
                    catch (Exception ex) { await DisplayAlert(ex.Message, "接口异常", "ok"); }
                }
            }
            else
            {
                try
                {
                    ApiResult ddd = await (DefalutConfig.BaseUrl + "SetRedisPcOpen").WithOAuthBearerToken(_token).GetAsync().ReceiveJson<ApiResult>(); ;
                    if ((ddd.Data as JToken).ToObject<PcStatus>().PcStatu== PcCmd.TurnOn)
                    {
                        //await DisplayAlert("下发成功", "开机", "ok"); 
                    }
                    else await DisplayAlert("下发失败", "开机", "ok");
                }
                catch (Exception ex) { await DisplayAlert(ex.Message, "接口异常", "ok"); }
            }
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MessageBox(new UserDto() { UserName="1",Password="2",Role="1"}));
        }
        public async void gettoken(UserDto o)
        {
            passwword = o.Password;
            username= o.UserName;
            _token = await (DefalutConfig.BaseUrl + "api/Token/GetToken").PostJsonAsync(new { Password = o.Password, UserName = o.UserName }).ReceiveString();
        }
        private async void asdsd_TextChanged(object sender, TextChangedEventArgs e)
        {
            await (DefalutConfig.BaseUrl + "SetRedisPcOpenByName?deviceStatus=" + asdsd.Text).WithOAuthBearerToken(_token).GetStringAsync();
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {

            if (int.TryParse(asdsd.Text, out int da))
                await (DefalutConfig.BaseUrl + "SetRedisPcCmd").WithOAuthBearerToken(_token).PostJsonAsync(
                    new PcStatus()
                    {   PcIp="",
                        Other = "",
                        PcLoginName = "",
                        PcName = "",
                        TimeAdd = da,
                        Time = DateTime.Now.ToString(),
                        PcCmd = PcCmd.AddTime,
                        PcStatu = PcCmd.TurnOn,
                    }
                    ); 

        }
    }
}