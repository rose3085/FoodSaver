using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Response;
using FoodSaverMaui.Services.Food;
using FoodSaverMaui.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{
    public partial class FindFoodViewModel : BaseViewModel
    {
        private readonly FoodService _foodService;
        public Command OnClickTapped { get; }
        public Command OnAddButtonClick { get; }
        public ObservableCollection<GetProductsResponse> Products { get; } = new();

        //private IEnumerable<GetProductsResponse> _products;
        //public IEnumerable<GetProductsResponse> Products
        //{
        //    get => _products;
        //    set
        //    {
        //        _products = value;
        //        OnPropertyChanged(nameof(Products));  // Notify UI about the change
        //    }
        //}
        public FindFoodViewModel(FoodService foodService)
        {
            _foodService = foodService;
            OnClickTapped = new Command(async() => await ClickTapped());
            OnAddButtonClick = new Command(async() => await AddButtonClick());
            //Products = new ObservableCollection<GetProductsResponse>();
        }


        public async Task AddButtonClick()
        {
            await Shell.Current.GoToAsync(nameof(UploadFood));
        
        }
      
        

        //// Command that fetches the data
        //[RelayCommand]
        public async Task ClickTapped()
        { 

            var request =await _foodService.GetAllProducts();
            if (request != null)
            {
                Products.Clear();
                foreach (var product in request)
                {
                    Products.Add(product);
                }
                OnPropertyChanged(nameof(Products));

            }
            else {

                await Shell.Current.DisplayAlert("Success", "", "Ok!");
            }

        }
    }
}
