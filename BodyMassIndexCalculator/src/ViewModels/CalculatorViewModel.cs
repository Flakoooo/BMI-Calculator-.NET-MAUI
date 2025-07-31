using BodyMassIndexCalculator.src.Models;
using BodyMassIndexCalculator.src.Services;
using BodyMassIndexCalculator.src.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyMassIndexCalculator.src.ViewModels
{
    public partial class CalculatorModel : ObservableObject
    {
        [ObservableProperty]
        private string? _errorText;

        [ObservableProperty]
        private bool _isResultVisible;

        [ObservableProperty]
        private string? _result;

        [ObservableProperty]
        private string? _recommendation;

        [ObservableProperty]
        private string? _height;

        [ObservableProperty]
        private string? _weight;
    }

    public partial class CalculatorViewModel : ObservableObject
    {
        private readonly IAPI _api;

        [ObservableProperty]
        private CalculatorModel _calculatorModel;

        public CalculatorViewModel(IAPI api)
        {
            _api = api;

            CalculatorModel = new CalculatorModel
            {
                ErrorText = string.Empty,
                IsResultVisible = false,
                Result = string.Empty,
                Recommendation = string.Empty,
                Height = string.Empty,
                Weight = string.Empty
            };
        }

        [RelayCommand]
        private async Task Calculate()
        {
            CalculatorModel.ErrorText = string.Empty;

            bool heightEmpty = string.IsNullOrWhiteSpace(CalculatorModel.Height);
            bool weightEmpty = string.IsNullOrWhiteSpace(CalculatorModel.Weight);

            if (heightEmpty || weightEmpty)
            {
                CalculatorModel.ErrorText = (heightEmpty, weightEmpty) switch
                {
                    (true, true) => "Заполните все поля!",
                    (true, false) => "Поле Рост не заполнено!",
                    (false, true) => "Поле Вес не заполнено!",
                    _ => string.Empty
                };
                CalculatorModel.IsResultVisible = false;
                return;
            }

            if (int.TryParse(CalculatorModel.Height, out int height) &&
                int.TryParse(CalculatorModel.Weight, out int weight))
            {
                bool heighLittle = height < 50;
                bool heighBig = height > 272;
                bool weightLittle = weight < 50;
                bool weightBig = weight > 500;
                if (heighLittle || weightLittle || heighBig || weightBig)
                {
                    CalculatorModel.ErrorText = (heighLittle, weightLittle, heighBig, weightBig) switch
                    {
                        (true, _, _, _) => "Рост не может быть меньше 50!",
                        (_, true, _, _) => "Вес не может быть меньше 2!",
                        (_, _, true, _) => "Рост не может быть больше 272!",
                        (_, _, _, true) => "Вес не может быть больше 500!",
                        _ => string.Empty
                    };
                    CalculatorModel.IsResultVisible = false;
                    return;
                }

                double index = Math.Round(weight / Math.Pow((double)height / 100, 2), 2);
                string recommendation = GetRecommendation(index);

                var id = SupabaseService.Client.Auth.CurrentUser?.Id;
                if (id != null)
                {
                    var result = await _api.CreateCalculation(Guid.Parse(id), height, weight, index, recommendation);
                    if (result.Error == null && result.Result != null)
                    {
                        CalculatorModel.Result = index.ToString();
                        CalculatorModel.Recommendation = recommendation;
                        CalculatorModel.IsResultVisible = true;
                    }
                    else CalculatorModel.ErrorText = result.Error ?? "Ошибка при сохранении данных";
                }
                else CalculatorModel.ErrorText = "Ошибка авторизации";
            }
            return;
        }

        private static string GetRecommendation(double index) => index switch
        {
            <= 16.0 => "Выраженный дефицит массы тела. Советуем набрать вес для здоровья.",
            <= 18.5 => "Недостаточная масса тела. Рекомендуется увеличить массу тела.",
            <= 24.99 => "Норма. Ваш вес в здоровом диапазоне — поддерживайте его!",
            <= 30.0 => "Избыточная масса тела или предожирение. Желательно снизить вес для улучшения самочувствия.",
            <= 35.0 => "Ожирение. Рекомендуется уменьшить вес под контролем специалиста.",
            <= 40.0 => "Ожирение резкое. Необходимо снижение веса с медицинской поддержкой.",
            _ => "Очень резкое ожирение. Требуется срочная коррекция веса под наблюдением врача."
        };
    }
}
