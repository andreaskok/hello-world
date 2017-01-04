using System.Collections.Generic;
using ERPDomain.Entities;
using System;

namespace ERPDomain.Abstract
{
    public interface IDimension_TableRelationshipRepository
    {
        IEnumerable<Dimension_TableRelationship> Dimension_TableRelationship { get; }
        IEnumerable<Dimension_TableRelationship> GetDimension_TableRelationshipPaging(Int64 startID, Int64 endID);
        IEnumerable<Dimension_TableRelationship> Dimension_TableRelationshipWildSearch(string fieldName, string fieldValue);
        IEnumerable<Dimension_TableRelationship> GetDimension_TableRelationshipByID(Int64 id);
        void SaveDimension_TableRelationship(Dimension_TableRelationship dimensiontablerelationship);
        void DeleteDimension_TableRelationship(Dimension_TableRelationship dimensiontablerelationship);
        long GetMaxID();
    }
}
