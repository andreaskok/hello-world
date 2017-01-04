using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IPaymentRepository
    {
        IEnumerable<Payment> Payment { get; }
        IEnumerable<Payment> GetPaymentPaging(Int64 startID, Int64 endID);
        IEnumerable<Payment> PaymentWildSearch(string fieldName, string fieldValue);
        IEnumerable<Payment> GetPaymentByID(Int64 id);
        void SavePayment(Payment payment);
        void DeletePayment(Payment payment);

        Int64 PaymentCount();
        Int64? GetMaxID();

    }
}
