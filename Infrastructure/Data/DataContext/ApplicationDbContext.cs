using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.User;
using Domain.Entities.Foods;
using Domain.Entities.Location;
using Domain.Entities.Payment;
using Domain.Entities.SalesRecord;


namespace Infrastructure.Data.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role,string>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<FoodModel> FoodModel { get; set; }
       // public DbSet<ApplicationUserFoodModel> ApplicationUserFoodModel { get; set; }
       public DbSet<OrderModel> Orders { get; set; }
        public DbSet<AddressModel> Address { get; set; }
        public DbSet<PaymentModel> Payment { get; set; }
        public DbSet<SalesRecordModel> SalesRecord { get; set; }
        public DbSet<DeliveryAddressModel> DeliveryAddress { get; set; }
    }
}





