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
    public class EFDimension_TableRelationshipRepository : IDimension_TableRelationshipRepository
    {
        private EFDbContext context = new EFDbContext();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContext"].ConnectionString);

        //public IEnumerable<Dimension_TableRelationship> Dimension_TableRelationship
        //{
        //    get { return context.Dimension_TableRelationship; }
        //}
        public IEnumerable<Dimension_TableRelationship> Dimension_TableRelationship
        {
            get
            {
                //improve performance
                return cnn.Query<Dimension_TableRelationship>("SELECT * FROM Dimension_TableRelationship");
            }
        }



        public IEnumerable<Dimension_TableRelationship> GetDimension_TableRelationshipPaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<Dimension_TableRelationship>("SELECT * FROM Dimension_TableRelationship WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<Dimension_TableRelationship> Dimension_TableRelationshipWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<Dimension_TableRelationship>("SELECT * FROM Dimension_TableRelationship WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");
        }

        public IEnumerable<Dimension_TableRelationship> GetDimension_TableRelationshipByID(Int64 id)
        {
            //improve performance
            return cnn.Query<Dimension_TableRelationship>("SELECT * FROM Dimension_TableRelationship WHERE ID = " + id);

        }

        public void SaveDimension_TableRelationship(Dimension_TableRelationship dimensiontablerelationship)
        {
            if (dimensiontablerelationship.ID == 0)
            {
                context.Dimension_TableRelationship.Add(dimensiontablerelationship);
            }
            else
            {
                context.Entry(dimensiontablerelationship).State = EntityState.Modified;
            }
            context.SaveChanges();
        }


        public void DeleteDimension_TableRelationship(Dimension_TableRelationship dimensiontablerelationship)
        {
            context.Dimension_TableRelationship.Attach(dimensiontablerelationship);
            context.Dimension_TableRelationship.Remove(dimensiontablerelationship);
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
            String sSql = "SELECT Max(ID) FROM Dimension_TableRelationship";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}

