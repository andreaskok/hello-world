using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Models
{
    public class CreditNoteLineModel
    {
        public CreditNote CreditNote { get; set; }
        public IEnumerable<CreditNoteLine> CreditNoteLine { get; set; }
        
    }
}
