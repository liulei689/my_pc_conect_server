using Controls.Models;

namespace Controls.Net7.App.Views;

public partial class CodePage : ContentPage
{
	public CodePage(Codess codess)
	{
		InitializeComponent();
		detail.Text = codess.Code;
	}
}