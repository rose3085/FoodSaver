using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Model;
using FoodSaverMaui.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{
    public partial class UpdateEmailViewModel : BaseViewModel
    {
        [ObservableProperty]
        string email;

        [ObservableProperty]
        string password;

        [ObservableProperty]
        string newEmail;


        private readonly UserProfileService _userProfileService;

        public Command OnConfirmTapped { get; }
        public UpdateEmailViewModel(UserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
            OnConfirmTapped = new Command(async () => await ConfirmTapped());
        }


        public async Task ConfirmTapped()
        {
            try
            {
                IsBusy = true;
                if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password) &&
                    !string.IsNullOrWhiteSpace(newEmail) &&  email != newEmail
                    )
                {
                    var requestModel = new UpdateEmailRequest
                    {
                        Email = email,
                        Password = password,
                        NewEmail = newEmail
                    };

                    var result = await _userProfileService.UpdateEmail(requestModel);
                    if (result != null)
                    {

                        var toast = Toast.Make($"{result}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                        await toast.Show();

                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Email update failed", "Please try again later", "Ok");

                    }
                }
                else
                {
                    var result = "Please enter all the required fields!!";
                    var toast = Toast.Make($"{result}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await toast.Show();
                }
            }
            finally { IsBusy = false; }
            }
                }
}
