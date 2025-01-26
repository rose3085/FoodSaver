using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class OrderDetail : ContentPage
{
	public OrderDetail(OrderDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
       

        if (BindingContext is OrderDetailViewModel viewModel)
        {
            // Check and execute OnPostTapped command
            if (viewModel.OnCheckDeliveryStatus.CanExecute(null))
            {
                viewModel.OnCheckDeliveryStatus.Execute(null);
            }
        }
    }
        }