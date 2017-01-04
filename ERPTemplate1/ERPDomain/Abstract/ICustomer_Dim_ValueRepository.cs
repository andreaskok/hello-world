using System.Collections.Generic;
using ERPDomain.Entities;
using System;

namespace ERPDomain.Abstract
{
    public interface ICustomer_Dim_ValueRepository
    {
        IEnumerable<Customer_Dim_Value> Customer_Dim_Value { get; }
        IEnumerable<Customer_Dim_Value> GetCustomer_Dim_ValuePaging(Int64 startID, Int64 endID);
        IEnumerable<Customer_Dim_Value> Customer_Dim_ValueWildSearch(string fieldName, string fieldValue);
        IEnumerable<Customer_Dim_Value> GetCustomer_Dim_ValueByID(Int64 id);
        void SaveCustomer_Dim_Value(Customer_Dim_Value customerdimvalue);
        void DeleteCustomer_Dim_Value(Customer_Dim_Value customerdimvalue);
        long GetMaxID();

    }
}
