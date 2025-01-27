using Android.Accounts;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Helper;
using FoodSaverMaui.Helper.CacheHelper;
using FoodSaverMaui.KhaltiServices;
using FoodSaverMaui.Response;
using FoodSaverMaui.Response.FoodOrder;
using FoodSaverMaui.Response.PurchaseHistory;
using FoodSaverMaui.Services.Food;
using FoodSaverMaui.Services.PurchaseHistory;
using FoodSaverMaui.Services.SalesRecord;
using FoodSaverMaui.Services.User;
using FoodSaverMaui.SignalRServices;
using FoodSaverMaui.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Plugin.Maui.Biometric;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;

//using static Microsoft.Maui.Controls.Device;

namespace FoodSaverMaui.ViewModel
{
    
    public partial class UserProfileViewModel : BaseViewModel
    {
        [ObservableProperty]
        public string userName;

        [ObservableProperty]
        public double percentage;
        [ObservableProperty]
        public double totalsales;
        [ObservableProperty]
        public double remainingLimit;

        [ObservableProperty]
        public bool showSalesLimitReachedMessage;

       

        //private double _percentage;

        //public double Percentage
        //{
        //    get => _percentage;
        //    set => SetProperty(ref _percentage, value);
        //}
        public ObservableCollection<GetProductsResponse> Products { get; } = new();
        public ObservableCollection<GetPurchaseWrapper> PurchaseHistory { get; } = new();

        public GetOrderByProductIdResponse OrderDetail { get; } = new();

        private bool _isComponent1Visible ;

        public bool IsComponent1Visible
        {
            get => _isComponent1Visible;
            set => SetProperty(ref _isComponent1Visible, value);
        }

        private bool _isComponent2Visible;

        public bool IsComponent2Visible
        {
            get => _isComponent2Visible;
            set => SetProperty(ref _isComponent2Visible, value);
        }

        private bool _isComponent3Visible;

        public bool IsComponent3Visible
        {
            get => _isComponent3Visible;
            set => SetProperty(ref _isComponent3Visible, value);
        }
        private readonly FoodService _foodService;
        private readonly UserProfileService _userProfileService;
        private readonly PurchaseHistoryService _purchaseHistoryService;
        private readonly SalesRecordServices _salesRecordService;
        private readonly ICacheService _cacheService;
        private readonly IJwtHelper _jwtHelper;
        private readonly IKhaltiService _khaltiServices;

        public Command OnEnableFingerprintTapped { get; }
        public Command OnChangePasswordTapped { get; }
        public Command OnDeleteUserTapped { get; }
        public Command OnChangeEmailTapped { get; }
        public Command OnLogoutTapped { get; }
      
        public Command OnGetUserName { get; }
        public Command OnPostTapped { get; }
        public Command OnEditProfileTapped { get; }
        public Command OnDotsTapped { get; }
        public Command OnHistoryTapped { get; }
        public Command OnPageLoad { get; }
        public Command OnPurchaseHistoryTapped { get; }
        public Command OnDetailButtonTapped { get; }
        public Command OnConfirmPaymentTapped { get; }

