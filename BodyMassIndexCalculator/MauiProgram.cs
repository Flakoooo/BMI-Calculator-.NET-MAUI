using BodyMassIndexCalculator.src.ViewModels;
using Microsoft.Extensions.Logging;

namespace BodyMassIndexCalculator
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>();

            builder.Services
                .AddTransient<CalculatorViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
