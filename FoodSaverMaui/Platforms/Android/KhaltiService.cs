using Java.Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Com.Khalti;

namespace FoodSaverMaui.KhaltiServices
{
    public partial class KhaltiService
    {
    //    private readonly HttpClient _httpClient;

    //    public KhaltiService(HttpClient httpClient)
    //    {
    //        _httpClient = httpClient;
    //    }
    //    public async Task<bool> KhaltiLaunch()
    //    {
    //        var initiateUrl = $"https://a.khalti.com/api/v2/epayment/initiate/";
    //        var payload = new
    //        {
    //            return_url = "http://example.com/",
    //            website_url = "https://example.com/",
    //            amount = "1000",
    //            purchase_order_id = "Order01",
    //            purchase_order_name = "test",
    //            customer_info = new
    //            {
    //                name = "Ram Bahadur",
    //                email = "test@khalti.com",
    //                phone = "9800000001"
    //            }
    //        };
    //        var jsonPayload = JsonConvert.SerializeObject(payload);
    //        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

    //        var client = new HttpClient();
    //        client.DefaultRequestHeaders.Add("Authorization", "key 203093b1796b472e9945a9fe74810874_68791341fdd94846a146f0457ff7b455");

    //        var response = await client.PostAsync(initiateUrl, content);
    //        var responseContent = await response.Content.ReadAsStringAsync();

    //        return true;
    //    }
    }
}
