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
using System.Windows.Input;

namespace FoodSaverMaui.ViewModel
{
    public partial class FindFoodViewModel : BaseViewModel
    {
        private readonly FoodService _foodService;
        public Command OnClickTapped { get; }
        public Command OnAddButtonClick { get; }
        public ObservableCollection<GetProductsResponse> Products { get; } = new();

        [ObservableProperty]
        private bool isPopupVisible;
        public Command OnSearchButtonPressed { get; }
        [ObservableProperty]
        string searchQuery;

        [ObservableProperty]
        bool isRefreshing;

        public Command OnDetailButtonClicked { get; }
        public Command OnRemoveButtonPressed { get; }

        public ICommand ShowPopupCommand => new Command(ShowPopup);
        public FindFoodViewModel(FoodService foodService)
        {
            _foodService = foodService;
            OnClickTapped = new Command(async() => await ClickTapped());
            OnAddButtonClick = new Command(async() => await AddButtonClick());
            OnSearchButtonPressed = new Command(async () => await SearchButtonPressed(SearchQuery));
            OnDetailButtonClicked = new Command<GetProductsResponse>(async (selectedProduct) => await DetailButtonClicked(selectedProduct));
            OnRemoveButtonPressed = new Command<GetProductsResponse>(async (product) => await RemoveButtonPressed(product));
            //Products = new ObservableCollection<GetProductsResponse>();
        }

        public async Task RemoveButtonPressed(GetProductsResponse product)
        {
            if (product != null && Products.Contains(product))
            {
                Products.Remove(product);
            }
        }

        public async Task DetailButtonClicked(GetProductsResponse selectedProduct)
        {
            if(selectedProduct == null)
                return;

           await Shell.Current.GoToAsync(nameof(FoodDetail), true, new Dictionary<string, object>
           {

            {"Product", selectedProduct }
           });
        }
        public async void ShowPopup()
        {
            string action = await Shell.Current.DisplayActionSheet("Arrange Price?", "Cancel", null, "High to Low", "Low to High");

            if (action == "High to Low")
            {
               
                var result = await SortProductsDescending();
            }
            if (action == "Low to High")
            {
                var result = await SortProductsAscending();
            }

           
        }

        public async Task AddButtonClick()
        {
            var limitReached = await SecureStorage.GetAsync("dailyLimitReached");
            if (limitReached == "True")
            {
                await Shell.Current.DisplayAlert("Sales limit reached!!", "Confirm Payment to upgrade your sales limit.", "Ok!");
                //var action = await Shell.Current.DisplayActionSheet("Sales limit reached!!", "Cancel", null, "Confirm Payment to upgrade your sales limit.");
                //if (action == "Confirm Payment to upgrade your sales limit.")
                //{

                //    await Shell.Current.GoToAsync(nameof(UserProfile));
                //}
            }
            else
            {
                await Shell.Current.GoToAsync(nameof(UploadFood));
            }
        
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
                    
                    if (request.Count() == 0)
                    {
                        await Shell.Current.DisplayAlert("No product to display", "Please try again later", "Ok!");
                    }
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
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }

        }


        public async Task<IEnumerable<GetProductsResponse>> SortProductsAscending()
        {
            var sortedProducts = MergeSort(Products, true);
            Products.Clear();
            foreach (var product in sortedProducts)
            {
                Products.Add(product);
            }
            OnPropertyChanged(nameof(Products));
            return sortedProducts;
        }

   
        public async Task<IEnumerable<GetProductsResponse>> SortProductsDescending()
        {

            var sortedProducts = MergeSort(Products, false);
            Products.Clear();
            foreach (var product in sortedProducts)
            {
                Products.Add(product);
            }
            OnPropertyChanged(nameof(Products));
            return sortedProducts;
        }



        public IEnumerable<GetProductsResponse> MergeSort(IEnumerable<GetProductsResponse> products, bool ascending = true)
        {
            List<GetProductsResponse> productList = products.ToList();

            if (productList.Count <= 1)
            {
                return productList; 
            }

           
            int mid = productList.Count / 2;
            var left = productList.Take(mid);
            var right = productList.Skip(mid);

           
            return Merge(MergeSort(left, ascending), MergeSort(right, ascending), ascending);
        }

      
        private IEnumerable<GetProductsResponse> Merge(IEnumerable<GetProductsResponse> left, IEnumerable<GetProductsResponse> right, bool ascending)
        {
            List<GetProductsResponse> merged = new List<GetProductsResponse>();
            using (var leftEnumerator = left.GetEnumerator())
            using (var rightEnumerator = right.GetEnumerator())
            {
                bool hasLeft = leftEnumerator.MoveNext();
                bool hasRight = rightEnumerator.MoveNext();

                while (hasLeft && hasRight)
                {
                    if ((ascending && leftEnumerator.Current.PricePerKg < rightEnumerator.Current.PricePerKg) ||
                        (!ascending && leftEnumerator.Current.PricePerKg > rightEnumerator.Current.PricePerKg))
                    {
                        merged.Add(leftEnumerator.Current);
                        hasLeft = leftEnumerator.MoveNext();
                    }
                    else
                    {
                        merged.Add(rightEnumerator.Current);
                        hasRight = rightEnumerator.MoveNext();
                    }
                }

                
                while (hasLeft)
                {
                    merged.Add(leftEnumerator.Current);
                    hasLeft = leftEnumerator.MoveNext();
                }
                while (hasRight)
                {
                    merged.Add(rightEnumerator.Current);
                    hasRight = rightEnumerator.MoveNext();
                }
            }

            return merged;
        }
    }
}

