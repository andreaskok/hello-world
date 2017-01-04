using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Models
{
    public class DynamicMenuModel
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
    }
}
