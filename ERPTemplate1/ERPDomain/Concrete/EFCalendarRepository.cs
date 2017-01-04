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
    public class EFCalendarRepository : ICalendarRepository
    {
        private EFDbContext context = new EFDbContext();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContext"].ConnectionString);
        public IEnumerable<Calendar> Calendar
        {
            get
            {
                //improve performance
                return cnn.Query<Calendar>("SELECT * FROM Calendar WHERE id = 0");
            }
        }
        public IEnumerable<Calendar> GetCalendarPaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<Calendar>("SELECT * FROM Calendar WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<Calendar> CalendarWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<Calendar>("SELECT * FROM Calendar WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<Calendar> GetCalendarByID(Int64 id)
        {
            //improve performance
            return cnn.Query<Calendar>("SELECT * FROM Calendar WHERE ID = " + id);

        }

        public void SaveCalendar(Calendar calendar)
        {
            if (calendar.ID == 0)
            {
                context.Calendar.Add(calendar);
            }
            else
            {
                context.Entry(calendar).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteCalendar(Calendar calendar)
        {
            context.Calendar.Attach(calendar);
            context.Calendar.Remove(calendar);
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

        public Int64 CalendarCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM Calendar";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64 GetMaxID()
        {
            Int64 iMax = 0;
            String sSql = "SELECT Max(ID) FROM Calendar";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }

}

