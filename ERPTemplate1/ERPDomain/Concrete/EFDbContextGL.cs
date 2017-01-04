using ERPDomain.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ERPDomain.Concrete
{
    public class EFDbContextGL : DbContext
    {
        public DbSet<ChartOfAccount> ChartOfAccount { get; set; }
        public DbSet<AccountGroup> AccountGroup { get; set; }
        public DbSet<ActivityGroup> ActivityGroup { get; set; }
        public DbSet<Activity> Activity { get; set; }
        public DbSet<ActivitySub> ActivitySub { get; set; }
        public DbSet<Journal> Journal { get; set; }
        public DbSet<JournalLine> JournalLine { get; set; }
        public DbSet<ChartOfAccount_Dim_Setup> ChartOfAccount_Dim_Setup { get; set; }
        public DbSet<ChartOfAccount_Dim_Value> ChartOfAccount_Dim_Value { get; set; }
        public DbSet<ChartOfAccount_DimensionValue> ChartOfAccount_DimensionValue { get; set; }  //Refer to ChartOfAccount_Dim_Value      
        public DbSet<MonthEndTransaction> MonthEndTransaction { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

    }
}
