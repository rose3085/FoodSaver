using FoodSaverMaui.Helper;
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
        private readonly IJwtHelper _jwtHelper;

        public KhaltiService(HttpClient httpClient, IJwtHelper jwtHelper)
        {
            _httpClient = httpClient;
            _jwtHelper = jwtHelper;
        }
        public async Task<string> KhaltiLaunch(string amount, string productId)
        {
            var buyerToken = await SecureStorage.GetAsync("token");
            var buyer =  _jwtHelper.ExtractUserInfo(buyerToken);
            if (buyer == null)
            {
                return null;
            }
          
            
            var initiateUrl = $"https://a.khalti.com/api/v2/epayment/initiate/";
           
            var payload = new
            {
                return_url = $"{App.Settings.ApiBaseUrl}/PaymentReturn/ReturnUrl",
                website_url = "https://pay.khalti.com",
                amount = 1000,
                purchase_order_id = productId,
                purchase_order_name = buyer,
                customer_info = new
                {
                    name = $"{buyer}",
                   // email = "rabinasedhai1@gmail.com",
                   
                },

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
                    await SecureStorage.SetAsync("pidx",khaltiResponse.pidx);

                    //var responseContent = await response.Content.ReadAsStringAsync();
                    //if (paymentUrl != null)
                    //{
                    //    await Launcher.OpenAsync($"{paymentUrl}");
                    //}
                    return paymentUrl;
                }
            }

            return null;
        }
    

}
}
