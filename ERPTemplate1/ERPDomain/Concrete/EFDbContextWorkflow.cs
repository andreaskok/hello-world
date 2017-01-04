using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Concrete
{
    public class EFDbContextWorkflow : DbContext
    {
        public DbSet<WfProcess> WfProcess { get; set; }
        public DbSet<WfProcessAdmin> WfProcessAdmin { get; set; }
        public DbSet<WfGroupMember> WfGroupMember { get; set; }
        public DbSet<WfGroup> WfGroup { get; set; }
        public DbSet<WfRequest> WfRequest { get; set; }
        public DbSet<WfRequestData> WfRequestData { get; set; }
        public DbSet<WfRequestNote> WfRequestNote { get; set; }
        public DbSet<WfRequestFile> WfRequestFile { get; set; }
        public DbSet<WfRequestStakeholder> WfRequestStakeholder { get; set; }
        public DbSet<WfState> WfState { get; set; }
        public DbSet<WfTransition> WfTransition { get; set; }
        public DbSet<WfStateType> WfStateType { get; set; }
        public DbSet<WfStateActivity> WfStateActivity { get; set; }
        public DbSet<WfTransitionAction> WfTransitionAction { get; set; }
        public DbSet<WfActionType> WfActionType { get; set; }
        public DbSet<WfAction> WfAction { get; set; }
        public DbSet<WfActionTarget> WfActionTarget { get; set; }
        public DbSet<WfActivityType> WfActivityType { get; set; }
        public DbSet<WfActivity> WfActivity { get; set; }
        public DbSet<WfTransitionActivity> WfTransitionActivity { get; set; }

        public DbSet<WfActivityTarget> WfActivityTarget { get; set; }
        public DbSet<WfTarget> WfTarget { get; set; }
        public DbSet<WfRequestAction> WfRequestAction { get; set; }
        public DbSet<WfEscalation> WfEscalation { get; set; }
        public DbSet<DfMaster> DfMaster { get; set; }
        public DbSet<DfMasterData> DfMasterData { get; set; }
        public DbSet<DfRequest> DfRequest { get; set; }
        public DbSet<DfRequestData> DfRequestData { get; set; }
        public DbSet<DfExemptItem> DfExemptItem { get; set; }
        public DbSet<DfAppliedModule> DfAppliedModule { get; set; }
        public DbSet<DfItemType> DfItemType { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            
        }
    }
}
