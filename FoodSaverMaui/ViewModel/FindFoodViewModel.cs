using CommunityToolkit.Maui.Alerts;
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
        public Command OnSearchButtonPressed { get; }
        [ObservableProperty]
        string searchQuery;

        [ObservableProperty]
        bool isRefreshing;
        public FindFoodViewModel(FoodService foodService)
        {
            _foodService = foodService;
            OnClickTapped = new Command(async() => await ClickTapped());
            OnAddButtonClick = new Command(async() => await AddButtonClick());
            OnSearchButtonPressed = new Command(async () => await SearchButtonPressed(SearchQuery));
            //Products = new ObservableCollection<GetProductsResponse>();
        }


        public async Task AddButtonClick()
        {
            await Shell.Current.GoToAsync(nameof(UploadFood));
        
        }

        public async Task SearchButtonPressed(string query)
        {

           
                if (string.IsNullOrWhiteSpace(query))
                {
                    await ClickTapped();
                }
                else
                {
                    
                    var filteredList = Products
                        .Where(p => p.ProductName.Contains(query, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                if (filteredList.Count == 0)
                {
                    Products.Clear();
                    OnPropertyChanged(nameof(Products));
                    var message = $"No product of name {query} found!! ";
                    var toast = Toast.Make($"{message}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await toast.Show();

                }
                else
                {
                    Products.Clear();
                    foreach (var product in filteredList)
                    {
                        Products.Add(product);
                    }


                    OnPropertyChanged(nameof(Products));
                }
                }
            

        }


        //// Command that fetches the data
        //[RelayCommand]
        public async Task ClickTapped()
        {
            try
            {
                
                IsBusy = true;
                var request = await _foodService.GetAllProducts();
                if (request != null)
                {
                    Products.Clear();
                    foreach (var product in request)
                    {
                        Products.Add(product);
                    }
                    OnPropertyChanged(nameof(Products));

                }
                else
                {

                    await Shell.Current.DisplayAlert("Success", "", "Ok!");
                }
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }

        }
    }
}
