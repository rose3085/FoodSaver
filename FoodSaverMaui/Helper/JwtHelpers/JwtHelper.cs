using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Helper
{
    public class JwtHelper : IJwtHelper
    {
        public string ExtractUserInfo(string jwtToken)
        {
            var parts = jwtToken.Split('.');
            if (parts.Length != 3)
            {
                throw new ArgumentException("Invalid JWT token format.");
            }

            var payload = parts[1];
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            var jsonBytes = Convert.FromBase64String(payload);
            var jsonPayload = Encoding.UTF8.GetString(jsonBytes);
            var payloadData = JObject.Parse(jsonPayload);

            //var username = payloadData["username"]?.ToString();
            //var email = payloadData["email"]?.ToString();
            var username = payloadData["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]?.ToString();

            return username;
        }
    }
}
