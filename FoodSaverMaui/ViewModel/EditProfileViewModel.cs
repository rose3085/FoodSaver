using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Model;
using FoodSaverMaui.Services.User;
using FoodSaverMaui.Views;
using System.Text.Json;
using System.Text.RegularExpressions;

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

        [ObservableProperty]
       string role;

        [ObservableProperty]
        string newRole = "string";
        [ObservableProperty]
        string newPhoneNumber;

        public Command OnSaveButtonClicked { get; }
        public Command OnGetUserInfo { get; }

        public Command OnUpdateRoleTapped { get; }

        public EditProfileViewModel(UserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
            OnSaveButtonClicked = new Command(async () => await SaveButtonClicked());
            OnGetUserInfo = new Command(async() => await GetUserInfo());
            OnUpdateRoleTapped = new Command(async() => await UpdateRole());
        }

        public bool IsValidArrayFormat(string input)
        {
            // Regular expression to match the format ["string","string",...]
            string pattern = @"^\[(\"".*?\""(,\s*\"".*?\"")*)?\]$";
            return Regex.IsMatch(input, pattern);
        }
        public async Task GetUserInfo()
        {
            var user = await _userProfileService.GetUserByName();

            if (user != null)
            {
                var roles = await _userProfileService.GetUserRoles();

                if (roles != null)
                {
                    if (IsValidArrayFormat(roles))
                    {
                        string trimmed = roles.Trim('[', ']');
                        Role = trimmed.Replace("\"", "");
                    }
                    else
                    {
                        Role = roles;
                    }

                    UserName = user.UserName;
                    Email = user.Email;
                    PhoneNumber = user.PhoneNumber;

                }
            }
        }


        public async Task UpdateRole()
        {
            if (Role == "Buyer")
            {
                var result = await Shell.Current.DisplayAlert("Update Role", "Do you want to become a Seller?", "Ok", "Cancel");
                if (result == true)
                {
                    newRole = "Seller";
                //    var listRole = new List<string>()
                //{"Seller","Buyer"
                //};
                //    var roles = JsonSerializer.Serialize<List<string>>(listRole);
                //    await SecureStorage.SetAsync("roles", roles);
                }
                else { newRole = "string"; }
                
            }
            else if (Role == "Seller")
            {
                var result = await Shell.Current.DisplayAlert("Update Role", "Do you want to become a buyer?", "Ok", "Cancel");
                if (result == true)
                {
                    newRole = "Buyer";
                //    var listRole = new List<string>()
                //{"Seller","Buyer"
                //                };
                //    var roles = JsonSerializer.Serialize<List<string>>(listRole);
                //    await SecureStorage.SetAsync("roles", roles);
                }
                else { newRole = "string"; }
            }
            else 
            {
                var result = await Shell.Current.DisplayAlert("Couldn't update role!", "You're already buyer and seller", "Ok", "Cancel");
                newRole = "string";
            }
        }

        public async Task SaveButtonClicked()
        {
            try
            {
                IsBusy = true;
                var password = await Shell.Current.DisplayPromptAsync("Confirm", "Enter your password?", "Ok", "Cancel");
                if (password != null)
                {
                    if (PhoneNumber == null)
                    {
                        newPhoneNumber = "string";
                    }
                    else
                    {
                        newPhoneNumber = PhoneNumber;
                    }
                    var requestModel = new UpdateUserRequest()
                    {
                        Password = password,
                        Email = Email,
                        UserName = UserName,
                        Role = newRole,
                        PhoneNumber = newPhoneNumber
                    };
                    var result = await _userProfileService.UpdateUser(requestModel);
                    if (result == true)
                    {
                        if (newRole != "string")
                        {
                            var listRole = new List<string>()
                            {"Seller","Buyer"
                            };
                            var roles = JsonSerializer.Serialize<List<string>>(listRole);
                            await SecureStorage.SetAsync("roles", roles);
                        }
                        
                       

                        await Shell.Current.GoToAsync("//TempPage"); 
                        await Task.Delay(200); 
                        //var homePage = Shell.Current.Items[2];
                        var homePage = Shell.Current.Items.FirstOrDefault(x => x.Title == null && x.Route == "IMPL_HomePage");
                        if (homePage != null)
                        {
                            Shell.Current.Items.Remove(homePage);
                            await Task.Delay(100); 
                        }

                        Routing.UnRegisterRoute("HomePage");
                       
                        Shell.Current.Items.Add(new ShellContent
                        {
                            Route = "HomePage",
                            ContentTemplate = new DataTemplate(typeof(HomePage))
                        });

                        await Shell.Current.GoToAsync("//HomePage");
                        string message = "User Updated Sucessfully!!";
                        var toast = Toast.Make($"{message}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                        await toast.Show();


                    }
                    else
                    {
                        string message = "Couldn't Update User!!!";
                        var toast = Toast.Make($"{message}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                        await toast.Show();
                    }
                }
            }finally
            { IsBusy = false; }
        }
    }
}
