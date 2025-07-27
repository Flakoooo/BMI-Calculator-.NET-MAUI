using BodyMassIndexCalculator.src.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BodyMassIndexCalculator.src.ViewModels
{
    public partial class ProfileModel : ObservableObject
    {
        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private string? _email;

        [ObservableProperty]
        private ObservableCollection<BodyMassIndexCalculation>? _bodyMassIndexCalculations;
    }

    public partial class ProfileViewModel : ObservableObject
    {
        private DateTime _lastRefreshTime = DateTime.MinValue;

        [ObservableProperty]
        private ProfileModel _profileModel;

        public ProfileViewModel()
        {
            ProfileModel = new ProfileModel
            {
                Name = string.Empty,
                Email = string.Empty,
                BodyMassIndexCalculations = []
            };
            SetUserData("Имя Фамилия", "Почта");
            _ = LoadCalculationsDataAsync();
        }

        [RelayCommand]
        public async Task RefreshData()
        {
            if ((DateTime.Now - _lastRefreshTime).TotalSeconds < 5)
                return;

            await LoadCalculationsDataAsync();
            _lastRefreshTime = DateTime.Now;
        }

        private async Task LoadCalculationsDataAsync()
        {
            IEnumerable<BodyMassIndexCalculation> calculations = 
                [  
                new BodyMassIndexCalculation 
                {
                    CreatedAt = DateTime.Now,
                    Height = 185,
                    Weight = 77,
                    BodyMassIndex = 22.5,
                    Recommendation = "рекомендация 1"
                },
                new BodyMassIndexCalculation
                {
                    CreatedAt = DateTime.Now.AddDays(-1),
                    Height = 180,
                    Weight = 82,
                    BodyMassIndex = 24.3,
                    Recommendation = "рекомендация 2"
                },
                new BodyMassIndexCalculation
                {
                    CreatedAt = DateTime.Now.AddDays(-2),
                    Height = 190,
                    Weight = 56,
                    BodyMassIndex = 19.2,
                    Recommendation = "рекомендация 3"
                }
                ];
            ProfileModel.BodyMassIndexCalculations = new ObservableCollection<BodyMassIndexCalculation>(
                calculations.OrderByDescending(bmic => bmic.CreatedAt)
            );
        }


        private void SetUserData(string fullName = "null null", string email = "null")
            => (ProfileModel.Name, ProfileModel.Email) = (fullName, email);
    }
}
