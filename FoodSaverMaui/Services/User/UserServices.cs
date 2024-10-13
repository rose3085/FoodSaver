
using FoodSaverMaui.Model;
using FoodSaverMaui.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Services.User
{
    public class UserServices
    {
        private readonly HttpClient _httpClient;
        //var url = $"{App.Settings.ApiBaseUrl}/v1/cms/register/bus-station";
        public UserServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<bool> LoginUser(UserLoginRequest loginRequest)
        {
            try
            {

                var url = $"{App.Settings.ApiBaseUrl}/api/User/LoginUser";
                var json = JsonConvert.SerializeObject(loginRequest) ;
                var content = new StringContent(json, Encoding.UTF8,"application/json");
                var response = await _httpClient.PostAsync(url, content) ;
                Console.WriteLine($"s Code: {(int)response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserLoginResponse>();
                    if (result.IsSuccess == true)
                    {
                        return true;
                    }
                    else 
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        
        }


        public async Task<bool> RegisterUser(UserRegisterRequest registerRequest,string role)
        {
            try
            {
                var url = $"{App.Settings.ApiBaseUrl}/api/User/RegisterUser?role={role}";
                //var url = $"https://localhost:7293/api/User/RegisterUser?role={role}";
                var json = JsonConvert.SerializeObject(registerRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                var response = await _httpClient.PostAsync(url, content);
                Console.WriteLine($"s Code: {(int)response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    //var result =  response.Content.ReadFromJsonAsync<>();
                    return true;
                }
                else
                {

                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
