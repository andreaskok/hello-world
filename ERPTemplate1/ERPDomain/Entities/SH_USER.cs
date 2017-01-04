using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERPDomain.Entities
{
    public class SH_USER
    {
        [Key]
        public int ID { get; set; }

        public int SH_ROLEID { get; set; } //Foreign Key to SH_ROLE
        public string UserID { get; set; }

        [DataType(DataType.Password)]
        public string UserPwd { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string UserEmail { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual SH_ROLE SH_ROLE { get; set; } //Navigation Property to parent table SH_ROLE
        public virtual ICollection<SH_ROLEMEMBER> SH_ROLEMEMBER { get; set; }
        public virtual ICollection<SH_USERROLE> SH_USERROLE { get; set; }

        //public virtual ICollection<UserPreference> UserPreference { get; set; }

        public virtual ICollection<WfProcessAdmin> WfProcessAdmin { get; set; } //Navigation property to child table WfProcessAdmin
        public virtual ICollection<WfGroupMember> WfGroupMemner { get; set; }//Navigation property to child table WfGroupMember
        public virtual ICollection<WfRequest> WfRequest { get; set; } //Navigation property to child table WfRequest
        public virtual ICollection<WfRequestNote> WfRequestNote { get; set; } //Navigation property to child table WfRequestMNote
        public virtual ICollection<WfRequestStakeholder> WfRequestStakeholder { get; set; } //Navigation property to child table WfRequestStakeholder
        public virtual ICollection<WfRequestFile> WfRequestFile { get; set; } //Navigation property to child table WfRequestFile
        public virtual ICollection<WfEscalation> WfEscalation { get; set; } //Navigation property to child table WfEscalation

    }
}
