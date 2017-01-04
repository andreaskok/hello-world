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
    public class EFChartOfAccount_Dim_ValueRepository : IChartOfAccount_Dim_ValueRepository
    {
        private EFDbContextGL context = new EFDbContextGL();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextGL"].ConnectionString);

        //public IEnumerable<ChartOfAccount_Dim_Value> ChartOfAccount_Dim_Value
        //{
        //    get { return context.ChartOfAccount_Dim_Value; }
        //}
        public IEnumerable<ChartOfAccount_Dim_Value> ChartOfAccount_Dim_Value
        {
            get
            {
                //improve performance
                return cnn.Query<ChartOfAccount_Dim_Value>("SELECT * FROM ChartOfAccount_Dim_Value");
            }
        }

        public IEnumerable<ChartOfAccount_Dim_Value> GetChartOfAccount_Dim_ValuePaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<ChartOfAccount_Dim_Value>("SELECT * FROM ChartOfAccount_Dim_Value WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<ChartOfAccount_Dim_Value> ChartOfAccount_Dim_ValueWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<ChartOfAccount_Dim_Value>("SELECT * FROM ChartOfAccount_Dim_Value WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");
        }

        public IEnumerable<ChartOfAccount_Dim_Value> GetChartOfAccount_Dim_ValueByID(Int64 id)
        {
            //improve performance
            return cnn.Query<ChartOfAccount_Dim_Value>("SELECT * FROM ChartOfAccount_Dim_Value WHERE ID = " + id);

        }

        public void SaveChartOfAccount_Dim_Value(ChartOfAccount_Dim_Value chartofaccountdimvalue)
        {
            if (chartofaccountdimvalue.ID == 0)
            {
                context.ChartOfAccount_Dim_Value.Add(chartofaccountdimvalue);
            }
            else
            {
                context.Entry(chartofaccountdimvalue).State = EntityState.Modified;
            }
            context.SaveChanges();
        }


        public void DeleteChartOfAccount_Dim_Value(ChartOfAccount_Dim_Value chartofaccountdimvalue)
        {
            context.ChartOfAccount_Dim_Value.Attach(chartofaccountdimvalue);
            context.ChartOfAccount_Dim_Value.Remove(chartofaccountdimvalue);
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
            String sSql = "SELECT Max(ID) FROM ChartOfAccount_Dim_Value";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }
    }
}

