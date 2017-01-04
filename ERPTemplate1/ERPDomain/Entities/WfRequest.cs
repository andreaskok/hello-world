using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfRequest
    {
        public long ID { get; set; }
        public int WfProcessID { get; set; } //Foreign key to parent table WfProcess
        public string Title { get; set; }

        public DateTime DateRequested { get; set; }
        public int SH_USERID { get; set; }//Foreign key to parent table SH_USER

        public int CurrentStateID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual WfProcess WfProcess { get; set; } //Navigation property to parent table WfProcess
        public virtual SH_USER SH_USER { get; set; } //Navigation property to parent table SH_USER

        public virtual ICollection<WfRequestData> WfRequestData { get; set; } //Navigation property to child table WfRequestData 
        public virtual ICollection<WfRequestNote> WfRequestNote {get;set;} //Navigation property to child table WfRequestNote
        public virtual ICollection<WfRequestStakeholder> WfRequestStakeholer { get; set; } //Navigation property to child table WfRequestStakeholer
        public virtual ICollection<WfRequestFile> WfRequestFile { get; set; } // Navigation property to child table WfRequestFile
        public virtual ICollection<WfRequestAction> WfRequestAction { get; set; } //Navigation property to child table WfRequestAction
    }
}
