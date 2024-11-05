using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{

    [QueryProperty(nameof(Product), "Product")]
    public partial class FoodDetailViewModel : BaseViewModel
    {
        
        private GetProductsResponse product;

        public GetProductsResponse Product // Define the property itself
        {
            get => product;
            set => SetProperty(ref product, value); // Use SetProperty to notify change
        }

        public Command OnRemoveButtonPressed { get; }

        public FoodDetailViewModel()
        {
            OnRemoveButtonPressed = new Command(async() => await RemoveButtonPressed());
        }

        public async Task RemoveButtonPressed()
        {
            await Shell.Current.GoToAsync("..");
        }

    }
}
