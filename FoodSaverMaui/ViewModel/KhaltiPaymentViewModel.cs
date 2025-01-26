using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.KhaltiServices;
using FoodSaverMaui.SignalRServices;
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
    [QueryProperty(nameof(CityName), "CityName")]
    [QueryProperty(nameof(ToleName), "ToleName")]
    [QueryProperty(nameof(WardNumber), "WardNumber")]
    public partial class KhaltiPaymentViewModel : BaseViewModel
    {

        private string _cityName;

        public string CityName
        {
            get => _cityName;
            set
            {
                if (_cityName != value)
                {
                    _cityName = value;
                    OnPropertyChanged(nameof(CityName));
                }
            }
        }

        private string _toleName;

        public string ToleName
        {
            get => _toleName;
            set
            {
                if (_toleName != value)
                {
                    _toleName = value;
                    OnPropertyChanged(nameof(ToleName));
                }
            }
        }

        private string _wardNumber;

        public string WardNumber
        {
            get => _wardNumber;
            set
            {
                if (_wardNumber != value)
                {
                    _wardNumber = value;
                    OnPropertyChanged(nameof(WardNumber));
                }
            }
        }


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
        private readonly ISignalRService _signalRService;

        public Command OnKhaltiPaymentButtonClicked { get; }
        public KhaltiPaymentViewModel(IKhaltiService khaltiServices,ISignalRService signalRService)
        {
            _khaltiServices = khaltiServices;
            _signalRService = signalRService;
            OnKhaltiPaymentButtonClicked = new Command(async() => await KhaltiPaymentButton());
           
        }





        public async Task KhaltiPaymentButton()
        {
            await SecureStorage.SetAsync("amount",Amount);
            await SecureStorage.SetAsync("productId",ProductId);
            await SecureStorage.SetAsync("cityName", CityName);
            await SecureStorage.SetAsync("toleName", ToleName);
            await SecureStorage.SetAsync("wardNumber", WardNumber);
           
            string pay = await _khaltiServices.KhaltiLaunch(Amount, ProductId);
            if (pay != null)
            {
                // await Shell.Current.GoToAsync($"{nameof(PaymentUrl)}?url={Uri.EscapeDataString(pay)}");
                await Shell.Current.GoToAsync($"{nameof(PaymentUrl)}?url={Uri.EscapeDataString(pay)}");
            }
            //await _signalRService.SendNotification("dd27b90-05b9-49a3-a2d6-5271d50b6c41","MeowwwwwwwwwwwwwwBhowwwwwwwwww");
        }
    }
}
