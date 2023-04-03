using Controls.Models;
using Controls.Net7.Api.Model.Dto;
using Controls.Net7.App.Uintitys;
using Flurl.Http;
using System.Globalization;
using System.Runtime.Serialization;

namespace Controls.Net7.App.Views;

public partial class CodeAdd : ContentPage
{
    public int _counts = 0;
	public CodeAdd(int counts,Codess dto=null)
	{
        InitializeComponent();
        _counts = counts;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    { 
        var dto =new Codess();
        dto.CreateTime = DateTime.Now.ToString("yyyy/M/d H:m:ss", DateTimeFormatInfo.InvariantInfo);
        dto.TimeUpate = dto.CreateTime;
        dto.ReadCount = 0;
        dto.ReadTime = dto.CreateTime;
        dto.From = "»¥ÁªÍø";
        dto.Technical = "»¥ÁªÍø";
        dto.Code = Code.Text;
        dto.Use=Use.Text;
        dto.UseDetail = UseDetail.Text; 
        dto._id = (_counts+1).ToString();
        await "adddata".GetUrl().PostJsonAsync(dto).ReceiveString();
       await Navigation.PopAsync();
       
    }

    private async void Use_TextChanged(object sender, TextChangedEventArgs e)
    {
        UseDetail.Text=Use.Text;
        if (Clipboard.Default.HasText)
        {
            Code.Text = await Clipboard.Default.GetTextAsync();
            //await ClearClipboard();
        }
        else
            Code.Text = "";
    }
}