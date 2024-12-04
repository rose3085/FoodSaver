
using FoodSaverMaui.Model;
using FoodSaverMaui.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;

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

        public async Task<IEnumerable<GetProductsResponse>> GetUserProducts()
        {
            try
            {
                var jwtToken = await SecureStorage.GetAsync("token");
                if (string.IsNullOrEmpty(jwtToken))
                {
                    throw new InvalidOperationException("Token not found.");
                }
                var url = $"{App.Settings.ApiBaseUrl}/api/Food/GetUserFood";
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



        public async Task<string> PostFood(string productName, string description, double pricePerKg, double quantity,string wardNumber, string toleName, string cityName,double latitude,double longitude, byte[] imageData, string imageName)
        {

            try
            {
                var jwtToken = await SecureStorage.GetAsync("token");
                if (jwtToken == null)
                {

                    return null;
                }
                var url = $"{App.Settings.ApiBaseUrl}/api/Food/AddFood";
                //var json = JsonConvert.SerializeObject(request);
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent(productName), "ProductName");
                    formData.Add(new StringContent(description), "Description");
                    formData.Add(new StringContent(pricePerKg.ToString(CultureInfo.InvariantCulture)), "PricePerKg");
                    formData.Add(new StringContent(quantity.ToString(CultureInfo.InvariantCulture)), "Quantity");
                    
                    formData.Add(new StringContent(wardNumber), "WardNumber");
                    formData.Add(new StringContent(toleName), "ToleName");
                    formData.Add(new StringContent(cityName), "CityName");
                    formData.Add(new StringContent(latitude.ToString(CultureInfo.InvariantCulture)), "Latitude");
                    formData.Add(new StringContent(longitude.ToString(CultureInfo.InvariantCulture)), "Longitude");


                    if (imageData != null && imageData.Length > 0)
                    {
                        var fileContent = new ByteArrayContent(imageData);
                        string fileExtension = Path.GetExtension(imageName).ToLower();
                        switch (fileExtension)
                        {
                            case ".jpg":
                            case ".jpeg":
                                fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                                break;
                            case ".png":
                                fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                                break;
                            default:
                                
                                Console.WriteLine("Unsupported file type.");
                                return "Image must be .jpg or .jpeg or .png"; 
                        }

                        formData.Add(fileContent, "ImageFile", imageName);
                    }
                    //var content = new StringContent(json, Encoding.UTF8, "application/json");
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                    var response = await _httpClient.PostAsync(url, formData);
                    Console.WriteLine($"s Code: {(int)response.StatusCode}");
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<UserManagerResponse>();
                        if (result.IsSuccess == true)
                        {

                            //SecureStorage.RemoveAll();
                            return result.Message;

                           // return true;
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
            }
            catch (Exception ex)
            {

                return null;
            }


        }


       
    }
}
