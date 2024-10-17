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
    public class OrderRepository : GenericRepository<OrderModel>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context)
            : base(context) { }

    }
}
