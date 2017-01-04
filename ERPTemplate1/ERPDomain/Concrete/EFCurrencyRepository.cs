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
    public class EFCurrencyRepository : ICurrencyRepository
    {
        private EFDbContext context = new EFDbContext();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContext"].ConnectionString);
        public IEnumerable<Currency> Currency
        {
            get
            {
                //improve performance
                return cnn.Query<Currency>("SELECT * FROM Currency");
            }
        }
        public IEnumerable<Currency> GetCurrencyPaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<Currency>("SELECT * FROM Currency WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<Currency> CurrencyWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<Currency>("SELECT * FROM Currency WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<Currency> GetCurrencyByID(Int64 id)
        {
            //improve performance
            return cnn.Query<Currency>("SELECT * FROM Currency WHERE ID = " + id);

        }

        public void SaveCurrency(Currency currency)
        {
            if (currency.ID == 0)
            {
                context.Currency.Add(currency);
            }
            else
            {
                context.Entry(currency).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteCurrency(Currency currency)
        {
            context.Currency.Attach(currency);
            context.Currency.Remove(currency);
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

        public Int64 CurrencyCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM Currency";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64 GetMaxID()
        {
            Int64 iMax = 0;
            String sSql = "SELECT Max(ID) FROM Currency";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
