using System;
using System.Collections.Generic;
using System.Data.Entity;
using ERPDomain.Abstract;
using ERPDomain.Entities;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ERPDomain.Concrete
{
    public class EFMonthEndTransactionRepository : IMonthEndTransactionRepository
    {
        private EFDbContextGL context = new EFDbContextGL();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextGL"].ConnectionString);

        public IEnumerable<MonthEndTransaction> MonthEndTransaction
        {
            get { return context.MonthEndTransaction; }
        }

        public IEnumerable<MonthEndTransaction> GetMonthEndTransactionPaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<MonthEndTransaction>("SELECT * FROM MonthEndTransaction WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<MonthEndTransaction> MonthEndTransactionWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<MonthEndTransaction>("SELECT * FROM MonthEndTransaction WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");
        }

        public IEnumerable<MonthEndTransaction> GetMonthEndTransactionByID(Int64 id)
        {
            //improve performance
            return cnn.Query<MonthEndTransaction>("SELECT * FROM MonthEndTransaction WHERE ID = " + id);

        }

        public void SaveMonthEndTransaction(MonthEndTransaction  monthendtransaction)
        {
            if (monthendtransaction.ID == 0)
            {
                context.MonthEndTransaction.Add(monthendtransaction);
            }
            else
            {
                context.Entry(monthendtransaction).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteMonthEndTransaction(MonthEndTransaction monthendtransaction)
        {
            context.MonthEndTransaction.Attach(monthendtransaction);
            context.MonthEndTransaction.Remove(monthendtransaction);
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

        public Int64 GetMaxID()
        {
            Int64 iMax = 0;
            String sSql = "SELECT Max(ID) FROM MonthEndTransaction";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }
    }
}
