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
    public class EFJobScheduleRepository : IJobScheduleRepository
    {
        private EFDbContext context = new EFDbContext();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContext"].ConnectionString);
        public IEnumerable<JobSchedule> JobSchedule
        {
            get
            {
                //improve performance
                     return cnn.Query<JobSchedule>("SELECT * FROM JobSchedule");
            }
        }

        public IEnumerable<JobSchedule> GetJobSchedulePaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<JobSchedule>("SELECT * FROM JobSchedule WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<JobSchedule> JobScheduleWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<JobSchedule>("SELECT * FROM JobSchedule WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<JobSchedule> GetJobScheduleByID(Int64 id)
        {
            //improve performance
            return cnn.Query<JobSchedule>("SELECT * FROM JobSchedule WHERE ID = " + id);

        }

        public void SaveJobSchedule(JobSchedule jobschedule)
        {
            if (jobschedule.ID == 0)
            {
                context.JobSchedule.Add(jobschedule);
            }
            else
            {
                context.Entry(jobschedule).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteJobSchedule(JobSchedule jobschedule)
        {
            context.JobSchedule.Attach(jobschedule);
            context.JobSchedule.Remove(jobschedule);
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

        public Int64 JobScheduleCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM JobSchedule";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64? GetMaxID()
        {
            Int64? iMax = 0;
            String sSql = "SELECT Max(ID) FROM JobSchedule";
            iMax = context.Database.SqlQuery<Int64?>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