        private readonly HubConnection _hubConnection;
        private readonly ISignalRService _signalRService;
        public UserProfileViewModel(FoodService foodService,IKhaltiService khaltiService,ISignalRService signalRService, ICacheService cacheService, IJwtHelper jwtHelper, SalesRecordServices salesRecordServices, UserProfileService userProfileService, PurchaseHistoryService purchaseHistoryService)
        {
            _foodService = foodService;
            _userProfileService = userProfileService;
            _purchaseHistoryService = purchaseHistoryService;
            _salesRecordService = salesRecordServices;
            _cacheService = cacheService;
            _jwtHelper = jwtHelper;
            _khaltiServices = khaltiService;
            _signalRService = signalRService;
            OnEnableFingerprintTapped = new Command(async () => await EnableFingerPrintTapped());
            OnPostTapped = new Command(async () => await PostTapped());
            OnHistoryTapped = new Command(async () => await HistoryTapped());
            OnChangePasswordTapped = new Command(async () => await ChangePasswordTapped());
            OnDeleteUserTapped = new Command(async () => await DeleteUserTapped());
            OnChangeEmailTapped = new Command(async () => await ChangeEmailTapped());
            OnLogoutTapped = new Command(async() => await LogoutTapped());
            OnDetailButtonTapped = new Command<GetProductsResponse>(async (selectedProduct) => await DetailButtonTapped(selectedProduct));
            OnGetUserName = new Command(async () => await GetUserName());
            OnEditProfileTapped = new Command(async() => await EditProfileTapped());
            OnDotsTapped = new Command<GetProductsResponse>(async (selectedProduct) => await DotsTapped(selectedProduct));
            IsComponent1Visible = true;
            IsComponent2Visible = false;
            IsComponent3Visible = false;
            ShowSalesLimitReachedMessage = false;
            OnPageLoad = new Command(async() => await GetUserRoles());
            OnPurchaseHistoryTapped = new Command(async() => await PurchaseHistoryTapped());
            OnConfirmPaymentTapped = new Command(async () => await ConfirmPayment());
        }

