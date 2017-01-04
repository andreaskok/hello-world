using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Models
{
    public class InvoiceLineModel
    {
        public Invoice Invoice { get; set; }
        public IEnumerable<InvoiceLine> InvoiceLine { get; set; }
        public Customer Customer { get; set; }

    }
}
