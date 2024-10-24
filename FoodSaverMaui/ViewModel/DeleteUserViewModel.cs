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

namespace FoodSaverMaui.ViewModel
{
   public partial class DeleteUserViewModel : BaseViewModel
    {
        [ObservableProperty]
        string email;

        [ObservableProperty]
        string password;

      


        private readonly UserProfileService _userProfileService;

        public Command OnConfirmTapped { get; }
        public DeleteUserViewModel(UserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
            OnConfirmTapped = new Command(async () => await ConfirmTapped());
        }
        public async Task ConfirmTapped()
        {
            try
            {
                IsBusy = true;
                if (!string.IsNullOrWhiteSpace(email) && 
                    !string.IsNullOrWhiteSpace(password)
                    )
                {
                    var requestModel = new UserLoginRequest
                    {
                        Email = email,
                        Password = password,
                      

                    };

                    var result = await _userProfileService.DeleteUser(requestModel);
                    if (result != null)
                    {

                        var toast = Toast.Make($"{result}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                        await toast.Show();
                        await Shell.Current.GoToAsync(nameof(Login));
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
