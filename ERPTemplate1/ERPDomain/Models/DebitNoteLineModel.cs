using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Models
{
    public class DebitNoteLineModel
    {
        public DebitNote DebitNote { get; set; }
        public IEnumerable<DebitNoteLine> DebitNoteLine { get; set; }
    }
}
