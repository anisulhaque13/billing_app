using Microsoft.EntityFrameworkCore;
using SimpleBilling.Models.Domain;

namespace SimpleBilling.Data
{
    public class BillingDBContext : DbContext
    {
        public BillingDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories{ get; set; } 
        public DbSet<Item> Items{ get; set; }
    }
}
