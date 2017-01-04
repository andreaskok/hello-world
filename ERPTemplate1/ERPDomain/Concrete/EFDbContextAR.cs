using ERPDomain.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ERPDomain.Concrete
{
    public class EFDbContextAR : DbContext
    {
        public DbSet<ChartOfAccount> ChartOfAccount { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Customer_Dim_Setup> Customer_Dim_Setup { get; set; }
        public DbSet<Customer_Dim_Value> Customer_Dim_Value { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<InvoiceLine> InvoiceLine { get; set; }
        public DbSet<CreditNote> CreditNote { get; set; }
        public DbSet<CreditNoteLine> CreditNoteLine { get; set; }
        public DbSet<DebitNote> DebitNote { get; set; }
        public DbSet<DebitNoteLine> DebitNoteLine { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

    }
}
