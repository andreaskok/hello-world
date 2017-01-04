using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PluginAR.Models
{
    public class DebitNoteLineExcelModel
    {
        public Int64? ID { get; set; }
        public Int64 DebitNoteID { get; set; }
        public Int64 ChartOfAccountID { get; set; }
        public string DebitNoteLineCode { get; set; }
        public string DebitNoteCode { get; set; }
        public string AccCode { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Total { get; set; }
        public bool IsNew { get; set; }
        public SelectList ChartOfAccountSelectList { get; set; }
    }
}