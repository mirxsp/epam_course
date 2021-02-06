using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Task4.Model;

namespace Task4.DAL.Contexts.Configurations
{
    public class SaleConfiguration : EntityTypeConfiguration<Sale>
    {
        public SaleConfiguration()
        {
            this.ToTable("Sales")
                .HasKey(x => x.Id);
            this.Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasRequired(x => x.Client);
            this.HasRequired(x => x.Item);
            this.Property(x => x.Date)
                .IsRequired();
        }
    }
}
