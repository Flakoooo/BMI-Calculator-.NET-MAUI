using BodyMassIndexCalculator.src.ViewModels;

namespace BodyMassIndexCalculator.src.Views;

public partial class CalculatorPage : ContentPage
{
	public CalculatorPage(CalculatorViewModel calculatorViewModel)
	{
        BindingContext = calculatorViewModel;
		InitializeComponent();
	}
}