using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Models
{
    public class RoleAccessModel
    {
        public SH_ROLE SH_ROLE { get; set; }
        public IEnumerable<SH_ROLEACCESS> SH_ROLEACCESSS { get; set; }

        public IEnumerable<SH_APP> SH_APPS { get; set; }
        public IEnumerable<DynamicMenu> DynamicMenus { get; set; }
    }
}
