using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface ICurrencyExchangeRateRepository
    {
        IEnumerable<CurrencyExchangeRate> CurrencyExchangeRate { get; }
        IEnumerable<CurrencyExchangeRate> GetCurrencyExchangeRatePaging(Int64 startID, Int64 endID);
        IEnumerable<CurrencyExchangeRate> CurrencyExchangeRateWildSearch(string fieldName, string fieldValue);
        IEnumerable<CurrencyExchangeRate> GetCurrencyExchangeRateByID(Int64 id);
        void SaveCurrencyExchangeRate(CurrencyExchangeRate currencyexchangerate);
        void DeleteCurrencyExchangeRate(CurrencyExchangeRate currencyexchangerate);
        Int64 CurrencyExchangeRateCount();
        Int64 GetMaxID();

    }
}
