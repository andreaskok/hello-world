using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface ICreditNoteLineRepository
    {
        IEnumerable<CreditNoteLine> CreditNoteLine { get; }
        IEnumerable<CreditNoteLine> GetCreditNoteLinePaging(Int64 startID, Int64 endID);
        IEnumerable<CreditNoteLine> GetCreditNoteLine(String fieldName, Int64 fieldValue);
        IEnumerable<CreditNoteLine> CreditNoteLineWildSearch(string fieldName, string fieldValue);
        IEnumerable<CreditNoteLine> GetCreditNoteLineByID(Int64 id);
        void SaveCreditNoteLine(CreditNoteLine creditnoteline);
        void DeleteCreditNoteLine(CreditNoteLine creditnoteline);
        Int64 CreditNoteLineCount();
        Int64 GetMaxID();
    }
}
