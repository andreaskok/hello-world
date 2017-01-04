using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class ParentMenu
    {
        public Int64 ID { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string ControllerName { get; set; }
        public string MethodName { get; set; }
        public bool Plugin { get; set; }
        public bool Buy { get; set; }
    }
}
