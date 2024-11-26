using Domain.Entities.Foods;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Location
{
    public class AddressModel
    {
        public string Id { get; set; }
        public string WardNumber { get; set; }

        public string ToleName { get; set; }

        public string CityName { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string FoodId { get; set; }
        public FoodModel Food { get; set; }
    }
}
