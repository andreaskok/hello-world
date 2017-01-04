using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Models
{
    public class PaymentLineModel
    {
        public Payment Payment { get; set; }
        public IEnumerable<PaymentLine> PaymentLine { get; set; }
        public Vendor Vendor { get; set; }

    }
}
