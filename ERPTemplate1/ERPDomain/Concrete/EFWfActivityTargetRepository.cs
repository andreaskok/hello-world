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
    public class EFWfActivityTargetRepository : IWfActivityTargetRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfActivityTarget> WfActivityTarget
        {
            get
            {
                //improve performance
                return cnn.Query<WfActivityTarget>("SELECT * FROM WfActivityTarget");
            }
        }

        public void SaveWfActivityTarget(WfActivityTarget wfActivityTarget)
        {
            if (wfActivityTarget.ID == 0)
            {
                context.WfActivityTarget.Add(wfActivityTarget);
            }
            else
            {
                context.Entry(wfActivityTarget).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfActivityTarget(WfActivityTarget wfActivityTarget)
        {
            context.WfActivityTarget.Attach(wfActivityTarget);
            context.WfActivityTarget.Remove(wfActivityTarget);
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
