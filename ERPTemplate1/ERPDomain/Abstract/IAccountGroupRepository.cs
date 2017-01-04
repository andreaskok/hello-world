using System.Collections.Generic;
using ERPDomain.Entities;
using System;

namespace ERPDomain.Abstract
{
    public interface IAccountGroupRepository
    {
        IEnumerable<AccountGroup> AccountGroup { get; }
        IEnumerable<AccountGroup> GetAccountGroupPaging(Int64 startID, Int64 endID);
        IEnumerable<AccountGroup> AccountGroupWildSearch(string fieldName, string fieldValue);
        IEnumerable<AccountGroup> GetAccountGroupByID(Int64 id);
        void SaveAccountGroup(AccountGroup accountgroup);
        void DeleteAccountGroup(AccountGroup accountgroup);
        Int64 AccountGroupCount();
        Int64? GetMaxID();

    }
}
