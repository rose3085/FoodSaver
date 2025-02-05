using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Maui.Alerts;
using FoodSaverMaui.Model;
using FoodSaverMaui.Response.FoodOrder;
using FoodSaverMaui.Services.OrderDelivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location = Microsoft.Maui.Devices.Sensors.Location;

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
        private readonly DeliveryService _deliveryService;

        public Command OnPinLocationTapped { get; }
        public Command OnCheckDeliveryStatus { get; }
        public Command OnUpdateDeliveryStatusTapped { get; }
        public OrderDetailViewModel(DeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
            OnCheckDeliveryStatus = new Command(async() => CheckDeliveryStatus());
            OnUpdateDeliveryStatusTapped = new Command(async() => UpdateDeliveryStatus());
            OnPinLocationTapped = new Command(async () => await PinLocationTapped());
        }
        public async Task PinLocationTapped()
        {
            if (!string.IsNullOrEmpty(OrderDetail.WardNumber) && !string.IsNullOrEmpty(OrderDetail.ToleName)
                    && !string.IsNullOrEmpty(OrderDetail.CityName))
            {

                var address = $"{OrderDetail.CityName} {OrderDetail.ToleName}";

                IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync(address);
                Location location = locations?.FirstOrDefault();

                var options = new MapLaunchOptions { Name = "Butwal" };

                try
                {
                    await Map.Default.OpenAsync(location, options);
                }
                catch (Exception ex)
                {
                    var toasts = Toast.Make($"Couldn't open GoogleMap", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await toasts.Show();

                }
            }
            else
            {

                var toast = Toast.Make($"Enter all address fields!", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                await toast.Show();
            }

        }
        public async Task UpdateDeliveryStatus()
        {

            try {

                var newStatus = await Shell.Current.DisplayActionSheet("Update delivery status?","Cancel", null, "Delivered","Not Delivered");
                if (newStatus == "Delivered")
                {
                    var requestModel = new UpdateDeliveryStatusModel()
                    { 
                        OrderId = orderDetail.OrderId,
                        IsDelivered = true,
                    };
                    var request = await _deliveryService.UpdateDeliveryStatus(requestModel);
                    if(request != null)
                    {
                        if (request.IsSuccess == true)
                        {
                            Status = newStatus;
                        }
                        var toast = Toast.Make($"{request.Message}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                        await toast.Show();
                    }
                
                }

                if (newStatus == "Not Delivered")
                {
                    var requestModel = new UpdateDeliveryStatusModel()
                    {
                        OrderId = orderDetail.OrderId,
                        IsDelivered = false,
                    };
                    var request = await _deliveryService.UpdateDeliveryStatus(requestModel);
                    if (request != null)
                    {
                        if (request.IsSuccess == true)
                        {
                            Status = newStatus;
                        }
                        var toast = Toast.Make($"{request.Message}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                        await toast.Show();
                    }
                }
            } catch { }
        
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
