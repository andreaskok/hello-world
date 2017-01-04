using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface ICountryRepository
    {
        IEnumerable<Country> Country { get; }
        IEnumerable<Country> GetCountryPaging(Int64 startID, Int64 endID);
        IEnumerable<Country> CountryWildSearch(string fieldName, string fieldValue);
        IEnumerable<Country> GetCountryByID(Int64 id);
        void SaveCountry(Country country);
        void DeleteCountry(Country country);
        Int64 CountryCount();
        Int64 GetMaxID();

    }
}
