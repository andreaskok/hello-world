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
    public class EFWfEscalationRepository : IWfEscalationRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfEscalation> WfEscalation
        {
            get
            {
                //improve performance
                return cnn.Query<WfEscalation>("SELECT * FROM WfEscalation");
            }
        }

        public void SaveWfEscalation(WfEscalation wfEscalation)
        {
            if (wfEscalation.ID == 0)
            {
                context.WfEscalation.Add(wfEscalation);
            }
            else
            {
                context.Entry(wfEscalation).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfEscalation(WfEscalation wfEscalation)
        {
            context.WfEscalation.Attach(wfEscalation);
            context.WfEscalation.Remove(wfEscalation);
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
