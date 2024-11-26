using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Views;
using Plugin.Maui.Biometric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{
   public partial class UserProfileViewModel : BaseViewModel
    {
        public Command OnEnableFingerprintTapped { get; }
        public Command OnChangePasswordTapped { get; }
        public Command OnDeleteUserTapped { get; }
        public Command OnChangeEmailTapped { get; }
        public Command OnLogoutTapped { get; }
        public UserProfileViewModel()
        {
            OnEnableFingerprintTapped = new Command(async () => await EnableFingerPrintTapped());
            OnChangePasswordTapped = new Command(async () => await ChangePasswordTapped());
            OnDeleteUserTapped = new Command(async () => await DeleteUserTapped());
            OnChangeEmailTapped = new Command(async () => await ChangeEmailTapped());
            OnLogoutTapped = new Command(async() => await LogoutTapped());
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
