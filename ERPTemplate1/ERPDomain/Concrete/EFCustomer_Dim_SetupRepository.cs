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
    public class EFCustomer_Dim_SetupRepository : ICustomer_Dim_SetupRepository
    {
        private EFDbContextAR context = new EFDbContextAR();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextAR"].ConnectionString);

        //public IEnumerable<Customer_Dim_Setup> Customer_Dim_Setup
        //{
        //    get { return context.Customer_Dim_Setup; }
        //}
        public IEnumerable<Customer_Dim_Setup> Customer_Dim_Setup
        {
            get
            {
                //improve performance
                return cnn.Query<Customer_Dim_Setup>("SELECT * FROM Customer_Dim_Setup");
            }
        }

        public IEnumerable<Customer_Dim_Setup> GetCustomer_Dim_SetupPaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<Customer_Dim_Setup>("SELECT * FROM Customer_Dim_Setup WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<Customer_Dim_Setup> Customer_Dim_SetupWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<Customer_Dim_Setup>("SELECT * FROM Customer_Dim_Setup WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");
        }

        public IEnumerable<Customer_Dim_Setup> GetCustomer_Dim_SetupByID(Int64 id)
        {
            //improve performance
            return cnn.Query<Customer_Dim_Setup>("SELECT * FROM Customer_Dim_Setup WHERE ID = " + id);

        }

        public void SaveCustomer_Dim_Setup(Customer_Dim_Setup customerdimsetup)
        {
            if (customerdimsetup.ID == 0)
            {
                context.Customer_Dim_Setup.Add(customerdimsetup);
            }
            else
            {
                context.Entry(customerdimsetup).State = EntityState.Modified;
            }
            context.SaveChanges();
        }


        public void DeleteCustomer_Dim_Setup(Customer_Dim_Setup customerdimsetup)
        {
            context.Customer_Dim_Setup.Attach(customerdimsetup);
            context.Customer_Dim_Setup.Remove(customerdimsetup);
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
            String sSql = "SELECT Max(ID) FROM Customer_Dim_Setup";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
