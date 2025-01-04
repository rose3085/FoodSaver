using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.KhaltiServices;
using FoodSaverMaui.Model;
using FoodSaverMaui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{
    //[QueryProperty(nameof(Amount), "Amount")]
    //[QueryProperty(nameof(ProductId), "ProductId")]
    //[QueryProperty(nameof(CityName), "CityName")]
    //[QueryProperty(nameof(WardNumber), "WardNumber")]
    //[QueryProperty(nameof(ToleName), "ToleName")]
   

    [QueryProperty(nameof(BuyFoodModel), "PurchaseDetail")]
    public partial class KhaltiPaymentViewModel : BaseViewModel
    {

        private BuyFoodModel buyFoodModel;

        public BuyFoodModel BuyFoodModel
        {
            get => buyFoodModel;
            set
            {
                if (buyFoodModel != value)
                {
                    buyFoodModel = value;


                    CityName = buyFoodModel?.CityName;
                    WardNumber = buyFoodModel?.WardNumber;
                    ToleName = buyFoodModel?.ToleName;
                    Amount = buyFoodModel.Amount.ToString();
                    ProductId = buyFoodModel?.ProductId;

                    OnPropertyChanged(nameof(BuyFoodModel));
                }
            }
        }


        private string cityName;
        public string CityName
        {
            get => cityName;
            set
            {
                if (cityName != value)
                {
                    cityName = value;
                    OnPropertyChanged(nameof(CityName));
                }
            }
        }

        private string toleName;
        public string ToleName
        {
            get => toleName;
            set
            {
                if (toleName != value)
                {
                    toleName = value;
                    OnPropertyChanged(nameof(ToleName));
                }
            }
        }
        private string wardNumber;
        public string WardNumber
        {
            get => wardNumber;
            set
            {
                if (wardNumber != value)
                {
                    wardNumber = value;
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
        public Command OnKhaltiPaymentButtonClicked { get; }
        public KhaltiPaymentViewModel(IKhaltiService khaltiServices)
        {
            _khaltiServices = khaltiServices;
            OnKhaltiPaymentButtonClicked = new Command(async() => await KhaltiPaymentButton());
           
        }

       

        public async Task KhaltiPaymentButton()
        {
            await SecureStorage.SetAsync("amount", _amount);
            await SecureStorage.SetAsync("productId", _productId);
            await SecureStorage.SetAsync("cityName", cityName);
            await SecureStorage.SetAsync("toleName", toleName);
            await SecureStorage.SetAsync("wardNumber", wardNumber);
            string pay = await _khaltiServices.KhaltiLaunch(_amount,_productId);
            if (pay != null)
            {
                // await Shell.Current.GoToAsync($"{nameof(PaymentUrl)}?url={Uri.EscapeDataString(pay)}");
                await Shell.Current.GoToAsync($"{nameof(PaymentUrl)}?url={Uri.EscapeDataString(pay)}");
            }
        }
    }
}
