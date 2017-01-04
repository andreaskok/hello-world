using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IInvoiceRepository
    {
        IEnumerable<Invoice> Invoice { get; }
        IEnumerable<Invoice> GetInvoicePaging(Int64 startID, Int64 endID);
        IEnumerable<Invoice> InvoiceWildSearch(string fieldName, string fieldValue);
        IEnumerable<Invoice> GetInvoiceByID(Int64 id);
        void SaveInvoice(Invoice invoice);
        void DeleteInvoice(Invoice invoice);
        Int64 InvoiceCount();
        Int64? GetMaxID();


    }
}
