using FoodSaverMaui.Model;
using FoodSaverMaui.Services.SalesRecord;
using CommunityToolkit.Maui.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    await Shell.Current.GoToAsync("//HomePage");
                    var message = "Payement successfull";
                    var toast = Toast.Make($"{message}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await toast.Show();
                    //IsSeller = true;
                    // string limitReached = request.dailyLimitReached.ToString();
                    SecureStorage.Remove("dailyLimitReached");


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
