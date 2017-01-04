using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ERPDomain.Abstract;
using ERPDomain.Concrete;
using ERPDomain.Entities;

namespace ERPDomain.Concrete
{
    public class EFWfProcessRepository : IWfProcessRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfProcess> WfProcess
        {
            get
            {
                //improve performance
                return cnn.Query<WfProcess>("SELECT * FROM WfProcess");
            }
        }

        public void SaveWfProcess(WfProcess wfProcess)
        {
            if (wfProcess.ID == 0)
            {
                context.WfProcess.Add(wfProcess);
            }
            else
            {
                context.Entry(wfProcess).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfProcess(WfProcess wfProcess)
        {
            context.WfProcess.Attach(wfProcess);
            context.WfProcess.Remove(wfProcess);
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
    }
}
