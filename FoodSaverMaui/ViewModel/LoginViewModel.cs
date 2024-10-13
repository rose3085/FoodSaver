using CommunityToolkit.Maui.Alerts;
using FoodSaverMaui.Model;
using FoodSaverMaui.Services.User;
using FoodSaverMaui.Views;
using Microsoft.Maui;
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

        private readonly UserServices _userServices;

        public Command OnSignInTapped { get; }
        public Command OnRegisterTapped { get; }
        public Command OnFingerPrintTapped { get; }
        public LoginViewModel(UserServices userServices)
        {
            _userServices = userServices;
            OnSignInTapped = new Command(async() => await SignInTapped());
            OnRegisterTapped = new Command(async() => await RegisterTapped());
            OnFingerPrintTapped = new Command(async() => await FingerPrintTapped());
        }

        public async Task FingerPrintTapped()
        {  
            var result = await BiometricAuthenticationService.Default.AuthenticateAsync(new AuthenticationRequest()
                {
                Title="Please enter your fingerprint",
                NegativeText="Cancel Authentication",
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
                var toast =  Toast.Make($"{resultError}",CommunityToolkit.Maui.Core.ToastDuration.Long,14);
                await toast.Show();
            }

            //var errorMsg = result.ErrorMsg;
        }
        public async Task RegisterTapped()

        {
            await Shell.Current.GoToAsync("..");

        }


        public async Task SignInTapped()
        {
            if (!string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password))
            {

                var requestModel = new UserLoginRequest
                {
                    Email = Email,
                    Password = Password,
                };

                var request =await _userServices.LoginUser(requestModel);
                if (request == true)
                {
                    await Shell.Current.DisplayAlert("Success", "User login successful", "Ok!");
                }
                else 
                {
                    await Shell.Current.DisplayAlert("Error", "Invalid Credential!!", "Ok!");

                }
            }

        }

    }
}
