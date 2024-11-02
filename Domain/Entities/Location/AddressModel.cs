using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Location
{
    public class AddressModel
    {
        public int Id { get; set; }
        public string WardNumber { get; set; }

        public string ToleName { get; set; }

        public CityModel City { get; set; }
    }
}
