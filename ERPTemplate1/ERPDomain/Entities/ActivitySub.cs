using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class ActivitySub
    {
        [Key]
        public Int64 ID { get; set; }

        public Int64 ActivityID { get; set; } //Foreign Key

        public string SubActCode { get; set; }
        public string Description { get; set; }
        public string ActCode { get; set; }
        public string UOMCode { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateID { get; set; }
        public string SubActId { get; set; }

        //Foreign Table
        public virtual Activity Activity { get; set; } //Navigation Property to parent table Activity
        //public virtual ICollection<Activity> Activity { get; set; } 

    }
}
