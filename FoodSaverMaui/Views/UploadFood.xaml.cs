using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class UploadFood : ContentPage
{
    private UploadFoodViewModel _viewModel;
    public UploadFood(UploadFoodViewModel vm)
	{
		InitializeComponent();
        _viewModel = vm;
        BindingContext = vm;
	}
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
      //  _viewModel.PickedImage = null;
        
       // _viewModel.DisposeImage(); // Call Dispose method in ViewModel
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        
        if (BindingContext is UploadFoodViewModel viewModel)
        {
            viewModel.Food = string.Empty;
            viewModel.Description = string.Empty;
            viewModel.Price = 0; 
            viewModel.WardNumber = string.Empty;
            viewModel.ToleName = string.Empty;
            viewModel.CityName = string.Empty;
            viewModel.UserName = string.Empty;

            viewModel.StepperValue = 0;

          
            viewModel.PickedImage = null;
        

            if (viewModel.OnGetUserName.CanExecute(null))
            {
                viewModel.OnGetUserName.Execute(null);
            }

            //// Check and execute OnGetUserName command
            //if (viewModel.OnGetUserName.CanExecute(null))
            //{
            //    viewModel.OnGetUserName.Execute(null);
            //}
        }
    }
    //protected override async void OnAppearing()
    //{
    //    base.OnAppearing();

    //    // Safely reload the image if needed
    //    _viewModel.Reset(); // This will clear the image and reset any other state
    //    _viewModel.Initialize();
    //}
}