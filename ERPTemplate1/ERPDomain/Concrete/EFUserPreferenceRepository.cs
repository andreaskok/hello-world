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
    public class EFUserPreferenceRepository : IUserPreferenceRepository
    {
        private EFDbContext context = new EFDbContext();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContext"].ConnectionString);
        public IEnumerable<UserPreference> UserPreference
        {
            get
            {
                //improve performance
                return cnn.Query<UserPreference>("SELECT * FROM UserPreference");
            }
        }
        public IEnumerable<UserPreference> GetUserPreference(String fieldName, Int64 fieldValue)
        {
            //improve performance
            return cnn.Query<UserPreference>("SELECT * FROM UserPreference WHERE " + fieldName + " = '" + fieldValue + "'");

        }

        public IEnumerable<UserPreference> GetUserPreferencePaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<UserPreference>("SELECT * FROM UserPreference WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<UserPreference> UserPreferenceWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<UserPreference>("SELECT * FROM UserPreference WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<UserPreference> GetUserPreferenceByID(Int64 id)
        {
            //improve performance
            return cnn.Query<UserPreference>("SELECT * FROM UserPreference WHERE ID = " + id);

        }

        public IEnumerable<UserPreference> GetUserPreferenceBySH_USERID(Int64 UserId)
        {
            //improve performance
            return cnn.Query<UserPreference>("SELECT * FROM UserPreference WHERE SH_USERID = " + UserId);

        }

        public void SaveUserPreference(UserPreference userpreference)
        {
            if (userpreference.ID == 0)
            {
                context.UserPreference.Add(userpreference);
            }
            else
            {
                context.Entry(userpreference).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteUserPreference(UserPreference userpreference)
        {
            context.UserPreference.Attach(userpreference);
            context.UserPreference.Remove(userpreference);
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

        public Int64 UserPreferenceCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM UserPreference";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64 GetMaxID()
        {
            Int64 iMax = 0;
            String sSql = "SELECT Max(ID) FROM UserPreference";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
