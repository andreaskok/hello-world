using ERPDomain.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ERPDomain.Concrete
{
    public class EFDbContextAP : DbContext
    {
        public DbSet<ChartOfAccount> ChartOfAccount { get; set; }
        public DbSet<Vendor> Vendor { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<PaymentLine> PaymentLine { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

    }
}
