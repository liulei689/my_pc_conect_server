namespace Controls.Net7.App.Views;

public partial class ImageZoom : ContentPage
{
	public ImageZoom(ImageSource imageSource)
	{
		InitializeComponent();
		image.Source = imageSource;
	}
}