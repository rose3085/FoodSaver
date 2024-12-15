using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Helper;
using FoodSaverMaui.Response;
using FoodSaverMaui.Services.Food;
using FoodSaverMaui.Services.SalesRecord;
using FoodSaverMaui.Views;
using Plugin.Maui.Biometric;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        private readonly FoodService _foodService;
        private readonly SalesRecordServices _salesRecordService;
        private readonly IJwtHelper _jwtHelper;

        public Command OnEnableFingerprintTapped { get; }
        public Command OnChangePasswordTapped { get; }
        public Command OnDeleteUserTapped { get; }
        public Command OnChangeEmailTapped { get; }
        public Command OnLogoutTapped { get; }
        public Command ToggleCommand { get; }
        public Command OnGetUserName { get; }
        public Command OnPostTapped { get; }
        public Command OnHistoryTapped { get; }
        public UserProfileViewModel(FoodService foodService,IJwtHelper jwtHelper,SalesRecordServices salesRecordServices)
        {
            _foodService = foodService;
            _salesRecordService = salesRecordServices;
            _jwtHelper = jwtHelper;
            OnEnableFingerprintTapped = new Command(async () => await EnableFingerPrintTapped());
            OnPostTapped = new Command(async () => await PostTapped());
            OnHistoryTapped = new Command(async () => await HistoryTapped());
            OnChangePasswordTapped = new Command(async () => await ChangePasswordTapped());
            OnDeleteUserTapped = new Command(async () => await DeleteUserTapped());
            OnChangeEmailTapped = new Command(async () => await ChangeEmailTapped());
            OnLogoutTapped = new Command(async() => await LogoutTapped());
            ToggleCommand = new Command(async () => await OnToggleCommand());
            OnGetUserName = new Command(async () => await GetUserName());
            IsComponent1Visible = true;
            IsComponent2Visible = false;
            ShowSalesLimitReachedMessage = false;
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
                var request = await _salesRecordService.GetSalesRecord();
            
                    string limitReached = request.dailyLimitReached.ToString();
                    await SecureStorage.SetAsync("dailyLimitReached",request.dailyLimitReached.ToString());
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
             
            }
            catch
            {

                await Shell.Current.DisplayAlert("Network Error", "Couldn't display products!!", "Ok!");
            }

        }


        public async Task PostTapped()
        {
            try
            {
                //var token = await SecureStorage.GetAsync("token");
                //UserName = _jwtHelper.ExtractUserInfo(token);
                IsBusy = true;
                var request = await _foodService.GetUserProducts();
                if (request != null)
                {
                    Products.Clear();
                    foreach (var product in request)
                    {
                        Products.Add(product);
                    }
                    OnPropertyChanged(nameof(Products));

                    IsComponent1Visible = true;
                    IsComponent2Visible = false;
                }
                else
                {

                    await Shell.Current.DisplayAlert("Network Error", "Couldn't display products!!", "Ok!");
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


        public async Task OnToggleCommand()
        {
            IsComponent1Visible = !IsComponent1Visible;
            IsComponent2Visible = !IsComponent2Visible;







            // Optional: Simulate some asynchronous behavior
            await Task.Delay(100);
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
            await Shell.Current.GoToAsync(nameof(Login));

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
