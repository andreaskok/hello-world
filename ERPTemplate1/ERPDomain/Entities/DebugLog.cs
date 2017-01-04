using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class DebugLog
    {
        [Key]
        public Int64 ID { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public String JsonLog { get; set; }
        public string UserID { get; set; }
        public string PluginName { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
