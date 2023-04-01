using Controls.Models;
using Controls.Net7.Api.Model.Dto;
using Controls.Net7.App.Uintitys;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Controls.Net7.App.Views;

public partial class CodeManger : ContentPage
{
    public static UserManger userManger;
    public CodeManger()
	{
		InitializeComponent();
  
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
        var resp = await "alldata".GetUrl().PostAsync().ReceiveJson<CodesResult>();
        Filelist.ItemsSource = resp.Data;
        Filelist.IsRefreshing = false;
        DateTime timetoday = DateTime.Today;
        DateTime timelastweek = DateTime.Today.AddDays(-7);
        DateTime timelastmons = DateTime.Today.AddMonths(-1);
        var ltoday = resp.Data.FindAll(o => o.CreateTime.CompareTo(timetoday.ToString()) > 0);
        var llastweek = resp.Data.FindAll(o => o.CreateTime.CompareTo(timelastweek.ToString()) > 0);
        var llastmons = resp.Data.FindAll(o => o.CreateTime.CompareTo(timelastmons.ToString()) > 0);
        status.Text = "����������" + ltoday.Count().ToString()
        + " ��7�죺" + llastweek.Count().ToString()
        + " ��1�£�" + llastmons.Count().ToString()
        + " �Ķ���" + resp.Data.Sum(o => o.ReadCount).ToString()
        + " ������" + resp.Data.Count().ToString();
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
        string action = await DisplayActionSheet("ѡ�����?", "ȡ��", null, "ɾ��", "�޸�", "����");
        if (action == "ɾ��")
        {
            if (await DisplayAlert("ȷ��ɾ��?", "ȷ��ɾ��", "��", "��"))
            {              
                if (sd != null)
                {
                    string url = "DeleteUser?username=" + sd.UserName;
                    var resp = await url.GetUrl().WithOAuthBearerToken(MainPage._token).PostAsync().ReceiveString();
                    ContentPage_Loaded(null, null);
                }
            }
        }
        else if (action == "�޸�") {        
                await Navigation.PushAsync(new MessageBox(sd, "�޸�"));
        }
    }
    public async void update(UserDto o) {
        var resp = await "UpdateUser".GetUrl().WithOAuthBearerToken(MainPage._token).PostJsonAsync(o).ReceiveString();
    ContentPage_Loaded(null, null);
}
}
