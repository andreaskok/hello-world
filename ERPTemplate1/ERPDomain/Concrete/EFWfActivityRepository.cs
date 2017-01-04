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
    public class EFWfActivityRepository : IWfActivityRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfActivity> WfActivity
        {
            get
            {
                //improve performance
                return cnn.Query<WfActivity>("SELECT * FROM WfActivity");
            }
        }

        public void SaveWfActivity(WfActivity wfActivity)
        {
            if (wfActivity.ID == 0)
            {
                context.WfActivity.Add(wfActivity);
            }
            else
            {
                context.Entry(wfActivity).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfActivity(WfActivity wfActivity)
        {
            context.WfActivity.Attach(wfActivity);
            context.WfActivity.Remove(wfActivity);
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
