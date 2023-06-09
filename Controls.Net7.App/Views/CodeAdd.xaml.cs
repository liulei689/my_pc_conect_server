using Controls.Models;
using Controls.Net7.Api.Model.Dto;
using Controls.Net7.App.Uintitys;
using Flurl.Http;
using System.Globalization;
using System.Runtime.Serialization;

namespace Controls.Net7.App.Views;

public partial class CodeAdd : ContentPage
{
    public string _counts ;
	public CodeAdd(string counts,Codess dto=null)
	{
        InitializeComponent();
        _counts = counts;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    { 
        var dto =new Codess();
        dto.CreateTime = DateTime.Now.ToString("yyyy/M/d H:mm:ss", DateTimeFormatInfo.InvariantInfo);
        dto.TimeUpate = dto.CreateTime;
        dto.ReadCount = 0;
        dto.ReadTime = dto.CreateTime;
        dto.From = "������";
        dto.Technical = "������";
        dto.Code = Code.Text;
        dto.Use=Use.Text;
        dto.UseDetail = UseDetail.Text; 
        dto._id = (int.Parse(_counts)+1).ToString();
        try
        {
            await "adddata".GetUrl().PostJsonAsync(dto).ReceiveString();
            await Navigation.PopAsync();
        }
        catch (Exception ex) { 
        }
  
       
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