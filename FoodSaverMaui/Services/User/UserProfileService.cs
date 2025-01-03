using FoodSaverMaui.Helper;
using FoodSaverMaui.Model;
using FoodSaverMaui.Response;
using FoodSaverMaui.Response.User;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Services.User
{
    public class UserProfileService
    {
        private readonly HttpClient _httpClient;
        private readonly IJwtHelper _jwtHelper;

        public UserProfileService(HttpClient httpClient, IJwtHelper jwtHelper)
        {
            _httpClient = httpClient;
            _jwtHelper = jwtHelper;
        }

        public async Task<ApplicationUser> GetUserByName()
        {
            var jwtToken = await SecureStorage.GetAsync("token");
            if (jwtToken == null)
            {

                return null;
            }
            var userName = _jwtHelper.ExtractUserInfo(jwtToken);
            var url = $"{App.Settings.ApiBaseUrl}/api/User/GetUserByName?userName={userName}";
            //var json = JsonConvert.SerializeObject(request);
            // var content = new StringContent(json, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApplicationUser>();
                return result;

            }
            else { return null; }
        }
        public async Task<string> UpdatePassword(UpdatePasswordRequest request)
        {
            try
            {
                

                var url = $"{App.Settings.ApiBaseUrl}/api/User/UpdatePassword";
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                Console.WriteLine($"s Code: {(int)response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserManagerResponse>();
                    if (result.IsSuccess == true)
                    {
                        
                        return result.Message;
                    }

                    else
                    {
                        return result.Message;
                    }
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


        public async Task<string> UpdateEmail(UpdateEmailRequest request)
        {
            try
            {

                var url = $"{App.Settings.ApiBaseUrl}/api/User/UpdateEmail";
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                Console.WriteLine($"s Code: {(int)response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserManagerResponse>();
                    if (result.IsSuccess == true)
                    {

                        return result.Message;
                    }

                    else
                    {
                        return result.Message;
                    }
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


        public async Task<string> DeleteUser(UserLoginRequest request)
        {
            try
            {
                var jwtToken = await SecureStorage.GetAsync("token");
                if (jwtToken == null)
                {

                    return null;
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
                       
                        SecureStorage.RemoveAll();
                        return result.Message;
                    }

                    else
                    {
                        return result.Message;
                    }
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