        public async Task ConfirmPayment()
        {

            var confirmPayment = await Shell.Current.DisplayAlert("Confirm Payment ?","Pay Rs.20 to continue selling foods.","Ok","Cancel");
            if (confirmPayment == true)
            {
                string pay = await _khaltiServices.KhaltiLaunch("20", "ProductId");
                if (pay != null)
                {
                    // await Shell.Current.GoToAsync($"{nameof(PaymentUrl)}?url={Uri.EscapeDataString(pay)}");
                    await Shell.Current.GoToAsync($"{nameof(ConfirmPaymentUrl)}?url={Uri.EscapeDataString(pay)}");
                }
            }
        
        }
        public async Task DetailButtonTapped(GetProductsResponse selectedProduct)
        {

            if (selectedProduct == null)
                return;
            if (selectedProduct.IsBooked == true)
            {

                var order = await _foodService.GetOrderByProductId(selectedProduct.Id);
                if (order != null)
                {

                    await Shell.Current.GoToAsync(nameof(OrderDetail), true, new Dictionary<string, object>
                    {

                     {"OrderDetail", order }
                    });
                }
                else 
                {
                    var message = "Couldn't display product details.";
                    var toast = Toast.Make($"{message}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await toast.Show();
                }

            }
            else
            {
                await Shell.Current.GoToAsync(nameof(FoodDetail), true, new Dictionary<string, object>
                    {

                     {"Product", selectedProduct }
                    });
            
            }
        }

        public async Task PurchaseHistoryTapped()
        {
            try {
                IsBusy = true;
                var result = await _purchaseHistoryService.GetUserPurchase();
                if (result == null)
                {
                    await Shell.Current.DisplayAlert("No Purchase History to display!!","Please try again later.","Ok!");
                }
                else 
                {
                    
                    PurchaseHistory.Clear();
                    await foreach (var product in result)
                    {
                        
                        PurchaseHistory.Add(product);
                    }
                    OnPropertyChanged(nameof(PurchaseHistory));
                    IsComponent1Visible = false;
                    IsComponent2Visible = false;
                    IsComponent3Visible = true;
                }

            }
            catch(Exception ex) {
                await Shell.Current.DisplayAlert("Something went wrong!", "Couldn't display Purchase History", "Ok");
            }
            finally { IsBusy = false; }
        }
        public async Task GetUserRoles()
        {
            try
            {
                IsSeller = false;
                IsBuyer = false;

                var roles = await SecureStorage.GetAsync("roles");
                if (roles != null)
                {
                    var rolesList = JsonSerializer.Deserialize<IList<string>>(roles);
                    if (rolesList != null)
                    {
                        if (rolesList.Count() > 0 && rolesList.Contains("Seller"))
                        {
                            IsSeller = true;
                            if (rolesList.Count() > 0 && rolesList.Contains("Buyer"))
                            {
                                IsBuyer = true;

                            }
                            await PostTapped();
                        }

                        else if (rolesList.Count() > 0 && rolesList.Contains("Buyer"))
                        {
                            IsBuyer = true;
                            await PurchaseHistoryTapped();
                            
                        }

                    }
                }
            }
            catch
            {

            }

        }

        public async Task DotsTapped(GetProductsResponse selectedProduct)
        {


           bool answer = await Shell.Current.DisplayAlert("Delete Product","Do you want to delete the product ?","Ok","Cancel");
            if (answer == true)
            {
             var id = selectedProduct.Id;
                if (id != null)
                { 
                    var result = await _foodService.DeleteFood(id);
                    if (result == true)
                    { await Shell.Current.DisplayAlert("Delete Product successful", "gffdhf", "Ok");
                        if (Products.Contains(selectedProduct))
                        {
                            Products.Remove(selectedProduct);
                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Delete Product unSuccessful", "gffdhf", "Ok");
                    }
                }
            }
        }
        public async Task GetUserName()
        {
            var token = await SecureStorage.GetAsync("token");
            UserName =  _jwtHelper.ExtractUserInfo(token);
        }
        public async Task HistoryTapped()
        {
            try
            {
                IsBusy = true;

                var request = await _salesRecordService.GetSalesRecord();
                if (request == null)
                {
                    await Shell.Current.DisplayAlert("You currently have no sales history to display!!","Please try again later.", "Ok!");
                }
                else
                {
                    string limitReached = request.dailyLimitReached.ToString();
                    await SecureStorage.SetAsync("dailyLimitReached", request.dailyLimitReached.ToString());
                    var newAmount = request.newAmount;
                    Totalsales = newAmount + request.totalPreviousAmount;
                    double maxLimit = 200;
                    RemainingLimit = maxLimit - newAmount;
                    if (RemainingLimit <= 0)
                    {
                        RemainingLimit = 0;
                        isSalesLimitReached = true;
                        ShowSalesLimitReachedMessage = true;

                    }
                    Percentage = newAmount / maxLimit;

                    if (Percentage > 1.0)
                    { Percentage = 1.0; }
                    IsComponent1Visible = false;
                    IsComponent2Visible = true;
                    IsComponent3Visible = false;
                }

            }
            catch
            {

                await Shell.Current.DisplayAlert("Network Error", "Couldn't display products!!", "Ok!");
            }
            finally
            {
                IsBusy = false;
            }

        }



        public async Task GetUserProduct()
        {
            var request = await _foodService.GetUserProducts();
            if (request.Count() > 0 )
            {

                Products.Clear();
                foreach (var product in request)
                {
                    Products.Add(product);
                }
                OnPropertyChanged(nameof(Products));

                IsComponent1Visible = true;
                IsComponent2Visible = false;
                IsComponent3Visible = false;
            }
            else
            {

                await Shell.Current.DisplayAlert( "You currently have no post to display.","Please try again later", "Ok!");
            }
            }


            public async Task PostTapped()
        {
            try
            {
                //var token = await SecureStorage.GetAsync("token");
                //UserName = _jwtHelper.ExtractUserInfo(token);
                IsBusy = true;
                var cacheResult = await _cacheService.GetFromCache<IEnumerable<GetProductsResponse>>("UserProduct");
                if (cacheResult != null)
                {
                    if (cacheResult.Count() == 0)
                    {
                        await GetUserProduct();
                    }
                    else
                    {
                        Products.Clear();
                        foreach (var product in cacheResult)
                        {
                            Products.Add(product);

                        }
                        OnPropertyChanged(nameof(Products));

                        IsComponent1Visible = true;
                        IsComponent2Visible = false;
                        IsComponent3Visible = false;
                    }
                }
                else
                {
                    await GetUserProduct();
                }
               
            }
            catch
            {

                await Shell.Current.DisplayAlert("Network Error", "Couldn't display products!!", "Ok!");
            }

            finally
            {
                IsBusy = false;
              
            }

        }

        public async Task EditProfileTapped()
        {
            await Shell.Current.GoToAsync(nameof(EditProfile));

        }
        public async Task ChangePasswordTapped()
        {
            await Shell.Current.GoToAsync(nameof(UpdatePassword));
        
        }

        public async Task DeleteUserTapped()
        {
            await Shell.Current.GoToAsync(nameof(DeleteUser));

        }
        public async Task ChangeEmailTapped()
        {
            await Shell.Current.GoToAsync(nameof(UpdateEmail));

        }
        public async Task LogoutTapped()
        {
            var checkFingerPrint =await SecureStorage.GetAsync("FingerPrint");

           var confirm = await Shell.Current.DisplayAlert("Logout","You're being logged out!","Ok","Cancel");
            if (confirm == true)
            {
                Products.Clear();
                await SecureStorage.SetAsync("isLoggedOut","yes");
               await _signalRService.Dispose();
                Application.Current.MainPage = new AppShell();
            }
        }


        public async Task EnableFingerPrintTapped()
        {

           bool confirm = await Shell.Current.DisplayAlert("Confirm","Do you want to enable biometric?","Yes","Cancel");
            if (confirm)
            {
                var result = await BiometricAuthenticationService.Default.AuthenticateAsync(new AuthenticationRequest()
                {
                    Title = "Please enter your fingerprint",
                    NegativeText = "Cancel",
                }, CancellationToken.None);
                //var status = BiometricHwStatus.LockedOut;
                if (result.Status == BiometricResponseStatus.Success)
                {
                   IsFingerPrintEnabled = true;
                    var msg = "enabled";
                    //bool msg = true;
                    await SecureStorage.SetAsync("FingerPrint", msg);
                    var message = "Finger print enabled successfully";
                    var toast = Toast.Make($"{message}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await toast.Show();
                }

                else

                {
                    IsFingerPrintEnabled = false;
                    var errorMsg = result.ErrorMsg;
                    var remove = "code:";
                    string pattern = $@"{remove}\s*\d{{1,2}}\s*(.*)";
                    Match match = Regex.Match(errorMsg, pattern);
                    string resultError = match.Groups[1].Value.Trim();
                    var toast = Toast.Make($"{resultError}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await toast.Show();
                }
            }
        }

        public async Task FingerPrintTapped()
        {
            var result = await BiometricAuthenticationService.Default.AuthenticateAsync(new AuthenticationRequest()
            {
                Title = "Please enter your fingerprint",
                NegativeText = "Cancel Authentication",
            }, CancellationToken.None);

            //var status = BiometricHwStatus.LockedOut;
            if (result.Status == BiometricResponseStatus.Success)
            {
                //await Shell.Current.DisplayAlert("Success", "Fingerprint authenticated successfully", "Ok!");
                await Shell.Current.GoToAsync("//HomePage");
                // await Navigation.PushAsync(new HomePage());
                //Microsoft.Maui.Controls.Application.Current.MainPage = new HomePage();
            }

            else

            {
                var errorMsg = result.ErrorMsg;
                var remove = "code:";
                string pattern = $@"{remove}\s*\d{{1,2}}\s*(.*)";
                Match match = Regex.Match(errorMsg, pattern);
                string resultError = match.Groups[1].Value.Trim();
                // string resultError = Regex.Replace(errorMsg, pattern, "").Trim();
                // await Shell.Current.DisplayAlert( $"{resultError}","", "Ok!"); }
                var toast = Toast.Make($"{resultError}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                await toast.Show();
            }

            //var errorMsg = result.ErrorMsg;
        }



    }
}
