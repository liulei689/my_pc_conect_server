using Controls.Models;
using Controls.Net7.Api.Model.Dto;
using Controls.Net7.App.Uintitys;
using Flurl.Http;
using System.Globalization;
using System.Runtime.Serialization;

namespace Controls.Net7.App.Views;

public partial class CodeAdd : ContentPage
{

	public CodeAdd(Codess dto=null)
	{
        InitializeComponent();
        dto.CreateTime=DateTime.Now.ToString("yyyy/M/d H:m:ss",DateTimeFormatInfo.InvariantInfo);
        dto.TimeUpate=dto.CreateTime;
        dto.ReadCount=0;
        dto.ReadTime=dto.CreateTime;
        dto.From = "»¥ÁªÍø";
        dto.Technical = "»¥ÁªÍø";
        dto.Code = "";
        
        Codessm.Root.BindingContext = dto; 
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
       var codes= Codessm.Root.BindingContext as Codess;
        codes._id = codes.Use;
       await "adddata".GetUrl().PostJsonAsync(codes).ReceiveString();
       await Navigation.PopAsync();
    }
}