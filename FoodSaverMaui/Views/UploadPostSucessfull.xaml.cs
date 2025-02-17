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
        // Application.Current.MainPage = new AppShell();
       // await Shell.Current.GoToAsync("//TempPage");
        //await Task.Delay(1000);
       // await Shell.Current.GoToAsync("//HomePage");
        //Shell.Current.CurrentItem = Shell.Current.Items.First(item => item.Route == "UploadFood");

        //await Shell.Current.GoToAsync("..");
    }
}