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
    public class EFWfStateRepository : IWfStateRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfState> WfState
        {
            get
            {
                //improve performance
                return cnn.Query<WfState>("SELECT * FROM WfState");
            }
        }

        public void SaveWfState(WfState wfState)
        {
            if (wfState.ID == 0)
            {
                context.WfState.Add(wfState);
            }
            else
            {
                context.Entry(wfState).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfState(WfState wfState)
        {
            context.WfState.Attach(wfState);
            context.WfState.Remove(wfState);
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
