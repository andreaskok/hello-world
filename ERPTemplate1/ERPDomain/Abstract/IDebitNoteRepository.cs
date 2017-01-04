using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IDebitNoteRepository
    {
        IEnumerable<DebitNote> DebitNote { get; }
        IEnumerable<DebitNote> GetDebitNotePaging(Int64 startID, Int64 endID);
        IEnumerable<DebitNote> DebitNoteWildSearch(string fieldName, string fieldValue);
        IEnumerable<DebitNote> GetDebitNoteByID(Int64 id);
        void SaveDebitNote(DebitNote debitnote);
        void DeleteDebitNote(DebitNote debitnote);
        Int64 DebitNoteCount();
        Int64? GetMaxID();

    }
}
