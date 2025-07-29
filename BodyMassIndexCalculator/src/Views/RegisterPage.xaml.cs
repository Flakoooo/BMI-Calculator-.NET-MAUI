using BodyMassIndexCalculator.src.ViewModels;

namespace BodyMassIndexCalculator.src.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel registerViewModel)
	{
        BindingContext = registerViewModel;
		InitializeComponent();
	}
}