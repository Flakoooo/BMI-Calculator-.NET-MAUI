using BodyMassIndexCalculator.src.ViewModels;

namespace BodyMassIndexCalculator.src.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileViewModel _viewModel;
    public ProfilePage(ProfileViewModel profileViewModel)
    {
        BindingContext = _viewModel = profileViewModel;
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.RefreshDataCommand.Execute(null);
    }
}