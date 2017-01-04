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
using ERPDomain.Entities;

namespace ERPDomain.Concrete
{
    public class EFWfProcessAdminRepository : IWfProcessAdminRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfProcessAdmin> WfProcessAdmin
        {
            get
            {
                //improve performance
                return cnn.Query<WfProcessAdmin>("SELECT * FROM WfProcessAdmin");
            }
        }

        public void SaveWfProcessAdmin(WfProcessAdmin wfProcessAdmin)
        {
            if (wfProcessAdmin.ID == 0)
            {
                context.WfProcessAdmin.Add(wfProcessAdmin);
            }
            else
            {
                context.Entry(wfProcessAdmin).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfProcessAdmin(WfProcessAdmin wfProcessAdmin)
        {
            context.WfProcessAdmin.Attach(wfProcessAdmin);
            context.WfProcessAdmin.Remove(wfProcessAdmin);
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
