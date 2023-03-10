using Controls.Net7.Api.Model.Dto;

namespace Controls.Net7.App.Views;

public partial class MessageBox : ContentPage
{

	public MessageBox(UserDto dto=null,string pages="��¼")
	{
 
        Title = pages;
        InitializeComponent();
        if (Title == "��¼")
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
        if (Title == "��¼")
            MainPage.mainPage.gettoken(UserDtosLogin.Root.BindingContext as UserDto);
        else if(Title == "�޸�") UserManger.userManger.update(UserDtos.Root.BindingContext as UserDto);

        else
            UserManger.userManger.AddUser(UserDtos.Root.BindingContext as UserDto);

    }
}