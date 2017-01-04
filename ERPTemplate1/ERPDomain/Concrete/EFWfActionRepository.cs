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
    public class EFWfActionRepository : IWfActionRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfAction> WfAction
        {
            get
            {
                //improve performance
                return cnn.Query<WfAction>("SELECT * FROM WfAction");
            }
        }

        public void SaveWfAction(WfAction wfAction)
        {
            if (wfAction.ID == 0)
            {
                context.WfAction.Add(wfAction);
            }
            else
            {
                context.Entry(wfAction).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfAction(WfAction wfAction)
        {
            context.WfAction.Attach(wfAction);
            context.WfAction.Remove(wfAction);
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
