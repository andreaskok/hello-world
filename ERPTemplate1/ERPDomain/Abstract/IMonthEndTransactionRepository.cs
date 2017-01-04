using System.Collections.Generic;
using ERPDomain.Entities;
using System;

namespace ERPDomain.Abstract
{
    public interface IMonthEndTransactionRepository
    {
        IEnumerable<MonthEndTransaction> MonthEndTransaction { get; }
        IEnumerable<MonthEndTransaction> GetMonthEndTransactionPaging(Int64 startID, Int64 endID);
        IEnumerable<MonthEndTransaction> MonthEndTransactionWildSearch(string fieldName, string fieldValue);
        IEnumerable<MonthEndTransaction> GetMonthEndTransactionByID(Int64 id);
        void SaveMonthEndTransaction(MonthEndTransaction monthendtransaction);
        void DeleteMonthEndTransaction(MonthEndTransaction monthendtransaction);
        long GetMaxID();
    }
}
