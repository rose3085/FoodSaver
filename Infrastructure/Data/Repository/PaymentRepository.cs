using Application.Interfaces.Data;
using Domain.Entities.Payment;
using Infrastructure.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repository
{
    public class PaymentRepository : GenericRepository<PaymentModel>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context)
            : base(context) { }
    }
}
