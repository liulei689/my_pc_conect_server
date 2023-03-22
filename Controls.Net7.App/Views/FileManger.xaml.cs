using Controls.Models;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;

namespace Controls.Net7.App.Views;

public partial class FileManger : ContentPage
{
	public FileManger()
	{
		InitializeComponent();
        picker.ItemsSource = new string[]{ "原名","兴业银行截图", "美女", "拍照", "萝莉", "风景", "设备", "配置", "服务器", "代码" };
        picker.SelectedIndex = 0;
#if WINDOWS
      
        
        //Filelist.RowHeight= 200;


#endif
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
      var res =await PickAndShow();
        Filelist.IsRefreshing = true;

        if (res != null)
        {
            string url = "http://124.221.160.244/FilePc/UploadImage?filename=" + picker.SelectedItem.ToString();
            var ls =res.ToList();
            for (int m = 0; m < ls.Count; m++)
            {
                var resp = await url.WithOAuthBearerToken(MainPage._token).PostMultipartAsync(mp => mp
            .AddFile("files", ls[m].FullPath)).ReceiveJson<ResponseFileList>();
            }
          
        }
        Filelist.IsRefreshing = false;
        ContentPage_Loaded(null, null);

    }
    public async Task<IEnumerable<FileResult>> PickAndShow()
    {
        try
        {
            //var customFileType = new FilePickerFileType(
            //    new Dictionary<DevicePlatform, IEnumerable<string>>
            //    {
            //        { DevicePlatform.iOS, new[] { "public.my.comic.extension" } }, // UTType values
            //        { DevicePlatform.Android, new[] { "*/*" } }, // MIME type
            //        { DevicePlatform.WinUI, new[] { "*/*" } }, // file extension
            //        { DevicePlatform.Tizen, new[] { "*/*" } },
            //        { DevicePlatform.macOS, new[] { "cbr", "cbz" } }, // UTType values
            //    });

            PickOptions options = new()
            {
                PickerTitle = "Please select a comic file",
            
            };
            return await FilePicker.Default.PickMultipleAsync(options);
      
        }
        catch (Exception ex)
        {
            await DisplayAlert(ex.Message,"","");
            // The user canceled or something went wrong
        }

        return null;
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        Filelist.IsRefreshing = true;
        string url = "http://124.221.160.244/FilePc/GetFileList";
        var resp = await url.WithOAuthBearerToken(MainPage._token).PostAsync().ReceiveJson<ApiResult>();
        Filelist.ItemsSource = (resp.Data as JToken).ToObject<ResponseFileList>().FileList;
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
        var sd = Filelist.SelectedItem as ResponseFile;
        string action = await DisplayActionSheet("选择操作?", "取消", null, "删除", "修改", "新增");
        if (action == "删除")
        {
            if (await DisplayAlert("确定删除?", "确定删除", "是", "否"))
            {              
                if (sd != null)
                {
                    string url = "http://124.221.160.244/FilePc/Delete?filename=" + sd.FileName;
                    var resp = await url.WithOAuthBearerToken(MainPage._token).PostAsync().ReceiveString();
                    ContentPage_Loaded(null, null);
                }
            }
        }
        else if (action == "修改") {
            string result = await DisplayPromptAsync("修改","修改中...", placeholder: sd.FileName,initialValue: sd.FileName,accept:"确定",cancel:"取消");
            if (result != "" && result.Contains("."))
            {
                string url = $"http://124.221.160.244/FilePc/Update?oldfilename={sd.FileName}&newfilename={result}";
                var resp = await url.WithOAuthBearerToken(MainPage._token).PostAsync().ReceiveJson<ApiResult>(); ;
                Filelist.ItemsSource = (resp.Data as JToken).ToObject<ResponseFileList>().FileList;
                Filelist.IsRefreshing = false;
            }
        }
    }
}
public class PinchToZoomContainer : ContentView
{
    double currentScale = 1;
    double startScale = 1;
    double xOffset = 0;
    double yOffset = 0;

    public PinchToZoomContainer()
    {
        PinchGestureRecognizer pinchGesture = new PinchGestureRecognizer();
        pinchGesture.PinchUpdated += OnPinchUpdated;
        GestureRecognizers.Add(pinchGesture);
    }

    void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        if (e.Status == GestureStatus.Started)
        {
            // Store the current scale factor applied to the wrapped user interface element,
            // and zero the components for the center point of the translate transform.
            startScale = Content.Scale;
            Content.AnchorX = 0;
            Content.AnchorY = 0;
        }
        if (e.Status == GestureStatus.Running)
        {
            // Calculate the scale factor to be applied.
            currentScale += (e.Scale - 1) * startScale;
            currentScale = Math.Max(1, currentScale);

            // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
            // so get the X pixel coordinate.
            double renderedX = Content.X + xOffset;
            double deltaX = renderedX / Width;
            double deltaWidth = Width / (Content.Width * startScale);
            double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

            // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
            // so get the Y pixel coordinate.
            double renderedY = Content.Y + yOffset;
            double deltaY = renderedY / Height;
            double deltaHeight = Height / (Content.Height * startScale);
            double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

            // Calculate the transformed element pixel coordinates.
            double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
            double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

            // Apply translation based on the change in origin.
            Content.TranslationX = Math.Clamp(targetX, -Content.Width * (currentScale - 1), 0);
            Content.TranslationY = Math.Clamp(targetY, -Content.Height * (currentScale - 1), 0);

            // Apply scale factor
            Content.Scale = currentScale;
        }
        if (e.Status == GestureStatus.Completed)
        {
            // Store the translation delta's of the wrapped user interface element.
            xOffset = Content.TranslationX;
            yOffset = Content.TranslationY;
        }
    }
}