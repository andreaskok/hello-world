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
    public class EFWfTransitionRepository : IWfTransitionRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfTransition> WfTransition
        {
            get
            {
                //improve performance
                return cnn.Query<WfTransition>("SELECT * FROM WfTransition");
            }
        }

        public void SaveWfTransition(WfTransition wfTransition)
        {
            if (wfTransition.ID == 0)
            {
                context.WfTransition.Add(wfTransition);
            }
            else
            {
                context.Entry(wfTransition).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfTransition(WfTransition wfTransition)
        {
            context.WfTransition.Attach(wfTransition);
            context.WfTransition.Remove(wfTransition);
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
