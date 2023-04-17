using Controls.Models;
using Controls.Net7.Api.Model.Dto;
using Controls.Net7.App.Uintitys;
using Flurl.Http;

namespace Controls.Net7.App.Views;

public partial class CodeManger : ContentPage
{
    public static UserManger userManger;
    public CodeManger()
	{
		InitializeComponent();
    }
    public List<Codess> codesses;
    public string counts;

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        Filelist.IsRefreshing = true;
        var resp = await "alldata".GetUrl().PostAsync().ReceiveJson<CodesResult>();
        Filelist.ItemsSource = resp.Data.OrderByDescending(o=>DateTime.Parse(o.CreateTime));        
        codesses =resp.Data;
        DateTime timetoday = DateTime.Today.AddHours(-12);
        DateTime timelastweek = DateTime.Today.AddDays(-7);
        DateTime timelastmons = DateTime.Today.AddMonths(-1);
        var ltoday = resp.Data.FindAll(o => DateTime.Parse(o.CreateTime).CompareTo(timetoday) > 0);
        var llastweek = resp.Data.FindAll(o => DateTime.Parse(o.CreateTime).CompareTo(timelastweek) > 0);
        var llastmons = resp.Data.FindAll(o => DateTime.Parse(o.CreateTime).CompareTo(timelastmons) > 0);
        Filelist.IsRefreshing = false;
        status.Text = "今日新增：" + ltoday.Count().ToString()
        + " 近7天：" + llastweek.Count().ToString()
        + " 近1月：" + llastmons.Count().ToString()
        + " 阅读：" + resp.Data.Sum(o => o.ReadCount).ToString()
        + " 总数：" + resp.Data.Count().ToString();
    }
    private void Filelist_Refreshing(object sender, EventArgs e)
    {
        ContentPage_Loaded(null, null);
    }

    private async void Filelist_ItemTapped(object sender, ItemTappedEventArgs e)
    {

        var sd = Filelist.SelectedItem as Codess;
        sd.ReadTime = DateTime.Now.ToString();
        sd.ReadCount++;
        var resp2 = await "updatedata".GetUrl().PostJsonAsync(sd).ReceiveString();

        await Navigation.PushAsync(new CodePage(sd));
    }
    public async void update(UserDto o) {
        var resp = await "UpdateUser".GetUrl().WithOAuthBearerToken(MainPage._token).PostJsonAsync(o).ReceiveString();
    ContentPage_Loaded(null, null);
}

    private void search_TextChanged(object sender, TextChangedEventArgs e)
    {
        var ds= codesses.FindAll(o => o.Use.Contains((sender as Entry).Text));
        Filelist.ItemsSource = ds;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        counts= codesses.Max(o => int.Parse(o._id)).ToString();
         await Navigation.PushAsync(new CodeAdd(counts,new Codess()));
       
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        ContentPage_Loaded(null, null);
        base.OnNavigatedTo(args);
    }
}
