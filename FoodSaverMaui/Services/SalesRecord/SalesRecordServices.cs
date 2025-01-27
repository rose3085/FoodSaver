using FoodSaverMaui.Helper;
using FoodSaverMaui.Model;
using FoodSaverMaui.Response;
using FoodSaverMaui.Response.SalesRecord;
using Newtonsoft.Json;
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

        public async Task<GetSalesRecordResponse> GetSalesRecord()
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
                    return result;

                }
                else
                {

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public async Task<ResponseManager> PostSellerRevenue(SellerRevenueModel requestModel)
        {
            try
            {
                var jwtToken = await SecureStorage.GetAsync("token");
                if (jwtToken == null)
                {

                    return null;
                }
                var url = $"{App.Settings.ApiBaseUrl}/api/SalesRecord/SellerRevenueUpdate";
                var json = JsonConvert.SerializeObject(requestModel);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                Console.WriteLine($"s Code: {(int)response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseManager>();
                    if (result.IsSuccess == true)
                    {
                        return result;
                    }
                    else
                    { return null; }
                }
                else
                {

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;

            }
        }

    }
}
