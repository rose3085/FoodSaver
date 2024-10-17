using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.Food
{
    public class OrderResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
    }
}
