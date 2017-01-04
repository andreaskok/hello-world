using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Models
{
    public class SystemSettingModel
    {
        public bool LogDebug { get; set; }
        public bool LogError { get; set; }
        public string ReportServerUrl { get; set; }
        public string Language { get; set; }
        public string Currency { get; set; }
        public string Theme { get; set; }
        public String RowPerPage { get; set; }
        public string DateFormat { get; set; }
        

    }
}
