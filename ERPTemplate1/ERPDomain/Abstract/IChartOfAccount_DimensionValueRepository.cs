using System.Collections.Generic;
using ERPDomain.Entities;
using System;

namespace ERPDomain.Abstract
{
    public interface IChartOfAccount_DimensionValueRepository
    {
        IEnumerable<ChartOfAccount_DimensionValue> ChartOfAccount_DimensionValue { get; }
        IEnumerable<ChartOfAccount_DimensionValue> GetChartOfAccount_DimensionValue(string fieldName, Int64 fieldValue);
        IEnumerable<ChartOfAccount_DimensionValue> ChartOfAccount_DimensionValueWildSearch(string fieldName, string fieldValue);
        IEnumerable<ChartOfAccount_DimensionValue> GetChartOfAccount_DimensionValueByID(Int64 id);
        void SaveChartOfAccount_DimensionValue(ChartOfAccount_DimensionValue chartofaccount_dimensionvalue);
        void DeleteChartOfAccount_DimensionValue(ChartOfAccount_DimensionValue chartofaccount_dimensionvalue);
    }
}
