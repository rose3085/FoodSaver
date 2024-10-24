
using FoodSaverMaui.Model;
using FoodSaverMaui.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Services.Food
{
    public class FoodService
    {
        private readonly HttpClient _httpClient;

        public FoodService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<GetProductsResponse>> GetAllProducts()
        {
            try
            {
                var jwtToken = await SecureStorage.GetAsync("token");
                if (string.IsNullOrEmpty(jwtToken))
                {
                    throw new InvalidOperationException("Token not found.");
                }
                var url = $"{App.Settings.ApiBaseUrl}/api/Food/GetFood";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                var response = await _httpClient.GetAsync(url, CancellationToken.None);
                Console.WriteLine($"s Code: {(int)response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {

                    var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetProductsResponse>>();
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



        public async Task<bool> PostFood(PostFoodRequest request)
        {

            try
            {
                var jwtToken = await SecureStorage.GetAsync("token");
                if (jwtToken == null)
                {

                    return false;
                }
                var url = $"{App.Settings.ApiBaseUrl}/api/User/DeleteUser";
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                var response = await _httpClient.PostAsync(url, content);
                Console.WriteLine($"s Code: {(int)response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserManagerResponse>();
                    if (result.IsSuccess == true)
                    {

                        //SecureStorage.RemoveAll();
                        //return result.Message;

                        return true;
                    }

                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                return null;
            }


        }


        //public async Task<T> AddFood()
        //{ 
        
        
        //}
    }
}
