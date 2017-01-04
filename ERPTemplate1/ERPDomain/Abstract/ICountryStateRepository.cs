using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface ICountryStateRepository
    {
        IEnumerable<CountryState> CountryState { get; }
        IEnumerable<CountryState> GetCountryState(String fieldName, Int64 fieldValue);
        IEnumerable<CountryState> GetCountryStatePaging(Int64 startID, Int64 endID);
        IEnumerable<CountryState> CountryStateWildSearch(string fieldName, string fieldValue);
        IEnumerable<CountryState> GetCountryStateByID(Int64 id);
        void SaveCountryState(CountryState countrystate);
        void DeleteCountryState(CountryState countrystate);
        Int64 CountryStateCount();
        Int64 GetMaxID();

    }
}
