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
    public class EFCountryStateRepository : ICountryStateRepository
    {
        private EFDbContext context = new EFDbContext();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContext"].ConnectionString);
        public IEnumerable<CountryState> CountryState
        {
            get
            {
                //improve performance
                return cnn.Query<CountryState>("SELECT * FROM CountryState");
            }
        }
        public IEnumerable<CountryState> GetCountryState(String fieldName, Int64 fieldValue)
        {
            //improve performance
            return cnn.Query<CountryState>("SELECT * FROM CountryState WHERE " + fieldName + " = '" + fieldValue + "'");

        }

        public IEnumerable<CountryState> GetCountryStatePaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<CountryState>("SELECT * FROM CountryState WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<CountryState> CountryStateWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<CountryState>("SELECT * FROM CountryState WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<CountryState> GetCountryStateByID(Int64 id)
        {
            //improve performance
            return cnn.Query<CountryState>("SELECT * FROM CountryState WHERE ID = " + id);

        }

        public void SaveCountryState(CountryState countrystate)
        {
            if (countrystate.ID == 0)
            {
                context.CountryState.Add(countrystate);
            }
            else
            {
                context.Entry(countrystate).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteCountryState(CountryState countrystate)
        {
            context.CountryState.Attach(countrystate);
            context.CountryState.Remove(countrystate);
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

        public Int64 CountryStateCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM CountryState";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64 GetMaxID()
        {
            Int64 iMax = 0;
            String sSql = "SELECT Max(ID) FROM CountryState";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
