using CathayBkWebAPI.Model;
using Microsoft.EntityFrameworkCore;


namespace CathayBkWebAPI.DataAccess
{
    public class MyDbcontext : DbContext
    {
        public MyDbcontext(DbContextOptions<MyDbcontext> options) : base(options)
        {

        }

        public DbSet<BTCPrice> BTCPrices { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
