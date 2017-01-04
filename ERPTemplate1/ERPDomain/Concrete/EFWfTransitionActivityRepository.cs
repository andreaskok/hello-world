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
    public class EFWfTransitionActivityRepository : IWfTransitionActivityRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfTransitionActivity> WfTransitionActivity
        {
            get
            {
                //improve performance
                return cnn.Query<WfTransitionActivity>("SELECT * FROM WfTransitionActivity");
            }
        }

        public void SaveWfTransitionActivity(WfTransitionActivity wfTransitionActivity)
        {
            if (wfTransitionActivity.ID == 0)
            {
                context.WfTransitionActivity.Add(wfTransitionActivity);
            }
            else
            {
                context.Entry(wfTransitionActivity).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfTransitionActivity(WfTransitionActivity wfTransitionActivity)
        {
            context.WfTransitionActivity.Attach(wfTransitionActivity);
            context.WfTransitionActivity.Remove(wfTransitionActivity);
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
