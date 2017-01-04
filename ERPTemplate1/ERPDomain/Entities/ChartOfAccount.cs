using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class ChartOfAccount
    {
        [Key]
        public Int64 ID { get; set; }

        public Int64 OrganizationID { get; set; } //Foreign Key
        public Int64 AccountGroupID { get; set; } //Foreign Key
        public Int64 ActivityID { get; set; } //Foreign Key
        public Int64 ActivitySubID { get; set; } //Foreign Key

        public string AccCode { get; set; }
        public string Description { get; set; }
        public string AccType { get; set; }
        public string AccGrpCode { get; set; }
        public string ActCode { get; set; }
        public string SubActCode { get; set; }
        public string ExpenseCode { get; set; }
        public string AccPurpose { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateID { get; set; }
        public string FinAccCode { get; set; }
        public string WSAccDistUse { get; set; }
        public string ControlAcc { get; set; }
        public string SuspendAcc { get; set; }
        public string Blocked { get; set; }
        public DateTime EffDateFrom { get; set; }
        public DateTime EffDateTo { get; set; }
        public string ReportDESC { get; set; }
        public string SubAccType { get; set; }

        //Foreign Table
        //public virtual ICollection<Organization> Organization { get; set; }
        //public virtual ICollection <AccountGroup> AccountGroup { get; set; }
        public virtual AccountGroup AccountGroup { get; set; } //Navigation Property to parent table AccountGroup
        //public virtual ICollection<ChartOfAccount_DimensionValue> ChartOfAccount_DimensionValue { get; set; }
        public virtual ICollection<Dimension_TableRelationship> Dimension_TableRelationship { get; set; }

        //public virtual Activity Activity { get; set; } //Navigation Property to parent table Activity
        //public virtual ActivitySub ActivitySub { get; set; } //Navigation Property to parent table ActivitySub
        //public virtual ICollection<JournalLine> JournalLine { get; set; }
        //public virtual ICollection<Journal> Journal { get; set; }
        //public virtual JournalLine JournalLine { get; set; }
        //public virtual ICollection<Activity> Activity { get; set; }
        //public virtual ICollection<ActivitySub> ActivitySub { get; set; }

    }
}
