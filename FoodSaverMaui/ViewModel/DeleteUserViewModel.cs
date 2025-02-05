using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Services.User;
using FoodSaverMaui.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodSaverMaui.Views;
using FoodSaverMaui.Helper.CacheHelper;

namespace FoodSaverMaui.ViewModel
{
   public partial class DeleteUserViewModel : BaseViewModel
    {
        [ObservableProperty]
        string email;

        [ObservableProperty]
        string password;
        [ObservableProperty]
        string userName;

      


        private readonly UserProfileService _userProfileService;
        private readonly ICacheService _cacheService;

        public Command OnConfirmTapped { get; }
        public DeleteUserViewModel(UserProfileService userProfileService,ICacheService cacheService)
        {
            _userProfileService = userProfileService;
            _cacheService = cacheService;
            OnConfirmTapped = new Command(async () => await ConfirmTapped());
        }
        public async Task ConfirmTapped()
        {
            try
            {
                IsBusy = true;
                if (!string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(UserName) &&
                    !string.IsNullOrWhiteSpace(Password)
                    )
                {
                    var requestModel = new UserLoginRequest
                    {
                        UserName = UserName,
                        Email = Email,
                        Password = Password,
                      

                    };

                    var result = await _userProfileService.DeleteUser(requestModel);
                    if (result != null)
                    {

                        //var toast = Toast.Make($"{result}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                        //await toast.Show();

                        if (result == "User Deleted Sucessfully")

                        {
                            var toast = Toast.Make($"{result}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                            await toast.Show();
                            SecureStorage.RemoveAll();
                            Preferences.Clear();
                            await _cacheService.Clear();

                            await Shell.Current.GoToAsync("//Login");
                        }
                        else
                        {
                            var toast = Toast.Make($"{result}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                            await toast.Show();
                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Password update failed", "Please try again later", "Ok");

                    }
                }
                else
                {
                    var result = "Please enter all the required fields!!";
                    var toast = Toast.Make($"{result}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await toast.Show();
                }

            }
            finally
            {
                IsBusy = false;

            }


        }
    }
}
