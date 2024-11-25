using FoodSaverMaui.KhaltiServices;

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
            bool pay = await _khaltiServices.KhaltiLaunch();
        }
    }
}
