using Application.Interfaces.Data;
using Domain.Entities.SalesRecord;
using Infrastructure.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repository
{
    public class SellerLimitRepository : GenericRepository<SalesRecordModel>, ISellerLimitRepository
    {
        public SellerLimitRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
