using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IDebitNoteLineRepository
    {
        IEnumerable<DebitNoteLine> DebitNoteLine { get; }
        IEnumerable<DebitNoteLine> GetDebitNoteLinePaging(Int64 startID, Int64 endID);
        IEnumerable<DebitNoteLine> GetDebitNoteLine(String fieldName, Int64 fieldValue);
        IEnumerable<DebitNoteLine> DebitNoteLineWildSearch(string fieldName, string fieldValue);
        IEnumerable<DebitNoteLine> GetDebitNoteLineByID(Int64 id);
        void SaveDebitNoteLine(DebitNoteLine debitnoteline);
        void DeleteDebitNoteLine(DebitNoteLine debitnoteline);
        void DeleteDebitNoteLineExcel(DebitNoteLine debitnoteline);
        Int64 DebitNoteLineCount();
        Int64 GetMaxID();
    }
}
