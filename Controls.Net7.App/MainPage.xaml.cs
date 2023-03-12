using Controls.Models;
using Controls.Net7.Api.Model.Dto;
using Controls.Net7.App.Views;
using Flurl.Http;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;
using System.Diagnostics;
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
        }
        bool isopen = false;
        public static string _token = "";
        int timeout = 0;
        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(500));
            while (await timer.WaitForNextTickAsync())
            {
                string status = "";
                try
                {
                    timeout++;
                    status = await (DefalutConfig.BaseUrl + "GetRedisPcStatus").WithOAuthBearerToken(_token).GetStringAsync();
                    if (timeout >= 10)
                    {
                        await (DefalutConfig.BaseUrl + "SetRedisPcOpenByName?deviceStatus=检查开机" + username).WithOAuthBearerToken(_token).GetStringAsync();
                        timeout = 0;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Unauthorized"))
                    {
                        try
                        {
                            _token = await (DefalutConfig.BaseUrl + "api/Token/GetToken").PostJsonAsync(new { Password = passwword, UserName = username }).ReceiveString();
                            status = await (DefalutConfig.BaseUrl + "GetRedisPcStatus").WithOAuthBearerToken(_token).GetStringAsync();
                        }
                        catch (Exception ed)
                        {
                            status = ed.Message;
                        }
                    }
                    status = ex.Message + _token;
                }
                if (content.Text != null && content.Text.Contains('\n') && content.Text.Split('\n').Length > 20) content.Text = "";
                if(!status.Contains("检查开机"))
                content.Text = status + "\n" + content.Text;
                if (status.Contains("关机"))
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
        private async void imgpicstatus_Clicked(object sender, EventArgs e)
        {
            if (isopen)
            {
                bool answer = await DisplayAlert("关机", "确认关机", "是", "否");
                if (answer)
                {
                    try
                    {
                        string ddd = await (DefalutConfig.BaseUrl + "SetRedisPcClose").WithOAuthBearerToken(_token).GetStringAsync();
                        if (ddd == "true")
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
                    string ddd = await (DefalutConfig.BaseUrl + "SetRedisPcOpenByName?deviceStatus=" + asdsd.Text).WithOAuthBearerToken(_token).GetStringAsync();
                    if (ddd == "true")
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
    }
}