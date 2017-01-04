using System.Collections.Generic;
using ERPDomain.Entities;
using System;

namespace ERPDomain.Abstract
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> Customer { get; }
        IEnumerable<Customer> CustomerWildSearch(string fieldName, string fieldValue);
        IEnumerable<Customer> GetCustomerByID(Int64 id);
        void SaveCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
    }
}
