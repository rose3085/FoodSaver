using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Model;
using FoodSaverMaui.Services.User;

namespace FoodSaverMaui.ViewModel
{
    public partial class UpdatePasswordViewModel : BaseViewModel
    {
        [ObservableProperty]
        string email;

        [ObservableProperty]
        string oldPassword;

        [ObservableProperty]
        string newPassword;

        [ObservableProperty]
        string confirmPassword;
        private readonly UserProfileService _userProfileService;

        public Command OnConfirmTapped { get; }
        public UpdatePasswordViewModel(UserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
            OnConfirmTapped = new Command(async() => await ConfirmTapped());
        }
        public async Task ConfirmTapped()
        {
            try
            {
                IsBusy= true;
                if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(oldPassword) &&
                    !string.IsNullOrWhiteSpace(newPassword) && !string.IsNullOrWhiteSpace(confirmPassword) && newPassword == confirmPassword
                    )
                {
                    var requestModel = new UpdatePasswordRequest
                    {
                        Email = email,
                        OldPassword = oldPassword,
                        NewPassword = newPassword,
                        ConfirmNewPassword = confirmPassword

                    };

                    var result = await _userProfileService.UpdatePassword(requestModel);
                    if (result != null)
                    {

                        var toast = Toast.Make($"{result}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                        await toast.Show();

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
