using Application.Interfaces.Data;
using Domain.Entities.Foods;
using Infrastructure.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repository
{
    public class ProductRepository : GenericRepository<FoodModel>, IFoodRepository
    {
        public ProductRepository(ApplicationDbContext context)
            : base(context) { }
       
    }
}
