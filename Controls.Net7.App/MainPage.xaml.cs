using Controls.Models;
using Controls.Net7.Api.Model.Dto;
using Flurl.Http;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;
using System.Diagnostics;

namespace Controls.Net7.App
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();

#if ANDROID

            Microsoft.Maui.Handlers.EntryHandler.Mapper.ModifyMapping("NoUnderline", (h, v, t) => { 
                h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform()); 
            });
#endif

            
        }

        bool isopen = false;
       public static string _token = "";
        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(500));
            while (await timer.WaitForNextTickAsync())
            {
                string status = "";
                try
                {
                     status = await (DefalutConfig.BaseUrl+"GetRedisPcStatus").WithOAuthBearerToken(_token). GetStringAsync();
                }catch(Exception ex) {
                    if (ex.Message.Contains("Unauthorized")) 
                    {
                        _token =   await (DefalutConfig.BaseUrl + "api/Token/GetToken").PostJsonAsync(new UserDto() { Password= "1", UserName="手机端操作",Role="1"}).ReceiveString();
                        status = await (DefalutConfig.BaseUrl +"GetRedisPcStatus").WithOAuthBearerToken(_token).GetStringAsync();

                    }
                }
            
                if (content.Text!=null && content.Text.Contains('\n') && content.Text.Split('\n').Length > 20) content.Text = "";
                content.Text = status+"\n"+ content.Text;
                if (status.Contains("关机"))
                {
                    imgpicstatus.Source = "state_3.png";
                    isopen = false;
                    //1

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
                        string ddd = await "http://140.246.128.207:82/SetRedisPcClose".WithOAuthBearerToken(_token).GetStringAsync();
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
                    string ddd = await ("http://140.246.128.207:82/SetRedisPcOpenByName?deviceStatus="+asdsd.Text).WithOAuthBearerToken(_token).GetStringAsync();
                    if (ddd == "true")
                    {
                        //await DisplayAlert("下发成功", "开机", "ok"); 
                    }
                    else await DisplayAlert("下发失败", "开机", "ok");


                }
                catch (Exception ex) { await DisplayAlert(ex.Message, "接口异常", "ok"); }

            }
        }

        private async void asdsd_TextChanged(object sender, TextChangedEventArgs e)
        {
            string ddd = await("http://140.246.128.207:82/SetRedisPcOpenByName?deviceStatus=" + asdsd.Text).WithOAuthBearerToken(_token).GetStringAsync();

        }
    }
}