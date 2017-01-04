using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface ICreditNoteRepository
    {
        IEnumerable<CreditNote> CreditNote { get; }
        IEnumerable<CreditNote> GetCreditNotePaging(Int64 startID, Int64 endID);
        IEnumerable<CreditNote> CreditNoteWildSearch(string fieldName, string fieldValue);
        IEnumerable<CreditNote> GetCreditNoteByID(Int64 id);
        void SaveCreditNote(CreditNote creditnote);
        void DeleteCreditNote(CreditNote creditnote);
        Int64 CreditNoteCount();
        Int64? GetMaxID();

    }
}
