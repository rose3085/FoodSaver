using FoodSaverMaui.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.KhaltiServices
{
    public partial class KhaltiService : IKhaltiService
    {
        private readonly HttpClient _httpClient;

        public KhaltiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> KhaltiLaunch()
        {
            var initiateUrl = $"https://a.khalti.com/api/v2/epayment/initiate/";
            var payload = new
            {
                return_url = "https://example.com",
                website_url = "https://pay.khalti.com",
                amount = "1000",
                purchase_order_id = "Order01",
                purchase_order_name = "test",
                customer_info = new
                {
                    name = "Ram Bahadur",
                    email = "rabinasedhai1@gmail.com",
                    phone = "9800000001"
                }
            };
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Key live_secret_key_68791341fdd94846a146f0457ff7b455");

            var response = await client.PostAsync(initiateUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                if (result != null)
                {
                    var khaltiResponse = JsonConvert.DeserializeObject<KhaltiResponse>(result);
                    var paymentUrl = khaltiResponse.payment_url;
                    //var responseContent = await response.Content.ReadAsStringAsync();
                    if (paymentUrl != null)
                    {
                        await Launcher.OpenAsync($"{paymentUrl}");
                    }
                }
            }

            //var location = new Location(47.645160, -122.1306032);
            //var options = new MapLaunchOptions { Name = "Microsoft Building 25" };

            //try
            //{
            //    await Map.Default.OpenAsync(location, options);
            //}
            //catch (Exception ex)
            //{
            //    // No map application available to open
            //}

            return true;
        }
    

}
}
