using FoodSaverMaui.Model;
using FoodSaverMaui.Services.User;
using CommunityToolkit.Maui.Alerts;
using FoodSaverMaui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{
    public partial class CreateAccountViewModel : BaseViewModel
    {
        private readonly UserServices _userServices;


        private bool _isRadioButtonsVisible;

        public bool IsRadioButtonsVisible
        {
            get => _isRadioButtonsVisible;
            set
            {
                _isRadioButtonsVisible = value;
                OnPropertyChanged();
            }
        }

        private string _username;
        public string UserName
        {
            get => _username;
            set
            {
                _username = value;
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

        public string _phone;
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));

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

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }

        }


        private bool isBuyerChecked;
        public bool IsBuyerChecked
        {
            get => isBuyerChecked;
            set
            {
                if (isBuyerChecked != value)
                {
                    isBuyerChecked = value;
                    OnPropertyChanged();
                    SelectedRole = value ? "Buyer" : SelectedRole; // Update selected role
                }
            }
        }

        private bool isSellerChecked;
        public bool IsSellerChecked
        {
            get => isSellerChecked;
            set
            {
                if (isSellerChecked != value)
                {
                    isSellerChecked = value;
                    OnPropertyChanged();
                    SelectedRole = value ? "Seller" : SelectedRole; // Update selected role
                }
            }
        }

        private string selectedRole;
        public string SelectedRole
        {
            get => selectedRole;
            set
            {
                if (selectedRole != value)
                {
                    selectedRole = value;
                    OnPropertyChanged();
                }
            }
        }


        public Command ToggleRadioButtonsCommand { get; }
        public Command OnRegisterTapped { get; }
        public Command OnSignInTapped { get; }
        public CreateAccountViewModel(UserServices userServices)
        {
            ToggleRadioButtonsCommand = new Command(async () => await OnToggleRadioButtonsCommand());
            OnRegisterTapped = new Command(async () => await RegisterTapped());
            OnSignInTapped = new Command(async () => await SignInTapped());
            _userServices = userServices;
        }


        public async Task SignInTapped()
        {

            await Shell.Current.GoToAsync("//Login");

        }

        public async Task RegisterTapped()
        {

            if (!string.IsNullOrWhiteSpace(UserName))
            {
                if (!string.IsNullOrWhiteSpace(Email) &&
                  !string.IsNullOrWhiteSpace(Phone) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(ConfirmPassword)
                  && !string.IsNullOrWhiteSpace(SelectedRole) && Password == ConfirmPassword)
                {
                    try
                    {

                        IsBusy = true;

                        var requestModel = new UserRegisterRequest
                        {
                            UserName = UserName,
                            Email = Email,
                            PhoneNumber = Phone,
                            Password = Password,
                            ConfirmPassword = ConfirmPassword,

                        };

                        var registerRequest = await _userServices.RegisterUser(requestModel, SelectedRole);
                        if (registerRequest == true)
                        {
                            //await Shell.Current.DisplayAlert("Success", "User successfully registered.", "OK!");
                            await Shell.Current.GoToAsync("//Login");
                            string resultError = "Enter registerd data to login!";
                            var toast = Toast.Make($"{resultError}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                            await toast.Show();
                        }
                        else
                        {

                            await Shell.Current.DisplayAlert("Couldn't register user", "Please try again.", "OK!");
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
            else
            {
                string resultError = "Enter Valid Credentials!!";
                var toast = Toast.Make($"{resultError}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                await toast.Show();

            }
        }

    

        public async Task OnToggleRadioButtonsCommand()
        {
            IsRadioButtonsVisible = !IsRadioButtonsVisible;

        }

    }
}
