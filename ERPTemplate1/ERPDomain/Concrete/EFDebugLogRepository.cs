using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ERPDomain.Abstract;
using ERPDomain.Entities;

namespace ERPDomain.Concrete
{
    public class EFDebugLogRepository : IDebugLogRepository
    {
        private EFDbContext context = new EFDbContext();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContext"].ConnectionString);

        public IEnumerable<DebugLog> DebugLog
        {
            get
            {
                return cnn.Query<DebugLog>("SELECT * FROM DebugLog WHERE id = 0");
            }
        }

        public void SaveDebugLog(DebugLog debugLog)
        {
            if (debugLog.ID == 0)
            {
                context.DebugLog.Add(debugLog);
            }
            else
            {

                context.Entry(debugLog).State = EntityState.Modified;
                //context.Entry(sh_app).Property(m => m.CreateDate).IsModified = false;
            }
            context.SaveChanges();
        }

        public void DeleteDebugLog(DebugLog debugLog)
        {
            context.DebugLog.Remove(debugLog);
            context.SaveChanges();
        }
    }
}
