using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Task4.Model;

namespace Task4.DAL.Contexts.Configurations
{
    public class ItemConfiguration : EntityTypeConfiguration<Item>
    {
        public ItemConfiguration()
        {
            this.ToTable("Items")
                .HasKey(x => x.Id);
            this.Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasIndex(x => new { x.Name, x.Price });
            this.Property(x => x.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();
            this.Property(x => x.Price)
                .IsRequired();
        }
    }
}
