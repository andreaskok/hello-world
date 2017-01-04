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
    public class EFErrorLogRepository : IErrorLogRepository
    {
        private EFDbContext context = new EFDbContext();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContext"].ConnectionString);

        public IEnumerable<ErrorLog> ErrorLog
        {
            get
            {
                return cnn.Query<ErrorLog>("SELECT * FROM ErrorLog WHERE id = 0");
            }
        }

        public void SaveErrorLog(ErrorLog errorLog)
        {
            if (errorLog.ID == 0)
            {
                context.ErrorLog.Add(errorLog);
            }
            else
            {

                context.Entry(errorLog).State = EntityState.Modified;
                //context.Entry(sh_app).Property(m => m.CreateDate).IsModified = false;
            }
            context.SaveChanges();
        }

        public void DeleteErrorLog(ErrorLog errorLog)
        {
            context.ErrorLog.Remove(errorLog);
            context.SaveChanges();
        }

    }
}
