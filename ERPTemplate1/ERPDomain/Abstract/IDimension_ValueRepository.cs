using System.Collections.Generic;
using ERPDomain.Entities;
using System;

namespace ERPDomain.Abstract
{
    public interface IDimension_ValueRepository
    {
        IEnumerable<Dimension_Value> Dimension_Value { get; }
        IEnumerable<Dimension_Value> GetDimension_ValuePaging(Int64 startID, Int64 endID);
        IEnumerable<Dimension_Value> Dimension_ValueWildSearch(string fieldName, string fieldValue);
        IEnumerable<Dimension_Value> GetDimension_ValueByID(Int64 id);
        void SaveDimension_Value(Dimension_Value dimensionvalue);
        void DeleteDimension_Value(Dimension_Value dimensionvalue);
        long GetMaxID();
    }
}
