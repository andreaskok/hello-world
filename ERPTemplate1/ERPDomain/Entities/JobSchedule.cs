using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class JobSchedule
    {
        [Key]
        public Int64 ID { get; set; }

        public string JobCode { get; set; }
        public string JobName { get; set; }
        public string JobType { get; set; }
        public string Description { get; set; }
        public string ExecuteFrequence { get; set; }
        public DateTime ExecuteDateStart { get; set; }
        public DateTime ExecuteDateEnd { get; set; }
        public DateTime LastRunDateTime { get; set; }
        public DateTime NextRunDateTime { get; set; }
        public int ExecuteFlag { get; set; }
        public bool EnabledFlag { get; set; }

    }
}
