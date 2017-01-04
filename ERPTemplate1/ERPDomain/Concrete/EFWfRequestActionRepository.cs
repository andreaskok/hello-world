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
    public class EFWfRequestActionRepository : IWfRequestActionRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfRequestAction> WfRequestAction
        {
            get
            {
                //improve performance
                return cnn.Query<WfRequestAction>("SELECT * FROM WfRequestAction");
            }
        }

        public void SaveWfRequestAction(WfRequestAction wfRequestAction)
        {
            if (wfRequestAction.ID == 0)
            {
                context.WfRequestAction.Add(wfRequestAction);
            }
            else
            {
                context.Entry(wfRequestAction).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfRequestAction(WfRequestAction wfRequestAction)
        {
            context.WfRequestAction.Attach(wfRequestAction);
            context.WfRequestAction.Remove(wfRequestAction);
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
