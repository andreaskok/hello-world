using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IJournalLineRepository
    {
        IEnumerable<JournalLine> JournalLine { get; }
        IEnumerable<JournalLine> GetJournalLine(String fieldName, Int64 fieldValue);
        IEnumerable<JournalLine> GetJournalLineByID(Int64 id);
        IEnumerable<JournalLine> JournalLineWildSearch(string fieldName, string fieldValue);
        void SaveJournalLine(JournalLine journalline);
        void DeleteJournalLine(JournalLine journalline);
    }
}
