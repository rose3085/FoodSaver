using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Response.FoodOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{
    [QueryProperty(nameof(OrderDetail), "OrderDetail")]
    public partial class OrderDetailViewModel : BaseViewModel
    {
  

        private GetOrderByProductIdResponse orderDetail;

        public GetOrderByProductIdResponse OrderDetail
        {
            get => orderDetail;
            set => SetProperty(ref orderDetail, value);
        }

        [ObservableProperty]
        string status = "Not Delivered";
        public Command OnCheckDeliveryStatus { get; }
        public OrderDetailViewModel()
        {
            OnCheckDeliveryStatus = new Command(async() => CheckDeliveryStatus());
        }

        public async Task CheckDeliveryStatus()
        {
            var isDelivered = OrderDetail.IsDelivered;
            if (isDelivered == true)
            {
                Status = "Delivered";
            }
            else {
                Status = "Not Delivered";
            }
        }

    }
}
