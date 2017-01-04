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
    public class EFChartOfAccount_Dim_SetupRepository : IChartOfAccount_Dim_SetupRepository
    {
        private EFDbContextGL context = new EFDbContextGL();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextGL"].ConnectionString);

        //public IEnumerable<ChartOfAccount_Dim_Setup> ChartOfAccount_Dim_Setup
        //{
        //    get { return context.ChartOfAccount_Dim_Setup; }
        //}
        public IEnumerable<ChartOfAccount_Dim_Setup> ChartOfAccount_Dim_Setup
        {
            get
            {
                //improve performance
                return cnn.Query<ChartOfAccount_Dim_Setup>("SELECT * FROM ChartOfAccount_Dim_Setup");
            }
        }

        public IEnumerable<ChartOfAccount_Dim_Setup> GetChartOfAccount_Dim_SetupPaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<ChartOfAccount_Dim_Setup>("SELECT * FROM ChartOfAccount_Dim_Setup WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<ChartOfAccount_Dim_Setup> ChartOfAccount_Dim_SetupWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<ChartOfAccount_Dim_Setup>("SELECT * FROM ChartOfAccount_Dim_Setup WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");
        }

        public IEnumerable<ChartOfAccount_Dim_Setup> GetChartOfAccount_Dim_SetupByID(Int64 id)
        {
            //improve performance
            return cnn.Query<ChartOfAccount_Dim_Setup>("SELECT * FROM ChartOfAccount_Dim_Setup WHERE ID = " + id);

        }

        public void SaveChartOfAccount_Dim_Setup(ChartOfAccount_Dim_Setup chartofaccountdimsetup)
        {
            if (chartofaccountdimsetup.ID == 0)
            {
                context.ChartOfAccount_Dim_Setup.Add(chartofaccountdimsetup);
            }
            else
            {
                context.Entry(chartofaccountdimsetup).State = EntityState.Modified;
            }
            context.SaveChanges();
        }


        public void DeleteChartOfAccount_Dim_Setup(ChartOfAccount_Dim_Setup chartofaccountdimsetup)
        {
            context.ChartOfAccount_Dim_Setup.Attach(chartofaccountdimsetup);
            context.ChartOfAccount_Dim_Setup.Remove(chartofaccountdimsetup);
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
            String sSql = "SELECT Max(ID) FROM ChartOfAccount_Dim_Setup";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
