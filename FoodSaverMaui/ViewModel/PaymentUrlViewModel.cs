using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{
    [QueryProperty(nameof(url), "url")]
    public partial class PaymentUrlViewModel : BaseViewModel
    {

        private string _url;

        public string url
        {
            get => _url;
            set
            {
                if (_url != value)
                {
                    _url = value;
                    OnPropertyChanged(nameof(url)); // Manual property change notification
                }
            }
        }


        //public Command NavigatedPage { get; }
        public PaymentUrlViewModel()
        {
           // NavigatedPage = new Command(async() => await OnPageNavigation());
        }
        
    }
}
