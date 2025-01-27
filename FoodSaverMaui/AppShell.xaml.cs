using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.ViewModel;
using FoodSaverMaui.Views;

namespace FoodSaverMaui
{
    public partial class AppShell : Shell
    {
 
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(CreateAccount), typeof(CreateAccount));
            Routing.RegisterRoute(nameof(FindFood), typeof(FindFood));
            Routing.RegisterRoute(nameof(LandingPage), typeof(LandingPage));
            Routing.RegisterRoute(nameof(UserProfile), typeof(UserProfile));
            Routing.RegisterRoute(nameof(UploadFood), typeof(UploadFood));
            Routing.RegisterRoute(nameof(UpdatePassword), typeof(UpdatePassword));
            Routing.RegisterRoute(nameof(DeleteUser), typeof(DeleteUser));
            Routing.RegisterRoute(nameof(UpdateEmail), typeof(UpdateEmail));
            Routing.RegisterRoute(nameof(FoodDetail), typeof(FoodDetail));
            Routing.RegisterRoute(nameof(PostSuccessfullPage), typeof(PostSuccessfullPage));
            Routing.RegisterRoute(nameof(PaymentUrl), typeof(PaymentUrl));
            Routing.RegisterRoute(nameof(KhaltiPaymentView), typeof(KhaltiPaymentView));
            Routing.RegisterRoute(nameof(UploadPostSucessfull), typeof(UploadPostSucessfull));
            Routing.RegisterRoute(nameof(EditProfile), typeof(EditProfile));
            Routing.RegisterRoute(nameof(OrderDetail), typeof(OrderDetail));
            Routing.RegisterRoute(nameof(ConfirmPaymentUrl), typeof(ConfirmPaymentUrl));
            Routing.RegisterRoute(nameof(ConfirmPaymentSucessfull), typeof(ConfirmPaymentSucessfull));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //IsLoggedIn = false;
            //var token = await SecureStorage.GetAsync("token");
            var checkLogin = await SecureStorage.GetAsync("isLoggedOut");
            if (checkLogin != null)

            {
                if (checkLogin == "yes")
                {
                    await GoToAsync("//Login");


                }
                else
                { await GoToAsync("//HomePage"); }
            }
            else { await GoToAsync("//Login"); }


        }
    }
}
