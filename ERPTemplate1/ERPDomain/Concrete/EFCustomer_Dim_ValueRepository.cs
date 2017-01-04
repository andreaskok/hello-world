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
    public class EFCustomer_Dim_ValueRepository : ICustomer_Dim_ValueRepository
    {
        private EFDbContextAR context = new EFDbContextAR();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextAR"].ConnectionString);

        //public IEnumerable<Customer_Dim_Value> Customer_Dim_Value
        //{
        //    get { return context.Customer_Dim_Value; }
        //}
        public IEnumerable<Customer_Dim_Value> Customer_Dim_Value
        {
            get
            {
                //improve performance
                return cnn.Query<Customer_Dim_Value>("SELECT * FROM Customer_Dim_Value");
            }
        }

        public IEnumerable<Customer_Dim_Value> GetCustomer_Dim_ValuePaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<Customer_Dim_Value>("SELECT * FROM Customer_Dim_Value WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<Customer_Dim_Value> Customer_Dim_ValueWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<Customer_Dim_Value>("SELECT * FROM Customer_Dim_Value WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");
        }

        public IEnumerable<Customer_Dim_Value> GetCustomer_Dim_ValueByID(Int64 id)
        {
            //improve performance
            return cnn.Query<Customer_Dim_Value>("SELECT * FROM Customer_Dim_Value WHERE ID = " + id);

        }

        public void SaveCustomer_Dim_Value(Customer_Dim_Value customerdimvalue)
        {
            if (customerdimvalue.ID == 0)
            {
                context.Customer_Dim_Value.Add(customerdimvalue);
            }
            else
            {
                context.Entry(customerdimvalue).State = EntityState.Modified;
            }
            context.SaveChanges();
        }


        public void DeleteCustomer_Dim_Value(Customer_Dim_Value customerdimvalue)
        {
            context.Customer_Dim_Value.Attach(customerdimvalue);
            context.Customer_Dim_Value.Remove(customerdimvalue);
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
            String sSql = "SELECT Max(ID) FROM Customer_Dim_Value";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }
    }
}

