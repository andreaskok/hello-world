using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IPaymentLineRepository
    {
        IEnumerable<PaymentLine> PaymentLine { get; }
        IEnumerable<PaymentLine> GetPaymentLine(String fieldName, Int64 fieldValue);
        IEnumerable<PaymentLine> GetPaymentLineByID(Int64 id);
        IEnumerable<PaymentLine> PaymentLineWildSearch(string fieldName, string fieldValue);
        void SavePaymentLine(PaymentLine paymentLine);
        void DeletePaymentLine(PaymentLine paymentLine);
    }
}
