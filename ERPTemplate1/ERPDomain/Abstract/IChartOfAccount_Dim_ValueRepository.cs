using System.Collections.Generic;
using ERPDomain.Entities;
using System;

namespace ERPDomain.Abstract
{
    public interface IChartOfAccount_Dim_ValueRepository
    {
        IEnumerable<ChartOfAccount_Dim_Value> ChartOfAccount_Dim_Value { get; }
        IEnumerable<ChartOfAccount_Dim_Value> GetChartOfAccount_Dim_ValuePaging(Int64 startID, Int64 endID);
        IEnumerable<ChartOfAccount_Dim_Value> ChartOfAccount_Dim_ValueWildSearch(string fieldName, string fieldValue);
        IEnumerable<ChartOfAccount_Dim_Value> GetChartOfAccount_Dim_ValueByID(Int64 id);
        void SaveChartOfAccount_Dim_Value(ChartOfAccount_Dim_Value chartofaccountdimvalue);
        void DeleteChartOfAccount_Dim_Value(ChartOfAccount_Dim_Value chartofaccountdimvalue);
        long GetMaxID();

    }
}
