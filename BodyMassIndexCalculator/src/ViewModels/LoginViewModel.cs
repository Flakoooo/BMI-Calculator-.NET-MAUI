using BodyMassIndexCalculator.src.Services;
using BodyMassIndexCalculator.src.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BodyMassIndexCalculator.src.ViewModels
{
    public partial class LoginModel : ObservableObject
    {
        [ObservableProperty]
        private string? _errorText;

        [ObservableProperty]
        private string? _email;

        [ObservableProperty]
        private string? _password;
    }

    public partial class LoginViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private LoginModel _loginModel;

        public LoginViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            LoginModel = new LoginModel
            {
                ErrorText = string.Empty,
                Email = string.Empty,
                Password = string.Empty
            };
        }

        [RelayCommand]
        private async Task Login()
        {
            LoginModel.ErrorText = string.Empty;

            bool emailEmpty = string.IsNullOrWhiteSpace(LoginModel.Email);
            bool passwordEmpty = string.IsNullOrWhiteSpace(LoginModel.Password);

            if (emailEmpty || passwordEmpty)
            {
                LoginModel.ErrorText = (emailEmpty, passwordEmpty) switch
                {
                    (true, true) => "Заполните все поля!",
                    (true, false) => "Заполните почту!",
                    (false, true) => "Заполните пароль!",
                    _ => string.Empty
                };
                return;
            }

            var result = await AuthService.SignIn(LoginModel.Email ?? "", LoginModel.Password ?? "");
            if (result.Result == null) LoginModel.ErrorText = result.Error;
            else
            {
                await _navigationService.GoToMainTabsAsync();
            }
        }

        [RelayCommand]
        private async Task GoToRegister() => await _navigationService.GoToRegisterAsync();
    }
}
