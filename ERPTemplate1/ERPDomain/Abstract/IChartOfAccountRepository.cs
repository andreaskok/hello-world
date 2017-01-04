using System.Collections.Generic;
using ERPDomain.Entities;
using System;

namespace ERPDomain.Abstract
{
    public interface IChartOfAccountRepository
    {
        IEnumerable<ChartOfAccount> ChartOfAccount { get; }
        IEnumerable<ChartOfAccount> GetChartOfAccountPaging(Int64 startID, Int64 endID);
        IEnumerable<ChartOfAccount> ChartOfAccountWildSearch(string fieldName, string fieldValue);
        IEnumerable<ChartOfAccount> GetChartOfAccountByID(Int64 id);
        void SaveChartOfAccount(ChartOfAccount chartofaccount);
        void DeleteChartOfAccount(ChartOfAccount chartofaccount);
        long GetMaxID();
    }
}
