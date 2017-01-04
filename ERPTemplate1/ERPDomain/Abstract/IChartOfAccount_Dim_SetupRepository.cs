using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IChartOfAccount_Dim_SetupRepository
    {
        IEnumerable<ChartOfAccount_Dim_Setup> ChartOfAccount_Dim_Setup { get; }
        IEnumerable<ChartOfAccount_Dim_Setup> GetChartOfAccount_Dim_SetupPaging(Int64 startID, Int64 endID);
        IEnumerable<ChartOfAccount_Dim_Setup> ChartOfAccount_Dim_SetupWildSearch(string fieldName, string fieldValue);
        IEnumerable<ChartOfAccount_Dim_Setup> GetChartOfAccount_Dim_SetupByID(Int64 id);
        void SaveChartOfAccount_Dim_Setup(ChartOfAccount_Dim_Setup chartofaccountdimsetup);
        void DeleteChartOfAccount_Dim_Setup(ChartOfAccount_Dim_Setup chartofaccountdimsetup);
        long GetMaxID();
    }
}
