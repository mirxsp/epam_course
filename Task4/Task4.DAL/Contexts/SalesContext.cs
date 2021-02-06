using System.Data.Entity;
using Task4.Model;

namespace Task4.DAL.Contexts
{
    public class SalesContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Sale> Sales { get; set; }

        public SalesContext() : base("name=salesdb")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Configurations.ClientConfiguration());
            modelBuilder.Configurations.Add(new Configurations.ItemConfiguration());
            modelBuilder.Configurations.Add(new Configurations.SaleConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
