using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{
    [QueryProperty(nameof(url), "url")]
    public partial class ConfirmPaymentUrlViewModel : BaseViewModel
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

        public Command OnPageMount { get; }
        public ConfirmPaymentUrlViewModel()
        {
            
        }

        public async Task PageMount()
        { 
        
        }
    }
}
