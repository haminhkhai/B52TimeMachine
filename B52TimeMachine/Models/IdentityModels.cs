using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace B52TimeMachine.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Ps> Pss { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<ServiceRental> ServiceRentals { get; set; }
        public DbSet<SplitTime> SplitTimes { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Restock> Restocks { get; set; }
        public DbSet<RestockService> RestockServices { get; set; }
        public DbSet<CheckQuantity> CheckQuantities { get; set; }
        public DbSet<CheckQuantityDetail> CheckQuantityDetails { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}