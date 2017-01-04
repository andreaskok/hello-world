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
    public class EFWfRequestStakeholderRepository : IWfRequestStakeholderRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfRequestStakeholder> WfRequestStakeholder
        {
            get
            {
                //improve performance
                return cnn.Query<WfRequestStakeholder>("SELECT * FROM WfRequestStakeholder");
            }
        }

        public void SaveWfRequestStakeholder(WfRequestStakeholder wfRequestStakeholder)
        {
            if (wfRequestStakeholder.ID == 0)
            {
                context.WfRequestStakeholder.Add(wfRequestStakeholder);
            }
            else
            {
                context.Entry(wfRequestStakeholder).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfRequestStakeholder(WfRequestStakeholder wfRequestStakeholder)
        {
            context.WfRequestStakeholder.Attach(wfRequestStakeholder);
            context.WfRequestStakeholder.Remove(wfRequestStakeholder);
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
