using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IBranchRepository
    {
        IEnumerable<Branch> Branch { get; }
        IEnumerable<Branch> GetBranch(String fieldName, Int64 fieldValue);
        IEnumerable<Branch> GetBranchPaging(Int64 startID, Int64 endID);
        IEnumerable<Branch> BranchWildSearch(string fieldName, string fieldValue);
        IEnumerable<Branch> GetBranchByID(Int64 id);
        void SaveBranch(Branch branch);
        void DeleteBranch(Branch branch);
        Int64 BranchCount();
        Int64 GetMaxID();

    }
}
