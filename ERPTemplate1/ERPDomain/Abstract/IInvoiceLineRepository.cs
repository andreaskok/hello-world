using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IInvoiceLineRepository
    {
        IEnumerable<InvoiceLine> InvoiceLine { get; }
        IEnumerable<InvoiceLine> GetInvoiceLine(String fieldName, Int64 fieldValue);
        IEnumerable<InvoiceLine> InvoiceLineWildSearch(string fieldName, string fieldValue);
        IEnumerable<InvoiceLine> GetInvoiceLineByID(Int64 id);
        void SaveInvoiceLine(InvoiceLine invoiceline);
        void DeleteInvoiceLine(InvoiceLine invoiceline);

    }
}
