using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Task4.Model;

namespace Task4.DAL.Contexts.Configurations
{
    public class ClientConfiguration : EntityTypeConfiguration<Client>
    {
        public ClientConfiguration()
        {
            this.ToTable("Clients")
                .HasKey(x => x.Id);
            this.Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasIndex(x => x.Name)
                .IsUnique();
            this.Property(x => x.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
