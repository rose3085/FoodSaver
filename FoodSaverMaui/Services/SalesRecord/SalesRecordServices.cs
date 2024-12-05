using FoodSaverMaui.Helper;
using FoodSaverMaui.Response.SalesRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Services.SalesRecord
{
    public class SalesRecordServices
    {
        private readonly HttpClient _httpClient;
        private readonly IJwtHelper _jwtHelper;

        public SalesRecordServices(HttpClient httpClient,IJwtHelper jwtHelper)
        {
            _httpClient = httpClient;
            _jwtHelper = jwtHelper;
        }

        public async Task<double> GetSalesRecord()
        {
            try
            {
                var jwtToken = await SecureStorage.GetAsync("token");
                if (string.IsNullOrEmpty(jwtToken))
                {
                    throw new InvalidOperationException("Token not found.");
                }
                var userName =  _jwtHelper.ExtractUserInfo(jwtToken);
                var url = $"{App.Settings.ApiBaseUrl}/api/SalesRecord/GetSingleRecord?userName={userName}";
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                var response = await _httpClient.GetAsync(url, CancellationToken.None);
                Console.WriteLine($"s Code: {(int)response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {

                    var result = await response.Content.ReadFromJsonAsync<GetSalesRecordResponse>();
                    return result.newAmount;

                }
                else
                {

                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }
}
