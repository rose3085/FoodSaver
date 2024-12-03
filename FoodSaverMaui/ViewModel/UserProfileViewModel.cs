using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Response;
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
        public ObservableCollection<GetProductsResponse> Products { get; } = new();

        private bool _isComponent1Visible = true;

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

        public Command OnEnableFingerprintTapped { get; }
        public Command OnChangePasswordTapped { get; }
        public Command OnDeleteUserTapped { get; }
        public Command OnChangeEmailTapped { get; }
        public Command OnLogoutTapped { get; }
        public Command ToggleCommand { get; }
        public UserProfileViewModel()
        {
            OnEnableFingerprintTapped = new Command(async () => await EnableFingerPrintTapped());
            OnChangePasswordTapped = new Command(async () => await ChangePasswordTapped());
            OnDeleteUserTapped = new Command(async () => await DeleteUserTapped());
            OnChangeEmailTapped = new Command(async () => await ChangeEmailTapped());
            OnLogoutTapped = new Command(async() => await LogoutTapped());
            ToggleCommand = new Command(async () => await OnToggleCommand());
            IsComponent1Visible = true;
            IsComponent2Visible = false;
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
