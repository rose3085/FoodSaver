using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class UploadPostSucessfull : ContentPage
{
	public UploadPostSucessfull(UploadPostSuccessfullViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}


	protected async override void OnAppearing()
	{
		base.OnAppearing();
        await Task.Delay(1000);
        await Shell.Current.GoToAsync("//HomePage");
    }
    }