using System;
using System.Collections.Generic;
using ERPDomain.Entities;
using ERPDomain.Models;

namespace ERPDomain.Abstract
{
    public interface IJournalRepository
    {
        IEnumerable<Journal> Journal { get; }

        IEnumerable<Journal> GetJournalPaging(Int64 startID, Int64 endID);
        IEnumerable<Journal> JournalWildSearch(string fieldName, string fieldValue);
        IEnumerable<Journal> GetJournalByID(Int64 id);

        IEnumerable<MonthEndPostingModel> GetJournalMonthEndPostingModel(Int64 journalID);
        void SaveJournal(Journal journal);
        void DeleteJournal(Journal journal);
        Int64 JournalCount();
        Int64? GetMaxID();
    }
}
