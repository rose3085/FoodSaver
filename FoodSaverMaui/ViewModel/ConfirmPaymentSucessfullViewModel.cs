using FoodSaverMaui.Model;
using FoodSaverMaui.Services.SalesRecord;
using CommunityToolkit.Maui.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodSaverMaui.Views;

namespace FoodSaverMaui.ViewModel
{
   public partial class ConfirmPaymentSucessfullViewModel : BaseViewModel
    {
        private readonly SalesRecordServices _salesRecordService;

        public Command OnPageMount { get; }
        public ConfirmPaymentSucessfullViewModel(SalesRecordServices salesRecordServices)
        {
            _salesRecordService = salesRecordServices;
            OnPageMount = new Command(async () => PageMount());
        }

        private void RestartApp()
        {
            // Get the current application instance
            var currentApp = Application.Current;

            // Create a new instance of AppShell
            var newShell = new AppShell();

            // Set the new shell as the main page
            currentApp.MainPage = newShell;

        }
        public async Task PageMount()
        {
            try {
                var revenueModel = new SellerRevenueModel()
                {
                    Amount = 20.0,
                    PidX ="gsdvfgdsvgvfdgsvh"
                };
                var request = await _salesRecordService.PostSellerRevenue(revenueModel);
                if (request != null)
                {

                    //await Shell.Current.GoToAsync("//TempPage");
                    //await Task.Delay(200);
                    ////var homePage = Shell.Current.Items[2];
                    //var homePage = Shell.Current.Items.FirstOrDefault(x => x.Title == null && x.Route == "IMPL_HomePage");
                    //if (homePage != null)
                    //{
                    //    Shell.Current.Items.Remove(homePage);
                    //    await Task.Delay(100);
                    //}

                    //Routing.UnRegisterRoute("HomePage");

                    //Shell.Current.Items.Add(new ShellContent
                    //{
                    //    Route = "HomePage",
                    //    ContentTemplate = new DataTemplate(typeof(HomePage))
                    //});

                    //await Shell.Current.GoToAsync("//HomePage");

                    var message = "Payement successfull";
                    var toast = Toast.Make($"{message}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await toast.Show();
                    //IsSeller = true;
                    // string limitReached = request.dailyLimitReached.ToString();
                    SecureStorage.Remove("dailyLimitReached");

                    //RestartApp();

                }
                else {


                    var message = "Something went worng!!.Payement was unsuccessfull";
                    var toast = Toast.Make($"{message}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await toast.Show();

                }

            } catch { }
        }
    }
}
