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
    public class EFWfActionTargetRepository : IWfActionTargetRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfActionTarget> WfActionTarget
        {
            get
            {
                //improve performance
                return cnn.Query<WfActionTarget>("SELECT * FROM WfActionTarget");
            }
        }

        public void SaveWfActionTarget(WfActionTarget wfActionTarget)
        {
            if (wfActionTarget.ID == 0)
            {
                context.WfActionTarget.Add(wfActionTarget);
            }
            else
            {
                context.Entry(wfActionTarget).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfActionTarget(WfActionTarget wfActionTarget)
        {
            context.WfActionTarget.Attach(wfActionTarget);
            context.WfActionTarget.Remove(wfActionTarget);
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
