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
    public class EFOrganizationRepository : IOrganizationRepository
    {
        private EFDbContext context = new EFDbContext();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContext"].ConnectionString);
        public IEnumerable<Organization> Organization
        {
            get
            {
                //improve performance
                return cnn.Query<Organization>("SELECT * FROM Organization WHERE id = 0");
            }
        }
        public IEnumerable<Organization> GetOrganizationPaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<Organization>("SELECT * FROM Organization WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<Organization> OrganizationWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<Organization>("SELECT * FROM Organization WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<Organization> GetOrganizationByID(Int64 id)
        {
            //improve performance
            return cnn.Query<Organization>("SELECT * FROM Organization WHERE ID = " + id);

        }

        public void SaveOrganization(Organization organization)
        {
            if (organization.ID == 0)
            {
                context.Organization.Add(organization);
            }
            else
            {
                context.Entry(organization).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteOrganization(Organization organization)
        {
            context.Organization.Attach(organization);
            context.Organization.Remove(organization);
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

        public Int64 OrganizationCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM Organization";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64 GetMaxID()
        {
            Int64 iMax = 0;
            String sSql = "SELECT Max(ID) FROM Organization";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
