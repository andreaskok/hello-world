using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class Activity
    {
        [Key]
        public Int64 ID { get; set; }

        public Int64 ActivityGroupID { get; set; } //Foreign Key 

        public string ActCode { get; set; }
        public string Description { get; set; }
        public string ActGrpCode { get; set; }
        public string UOMCode { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateID { get; set; }
        public string LabOverheadInd { get; set; }
        public string Head { get; set; }
        public string SHead { get; set; }

        //Foreign Table
        //public virtual ICollection<ActivityGroup> ActivityGroup { get; set; }
        public virtual ActivityGroup ActivityGroup { get; set; } //Navigation Property to parent table ActivityGroup
        //public virtual ICollection<ActivitySub> ActivitySub { get; set; } //Navigation Property to child table ActivitySub

    }
}
