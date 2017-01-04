using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface ICurrencyRepository
    {
        IEnumerable<Currency> Currency { get; }
        IEnumerable<Currency> GetCurrencyPaging(Int64 startID, Int64 endID);
        IEnumerable<Currency> CurrencyWildSearch(string fieldName, string fieldValue);
        IEnumerable<Currency> GetCurrencyByID(Int64 id);
        void SaveCurrency(Currency currency);
        void DeleteCurrency(Currency currency);
        Int64 CurrencyCount();
        Int64 GetMaxID();

    }
}
