using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core.Platform;
using FoodSaverMaui.Helper;
using FoodSaverMaui.Model;
using FoodSaverMaui.Services.User;
using FoodSaverMaui.Views;
using Microsoft.Maui;
using Newtonsoft.Json.Linq;
using Plugin.Maui.Biometric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{
    public partial class LoginViewModel : BaseViewModel
    {
        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));

            }


        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            { 
                _email = value;
                OnPropertyChanged(nameof(Email));
            
            }
        
        
        }

        private string _password;
        public string Password
        {
            get => _password;
            set 
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            
            }
        
        }

        private readonly IJwtHelper _jwtHelper;
        private readonly UserServices _userServices;

        public Command OnSignInTapped { get; }
        public Command OnRegisterTapped { get; }
        public Command OnFingerPrintTapped { get; }
        public LoginViewModel(UserServices userServices, IJwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
            _userServices = userServices;
            OnSignInTapped = new Command(async() => await SignInTapped());
            OnRegisterTapped = new Command(async() => await RegisterTapped());
            OnFingerPrintTapped = new Command(async() => await FingerPrintTapped());
        }

        public async Task FingerPrintTapped()
        {
            var jwtToken = await SecureStorage.GetAsync("token");
            var fingerPrint = await SecureStorage.GetAsync("FingerPrint");
            if (jwtToken != null && fingerPrint != null)
            {

                var result = await BiometricAuthenticationService.Default.AuthenticateAsync(new AuthenticationRequest()
                {
                    Title = "Please enter your fingerprint",
                    NegativeText = "Cancel Authentication",
                }, CancellationToken.None);

               
                if (result.Status == BiometricResponseStatus.Success)
                {
                    //await Shell.Current.DisplayAlert("Success", "Fingerprint authenticated successfully", "Ok!");

                    var username = _jwtHelper.ExtractUserInfo(jwtToken);
                    await Shell.Current.GoToAsync("//HomePage");

                }
                else
                {
                    var errorMsg = result.ErrorMsg;
                    var remove = "code:";
                    string pattern = $@"{remove}\s*\d{{1,2}}\s*(.*)";
                    Match match = Regex.Match(errorMsg, pattern);
                    string resultError = match.Groups[1].Value.Trim();
                    var toast = Toast.Make($"{resultError}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await toast.Show();
                }
              
            }  else
                {
                await Shell.Current.DisplayAlert("Error", "Fingerprint not enabled", "Ok!");

            }
        
        }
        public async Task RegisterTapped()

        {
            //await Shell.Current.GoToAsync(nameof(CreateAccount));
            //CreateAccount
            await Shell.Current.GoToAsync("//CreateAccount");
        }


        public async Task SignInTapped()
        {
            
            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password))
            {
                try
                {

                    IsBusy = true;
                    var requestModel = new UserLoginRequest
                    {
                        UserName = UserName,
                        Email = Email,
                        Password = Password,
                    };

                    var request = await _userServices.LoginUser(requestModel);
                    if (request == true)
                    {
                        //await Shell.Current.DisplayAlert("Success", "User login successful", "Ok!");
                        await Shell.Current.GoToAsync("//HomePage", true);

                    }
                    else
                    {
                        string resultError = "Enter Valid Credentials!!";
                        var toast = Toast.Make($"{resultError}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                        await toast.Show();


                    }
                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
            {
                string resultError = "Enter Valid Credentials!!";
                var toast = Toast.Make($"{resultError}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                await toast.Show();

            }

        }

    }
}
