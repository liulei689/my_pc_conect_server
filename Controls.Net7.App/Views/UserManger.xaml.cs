using Controls.Models;
using Controls.Net7.Api.Model.Dto;
using Controls.Net7.App.Uintitys;
using Flurl.Http;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Controls.Net7.App.Views;

public partial class UserManger : ContentPage
{
    public static UserManger userManger;
    public UserManger()
	{
		InitializeComponent();
        picker.ItemsSource = new string[]{ "原名","兴业银行截图", "美女", "拍照", "萝莉", "风景", "设备", "配置", "服务器", "代码" };
        picker.SelectedIndex = 0;
        userManger = this;
#if WINDOWS
      
        
        //Filelist.RowHeight= 200;


#endif
    }
    public async void AddUser(UserDto o)
    {
        try
        {
            await "AddUser".GetUrl().PostJsonAsync(o).ReceiveString();
            ContentPage_Loaded(null, null);
        }
        catch { }

    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MessageBox(new UserDto() { UserName = "1", Password = "2", Role = "1" }, ""));


    }
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        Filelist.IsRefreshing = true;
        var resp = await "SelectAllUser".GetUrl().PostAsync().ReceiveJson<IEnumerable<UserDto>>();
        Filelist.ItemsSource = resp;
        Filelist.IsRefreshing = false;

    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
       var ds= sender as ImageButton;
        await Navigation.PushAsync(new ImageZoom(ds.Source));
       // await Navigation.PopAsync();

    }

    private void Filelist_Refreshing(object sender, EventArgs e)
    {
        ContentPage_Loaded(null, null);
    }

    private async void Filelist_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var sd = Filelist.SelectedItem as UserDto;
        string action = await DisplayActionSheet("选择操作?", "取消", null, "删除", "修改", "新增");
        if (action == "删除")
        {
            if (await DisplayAlert("确定删除?", "确定删除", "是", "否"))
            {              
                if (sd != null)
                {
                    string url = "DeleteUser?username=" + sd.UserName;
                    var resp = await url.GetUrl().WithOAuthBearerToken(MainPage._token).PostAsync().ReceiveString();
                    ContentPage_Loaded(null, null);
                }
            }
        }
        else if (action == "修改") {        
                await Navigation.PushAsync(new MessageBox(sd, "修改"));
        }
    }
    public async void update(UserDto o) {
        var resp = await "UpdateUser".GetUrl().WithOAuthBearerToken(MainPage._token).PostJsonAsync(o).ReceiveString();
    ContentPage_Loaded(null, null);
}
}
