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
    public class EFDimension_ValueRepository : IDimension_ValueRepository
    {
        private EFDbContext context = new EFDbContext();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContext"].ConnectionString);

        //public IEnumerable<Dimension_Value> Dimension_Value
        //{
        //    get { return context.Dimension_Value; }
        //}
        public IEnumerable<Dimension_Value> Dimension_Value
        {
            get
            {
                //improve performance
                return cnn.Query<Dimension_Value>("SELECT * FROM Dimension_Value");
            }
        }

        public IEnumerable<Dimension_Value> GetDimension_ValuePaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<Dimension_Value>("SELECT * FROM Dimension_Value WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<Dimension_Value> Dimension_ValueWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<Dimension_Value>("SELECT * FROM Dimension_Value WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");
        }

        public IEnumerable<Dimension_Value> GetDimension_ValueByID(Int64 id)
        {
            //improve performance
            return cnn.Query<Dimension_Value>("SELECT * FROM Dimension_Value WHERE ID = " + id);

        }

        public void SaveDimension_Value(Dimension_Value dimensionvalue)
        {
            if (dimensionvalue.ID == 0)
            {
                context.Dimension_Value.Add(dimensionvalue);
            }
            else
            {
                context.Entry(dimensionvalue).State = EntityState.Modified;
            }
            context.SaveChanges();
        }


        public void DeleteDimension_Value(Dimension_Value dimensionvalue)
        {
            context.Dimension_Value.Attach(dimensionvalue);
            context.Dimension_Value.Remove(dimensionvalue);
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
            String sSql = "SELECT Max(ID) FROM Dimension_Value";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
