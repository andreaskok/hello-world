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
    public class EFCountryRepository : ICountryRepository
    {
        private EFDbContext context = new EFDbContext();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContext"].ConnectionString);
        public IEnumerable<Country> Country
        {
            get
            {
                //improve performance
                return cnn.Query<Country>("SELECT * FROM Country");
            }
        }
        public IEnumerable<Country> GetCountryPaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<Country>("SELECT * FROM Country WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<Country> CountryWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<Country>("SELECT * FROM Country WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<Country> GetCountryByID(Int64 id)
        {
            //improve performance
            return cnn.Query<Country>("SELECT * FROM Country WHERE ID = " + id);

        }

        public void SaveCountry(Country country)
        {
            if (country.ID == 0)
            {
                context.Country.Add(country);
            }
            else
            {
                context.Entry(country).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteCountry(Country country)
        {
            context.Country.Attach(country);
            context.Country.Remove(country);
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

        public Int64 CountryCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM Country";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64 GetMaxID()
        {
            Int64 iMax = 0;
            String sSql = "SELECT Max(ID) FROM Country";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
