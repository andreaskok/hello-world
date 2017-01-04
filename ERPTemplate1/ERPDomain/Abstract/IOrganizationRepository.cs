using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IOrganizationRepository
    {
        IEnumerable<Organization> Organization { get; }
        IEnumerable<Organization> GetOrganizationPaging(Int64 startID, Int64 endID);
        IEnumerable<Organization> OrganizationWildSearch(string fieldName, string fieldValue);
        IEnumerable<Organization> GetOrganizationByID(Int64 id);
        void SaveOrganization(Organization organization);
        void DeleteOrganization(Organization organization);
        Int64 OrganizationCount();
        Int64 GetMaxID();

    }
}
