using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class DynamicMenu
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public int MenuLevel { get; set; }
        public string MenuName { get; set; }
        public string ControllerName { get; set; }
        public string MethodName { get; set; }
        public string AreaName { get; set; }
        public string PluginName { get; set; }
        public string ResourceName { get; set; }
        public bool Plugin { get; set; }

        public bool Buy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual ICollection<SH_ROLEACCESS> SH_ROLEACCEESS { get; set; }
    }
}
