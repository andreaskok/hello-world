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
    public class EFChartOfAccountRepository : IChartOfAccountRepository
    {
        private EFDbContextGL context = new EFDbContextGL();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextGL"].ConnectionString);

        public IEnumerable<ChartOfAccount> ChartOfAccount
        {
            get { return context.ChartOfAccount; }
        }
        //public IEnumerable<ChartOfAccount> ChartOfAccount
        //{
        //    get
        //    {
        //        //improve performance
        //        return cnn.Query<ChartOfAccount>("SELECT * FROM ChartOfAccount WHERE id = 0");
        //    }
        //}

        public IEnumerable<ChartOfAccount> GetChartOfAccountPaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<ChartOfAccount>("SELECT * FROM ChartOfAccount WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<ChartOfAccount> ChartOfAccountWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<ChartOfAccount>("SELECT * FROM ChartOfAccount WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");
        }

        public IEnumerable<ChartOfAccount> GetChartOfAccountByID(Int64 id)
        {
            //improve performance
            return cnn.Query<ChartOfAccount>("SELECT * FROM ChartOfAccount WHERE ID = " + id);

        }

        public void SaveChartOfAccount(ChartOfAccount chartofaccount)
        {
            if (chartofaccount.ID == 0)
            {
                context.ChartOfAccount.Add(chartofaccount);
            }
            else
            {
                context.Entry(chartofaccount).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        //public void DeleteChartOfAccount(ChartOfAccount chartofaccount)
        //{
        //    context.ChartOfAccount.Remove(chartofaccount);
        //    context.SaveChanges();
        //}

        public void DeleteChartOfAccount(ChartOfAccount chartofaccount)
        {
            context.ChartOfAccount.Attach(chartofaccount);
            context.ChartOfAccount.Remove(chartofaccount);
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
            String sSql = "SELECT Max(ID) FROM ChartOfAccount";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }


    }
}
