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
    public partial class EditProfileViewModel : BaseViewModel
    {
        private readonly UserProfileService _userProfileService;
        //public CurrentUserInfo userModel {  get; set; }

        [ObservableProperty]
        string userName;

        [ObservableProperty]
        string email;

        [ObservableProperty]
        string phoneNumber;

        //[ObservableProperty]
        //IList<string> role;


        public Command OnSaveButtonClicked { get; }
        public Command OnGetUserInfo { get; }

        public EditProfileViewModel(UserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
            OnSaveButtonClicked = new Command(async () => await SaveButtonClicked());
            OnGetUserInfo = new Command(async() => await GetUserInfo());
        }

        public async Task GetUserInfo()
        {
            var user = await _userProfileService.GetUserByName();

            if (user != null)
            {    UserName = user.UserName;
                Email = user.Email;
                PhoneNumber = user.PhoneNumber;
            }
        }
        public async Task SaveButtonClicked()
        {
            var password = await Shell.Current.DisplayPromptAsync("Confirm","Enter your password?","Ok","Cancel");
            //if(password != null)
        }
    }
}
