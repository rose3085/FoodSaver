using FoodSaverMaui.KhaltiServices;
using FoodSaverMaui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{
    public partial class KhaltiPaymentViewModel : BaseViewModel
    {
        private readonly IKhaltiService _khaltiServices;
        public Command OnKhaltiPaymentButtonClicked { get; }
        public KhaltiPaymentViewModel(IKhaltiService khaltiServices)
        {
            _khaltiServices = khaltiServices;
            OnKhaltiPaymentButtonClicked = new Command(async() => await KhaltiPaymentButton());
        }

        public async Task KhaltiPaymentButton()
        {
            string pay = await _khaltiServices.KhaltiLaunch();
            if (pay != null)
            {
                // await Shell.Current.GoToAsync($"{nameof(PaymentUrl)}?url={Uri.EscapeDataString(pay)}");
                await Shell.Current.GoToAsync($"{nameof(PaymentUrl)}?url={Uri.EscapeDataString(pay)}");
            }
        }
    }
}
