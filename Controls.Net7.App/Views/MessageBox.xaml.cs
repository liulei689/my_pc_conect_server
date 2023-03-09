namespace Controls.Net7.App.Views;

public partial class MessageBox : ContentPage
{
	public MessageBox()
	{
		InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        MainPage.username = username.Text;
        MainPage.passwword = password.Text;

        MainPage.role = role.Text;

        await Navigation.PopAsync();
        MainPage.mainPage.gettoken();

    }
}