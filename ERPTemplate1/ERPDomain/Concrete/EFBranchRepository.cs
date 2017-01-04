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
    public class EFBranchRepository : IBranchRepository
    {
        private EFDbContext context = new EFDbContext();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContext"].ConnectionString);
        public IEnumerable<Branch> Branch
        {
            get
            {
                //improve performance
                return cnn.Query<Branch>("SELECT * FROM Branch WHERE id = 0");
            }
        }
        public IEnumerable<Branch> GetBranch(String fieldName, Int64 fieldValue)
        {
            //improve performance
            return cnn.Query<Branch>("SELECT * FROM Branch WHERE " + fieldName + " = '" + fieldValue + "'");

        }

        public IEnumerable<Branch> GetBranchPaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<Branch>("SELECT * FROM Branch WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<Branch> BranchWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<Branch>("SELECT * FROM Branch WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<Branch> GetBranchByID(Int64 id)
        {
            //improve performance
            return cnn.Query<Branch>("SELECT * FROM Branch WHERE ID = " + id);

        }

        public void SaveBranch(Branch branch)
        {
            if (branch.ID == 0)
            {
                context.Branch.Add(branch);
            }
            else
            {
                context.Entry(branch).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteBranch(Branch branch)
        {
            context.Branch.Attach(branch);
            context.Branch.Remove(branch);
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

        public Int64 BranchCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM Branch";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64 GetMaxID()
        {
            Int64 iMax = 0;
            String sSql = "SELECT Max(ID) FROM Branch";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
