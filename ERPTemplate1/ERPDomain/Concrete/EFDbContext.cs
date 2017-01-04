
using ERPDomain.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ERPDomain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Employee> Employee { get; set; }
        public DbSet<ParentMenu> ParentMenu { get; set; }
        public DbSet<SubMenu> SubMenu { get; set; }
        public DbSet<SH_USER> SH_USER { get; set; }
        public DbSet<SH_APP> SH_APP { get; set; }
        public DbSet<SH_ROLE> SH_ROLE { get; set; }
        public DbSet<SH_ROLEMEMBER> SH_ROLEMEMBER { get; set; }
        public DbSet<SH_ROLEACCESS> SH_ROLEACCESS { get; set; }
        public DbSet<PluginArea> PluginArea { get; set; }
        public DbSet<DimensionSetup> DimensionSetup { get; set; } // Refer to dimension setting
        public DbSet<Dimension_Setting> Dimension_Setting { get; set; }
        public DbSet<Dimension_Value> Dimension_Value { get; set; }
        public DbSet<Dimension_TableRelationship> Dimension_TableRelationship { get; set; }
        public DbSet<DebugLog> DebugLog { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }
        public DbSet<SH_USERROLE> SH_USERROLE { get; set; }

        public DbSet<DynamicMenu> DynamicMenu { get; set; }
        public DbSet<Organization> Organization { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<CountryState> CountryState { get; set; }
        public DbSet<Calendar> Calendar { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<CurrencyExchangeRate> CurrencyExchangeRate { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<UserPreference> UserPreference { get; set; }
        
        public DbSet<JobSchedule> JobSchedule { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Entity<Payment>().Property(x => x.TotalAmount).HasPrecision(28, 6);
            //modelBuilder.Entity<PaymentLine>().Property(x => x.Amount).HasPrecision(28, 6);
        }

        public System.Data.Entity.DbSet<ERPDomain.Entities.ChartOfAccount> ChartOfAccounts { get; set; }
    }
}
