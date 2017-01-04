using System.Collections.Generic;
using ERPDomain.Entities;


namespace ERPDomain.Models
{
    public class JournalLineModel
    {
        public Journal Journal { get; set; }
        public IEnumerable<JournalLine> JournalLine { get; set; }

    }
}
