using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERPDomain.Entities
{
    public class PluginArea
    {
        public int ID { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public bool Buy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
