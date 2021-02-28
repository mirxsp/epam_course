using System.Data.Entity;
using WEB.Models;

namespace WEB.DAL.Contexts
{
    public class SalesContext : DbContext
    {
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }

        public SalesContext() : base("name=salesdb")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Sales)
                .WithRequired(e => e.Client)
                .HasForeignKey(e => e.Client_Id);

            modelBuilder.Entity<Item>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.Sales)
                .WithRequired(e => e.Item)
                .HasForeignKey(e => e.Item_Id);

            modelBuilder.Entity<Sale>()
                .Property(e => e.Date)
                .HasColumnType("datetime2");

            modelBuilder.Entity<Manager>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Manager>()
                .HasMany(e => e.Sales)
                .WithRequired(e => e.Manager)
                .HasForeignKey(e => e.Manager_Id);

            modelBuilder.Entity<Client>()
                .Property(e => e.RowVersion)
                .IsRowVersion();

            modelBuilder.Entity<Manager>()
                .Property(e => e.RowVersion)
                .IsRowVersion();

            modelBuilder.Entity<Sale>()
                .Property(e => e.RowVersion)
                .IsRowVersion();

            modelBuilder.Entity<Item>()
                .Property(e => e.RowVersion)
                .IsRowVersion();

            base.OnModelCreating(modelBuilder);
        }
    }
}
