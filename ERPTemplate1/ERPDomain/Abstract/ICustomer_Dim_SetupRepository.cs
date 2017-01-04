using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface ICustomer_Dim_SetupRepository
    {
        IEnumerable<Customer_Dim_Setup> Customer_Dim_Setup { get; }
        IEnumerable<Customer_Dim_Setup> GetCustomer_Dim_SetupPaging(Int64 startID, Int64 endID);
        IEnumerable<Customer_Dim_Setup> Customer_Dim_SetupWildSearch(string fieldName, string fieldValue);
        IEnumerable<Customer_Dim_Setup> GetCustomer_Dim_SetupByID(Int64 id);
        void SaveCustomer_Dim_Setup(Customer_Dim_Setup customerdimsetup);
        void DeleteCustomer_Dim_Setup(Customer_Dim_Setup customerdimsetup);
        long GetMaxID();
    }
}
