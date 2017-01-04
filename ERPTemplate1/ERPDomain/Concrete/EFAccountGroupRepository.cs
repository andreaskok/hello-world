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
    public class EFAccountGroupRepository : IAccountGroupRepository
    {
        private EFDbContextGL context = new EFDbContextGL();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextGL"].ConnectionString);

        public IEnumerable<AccountGroup> AccountGroup
        {
            get { return context.AccountGroup; }
        }

        public IEnumerable<AccountGroup> AccountGroupWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<AccountGroup>("SELECT * FROM AccountGroup WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");
        }

        public IEnumerable<AccountGroup> GetAccountGroupPaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<AccountGroup>("SELECT * FROM AccountGroup WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<AccountGroup> GetAccountGroupByID(Int64 id)
        {
            //improve performance
            return cnn.Query<AccountGroup>("SELECT * FROM AccountGroup WHERE ID = " + id);

        }

        public void SaveAccountGroup(AccountGroup accountgroup)
        {
            if (accountgroup.ID == 0)
            {
                context.AccountGroup.Add(accountgroup);
            }
            else
            {
                context.Entry(accountgroup).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteAccountGroup(AccountGroup accountgroup)
        {
            context.AccountGroup.Attach(accountgroup);
            context.AccountGroup.Remove(accountgroup);
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

        public Int64 AccountGroupCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM AccountGroup";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64? GetMaxID()
        {
            Int64? iMax = 0;
            String sSql = "SELECT Max(ID) FROM AccountGroup";
            iMax = context.Database.SqlQuery<Int64?>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
