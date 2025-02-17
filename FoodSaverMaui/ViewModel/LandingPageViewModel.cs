using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Helper.CacheHelper;
using FoodSaverMaui.Response;
using FoodSaverMaui.Services.Food;
using FoodSaverMaui.Views;
using System;
using System.Timers;
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

        [ObservableProperty]
        private ObservableCollection<string> imageList;

        [ObservableProperty]
        private int currentIndex;

        private System.Timers.Timer _timer;
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

        public async Task ImageCarousel()
        {
            // Load images from the Resources/Images folder
            ImageList = new ObservableCollection<string>
        {
            "welcomeimage.png",
            "notificationimage.png",
            "buyimage.png",
            "khaltiimage.png",
           
            "mapimage.png"
        };

            StartAutoSwipe();
        }
        private void StartAutoSwipe()
        {
            _timer = new System.Timers.Timer(4000);
            _timer.Elapsed += (s, e) =>
            {
                if (ImageList.Count == 0) return;
                CurrentIndex = (CurrentIndex + 1) % ImageList.Count;
            };
            _timer.AutoReset = true;
            _timer.Start();
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

        public async Task<Location> GetLocation()
        {
            try { 

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            Location location = await Geolocation.Default.GetLocationAsync(request);


            return location;
                 }
            catch { return null; }
        }

        public async Task GetProducts()
        {
            try
            {
                IsBusy = true;
                var location = await GetLocation();
                if (location != null)
                {
                    double currentLat = location.Latitude;
                    double currentLong = location.Longitude;
                    var request = await _foodService.GetAllProducts(currentLat, currentLong);
                   // await _cacheService.AddOrUpdateCache("ProductHomePage", request);
                    if (request != null)
                    {
                        if (request.Count() > 0)
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
                            Products.Clear();
                            await Shell.Current.DisplayAlert("No products available to display!!", "Please try again later.", "Ok!"); }
                    }
                    else
                    {
                        Products.Clear();
                        await Shell.Current.DisplayAlert("Network Error", "Couldn't display products!!", "Ok!");

                    }
                }
            }
            catch
            { Products.Clear(); }
        }

        public async Task PageMounted()
        {
            try
            {
                //var cachedResult = await _cacheService.GetFromCache<IEnumerable<GetProductsResponse>>("ProductHomePage");
                //if (cachedResult != null)
                //{
                //    if (cachedResult.Count() == 0)
                //    {
                //        await GetProducts();
                //    }
                //    else
                //    {
                //        Products.Clear();
                //        foreach (var product in cachedResult)
                //        {
                //            Products.Add(product);
                //        }
                //        OnPropertyChanged(nameof(Products));
                //    }
                //}
                //else
                //{
                await ImageCarousel();

             
                await GetProducts();

                
            }
            catch { }
            finally { IsBusy = false; }

        }
    }
}