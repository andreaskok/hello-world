using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class SH_ROLEACCESS
    {
        public int ID { get; set; }
        public int SH_ROLEID { get; set; } //Foreign Key to SH_ROLE
        public int SH_APPID { get; set; } //Foreign Key to SH_APP
        public int DynamicMenuID { get; set; }
        public bool IsChecker { get; set; }
        public bool AllowRead { get; set; }
        public bool AllowInsert { get; set; }
        public bool AllowUpdate { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowPrint { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual SH_ROLE SH_ROLE { get; set; } //Navigation Property to parent table SH_ROLE
        public virtual SH_APP SH_APP { get; set; } // Navigation Property to parent table SH_APP
        public virtual DynamicMenu DynamicMenu { get; set; } // Navigation Property to parent table DynamicMenu
    }
}
