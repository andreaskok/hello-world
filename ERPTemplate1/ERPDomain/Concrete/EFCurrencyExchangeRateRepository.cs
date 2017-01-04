using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using ERPDomain.Abstract;
using ERPDomain.Entities;
using Dapper;

namespace ERPDomain.Concrete
{
    public class EFCurrencyExchangeRateRepository : ICurrencyExchangeRateRepository
    {
        private EFDbContext context = new EFDbContext();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContext"].ConnectionString);
        public IEnumerable<CurrencyExchangeRate> CurrencyExchangeRate
        {
            get
            {
                //improve performance
                return cnn.Query<CurrencyExchangeRate>("SELECT * FROM CurrencyExchangeRate");
            }
        }
        public IEnumerable<CurrencyExchangeRate> GetCurrencyExchangeRatePaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<CurrencyExchangeRate>("SELECT * FROM CurrencyExchangeRate WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<CurrencyExchangeRate> CurrencyExchangeRateWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<CurrencyExchangeRate>("SELECT * FROM CurrencyExchangeRate WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<CurrencyExchangeRate> GetCurrencyExchangeRateByID(Int64 id)
        {
            //improve performance
            return cnn.Query<CurrencyExchangeRate>("SELECT * FROM CurrencyExchangeRate WHERE ID = " + id);

        }

        public void SaveCurrencyExchangeRate(CurrencyExchangeRate currencyexchangerate)
        {
            if (currencyexchangerate.ID == 0)
            {
                context.CurrencyExchangeRate.Add(currencyexchangerate);
            }
            else
            {
                context.Entry(currencyexchangerate).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteCurrencyExchangeRate(CurrencyExchangeRate currencyexchangerate)
        {
            context.CurrencyExchangeRate.Attach(currencyexchangerate);
            context.CurrencyExchangeRate.Remove(currencyexchangerate);
            //context.SaveChanges();
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store 
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
            //https://msdn.microsoft.com/en-us/data/jj592904, fixed concurrency exception

        }

        public Int64 CurrencyExchangeRateCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM CurrencyExchangeRate";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64 GetMaxID()
        {
            Int64 iMax = 0;
            String sSql = "SELECT Max(ID) FROM CurrencyExchangeRate";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
