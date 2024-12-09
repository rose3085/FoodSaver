
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {
        public BaseViewModel()
        {

        }

        [ObservableProperty]
        //[AlsoNotifyChangeFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title;

        public bool IsNotBusy => IsBusy;

        [ObservableProperty]
       
        bool isFingerPrintEnabled;

        [ObservableProperty]

       public bool isSalesLimitReached;

    }
}
