using Controls.Net7.Api.Model.Dto;

namespace Controls.Net7.App.Views;

public partial class MessageBox : ContentPage
{

	public MessageBox(UserDto dto=null,string pages="µÇÂ¼")
	{
 
        Title = pages;
        InitializeComponent();
        if (Title == "µÇÂ¼")
        {
            UserDtos.IsVisible = false;
            UserDtosLogin.IsVisible = true;
            UserDtosLogin.Root.BindingContext = dto;
        }


        else {
            UserDtos.IsVisible = true;
            UserDtosLogin.IsVisible = false;
            UserDtos.Root.BindingContext = dto;
           

        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
              await Navigation.PopAsync();
        if (Title == "µÇÂ¼")
            MainPage.mainPage.gettoken(UserDtosLogin.Root.BindingContext as UserDto);
        else if(Title == "ÐÞ¸Ä") UserManger.userManger.update(UserDtos.Root.BindingContext as UserDto);

        else
            UserManger.userManger.AddUser(UserDtos.Root.BindingContext as UserDto);

    }
}