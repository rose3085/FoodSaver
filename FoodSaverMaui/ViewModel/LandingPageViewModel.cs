using FoodSaverMaui.Helper.CacheHelper;
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
    public partial class LandingPageViewModel : BaseViewModel
    {
        private readonly FoodService _foodService;
        private readonly ICacheService _cacheService;

        public ObservableCollection<GetProductsResponse> Products { get; } = new();
        public Command OnClickTapped { get; }
        public Command OnDetailButtonClicked { get; }
        public Command OnMoreTapped { get; }
        public LandingPageViewModel(FoodService foodService,ICacheService cacheService)
        {
            _foodService = foodService;
            _cacheService = cacheService;
            OnClickTapped = new Command(async () => await PageMounted());
            OnDetailButtonClicked = new Command<GetProductsResponse>(async (selectedProduct) => await DetailButtonClicked(selectedProduct));
            OnMoreTapped = new Command(async() => await MoreTapped());
        }


        public async Task MoreTapped()
        {
            //await Shell.Current.GoToAsync(nameof(FindFood));

           
            // Shell.Current.CurrentItem = Shell.Current.Items.FirstOrDefault(item => item.Route == nameof(FindFood));

        }
        public async Task DetailButtonClicked(GetProductsResponse selectedProduct)
        {
            if (selectedProduct == null)
                return;

            await Shell.Current.GoToAsync(nameof(FoodDetail), true, new Dictionary<string, object>
           {

            {"Product", selectedProduct }
           });
        }



        public async Task GetProducts()
        {
            var request = await _foodService.GetAllProducts();
            await _cacheService.AddOrUpdateCache("ProductHomePage",request);
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

                await Shell.Current.DisplayAlert("Network Error", "Couldn't display products!!", "Ok!");

            }
        }

        public async Task PageMounted()
        {

            var cachedResult = await _cacheService.GetFromCache<IEnumerable<GetProductsResponse>>("ProductHomePage");
            if (cachedResult != null)
            {
                if (cachedResult.Count() == 0)
                {
                    await GetProducts();
                }
                else 
                {
                    Products.Clear();
                    foreach (var product in cachedResult)
                    {
                        Products.Add(product);
                    }
                    OnPropertyChanged(nameof(Products));
                }
            }
            else 
            {
                await GetProducts();
            }


        }
    }
}