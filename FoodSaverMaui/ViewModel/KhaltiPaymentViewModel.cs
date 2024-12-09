using FoodSaverMaui.KhaltiServices;
using FoodSaverMaui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{
    [QueryProperty(nameof(Amount), "Amount")]
    [QueryProperty(nameof(ProductId), "ProductId")]
    public partial class KhaltiPaymentViewModel : BaseViewModel
    {
        private string _amount;

        public string Amount
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged(nameof(Amount)); 
                }
            }
        }

        private string _productId;

        public string ProductId
        {
            get => _productId;
            set
            {
                if (_productId != value)
                {
                    _productId = value;
                    OnPropertyChanged(nameof(ProductId));
                }
            }
        }


        private readonly IKhaltiService _khaltiServices;
        public Command OnKhaltiPaymentButtonClicked { get; }
        public KhaltiPaymentViewModel(IKhaltiService khaltiServices)
        {
            _khaltiServices = khaltiServices;
            OnKhaltiPaymentButtonClicked = new Command(async() => await KhaltiPaymentButton());
        }

        public async Task KhaltiPaymentButton()
        {
            await SecureStorage.SetAsync("amount",Amount);
            await SecureStorage.SetAsync("productId",ProductId);
            string pay = await _khaltiServices.KhaltiLaunch(Amount,ProductId);
            if (pay != null)
            {
                // await Shell.Current.GoToAsync($"{nameof(PaymentUrl)}?url={Uri.EscapeDataString(pay)}");
                await Shell.Current.GoToAsync($"{nameof(PaymentUrl)}?url={Uri.EscapeDataString(pay)}");
            }
        }
    }
}
