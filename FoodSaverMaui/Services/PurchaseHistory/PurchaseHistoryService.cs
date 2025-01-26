using FoodSaverMaui.Helper;
using FoodSaverMaui.Response.PurchaseHistory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Services.PurchaseHistory
{
    public class PurchaseHistoryService
    {
        private readonly HttpClient _httpClient;
        private readonly IJwtHelper _jwtHelper;
        public PurchaseHistoryService(HttpClient httpClient, IJwtHelper jwtHelper)
        {
            _httpClient = httpClient;
            _jwtHelper = jwtHelper;
        }


        public async Task<IAsyncEnumerable<GetPurchaseWrapper>> GetUserPurchase()
        {
            try
            {
                var jwtToken = await SecureStorage.GetAsync("token");
                if (jwtToken == null)
                {

                    return null;
                }
                var url = $"{App.Settings.ApiBaseUrl}/api/PurchaseHistory/GetUserPurchaseHistory";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    // var result =  response.Content.ReadFromJsonAsAsyncEnumerable<GetPurchaseHistoryResponse>();
                   
                    var result = response.Content.ReadFromJsonAsAsyncEnumerable<GetPurchaseWrapper>();
                    return result;





                }
                else { return null; }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
