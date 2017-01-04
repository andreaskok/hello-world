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
    public class EFWfActivityTypeRepository : IWfActivityTypeRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfActivityType> WfActivityType
        {
            get
            {
                //improve performance
                return cnn.Query<WfActivityType>("SELECT * FROM WfActivityType");
            }
        }

        public void SaveWfActivityType(WfActivityType wfActivityType)
        {
            if (wfActivityType.ID == 0)
            {
                context.WfActivityType.Add(wfActivityType);
            }
            else
            {
                context.Entry(wfActivityType).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfActivityType(WfActivityType wfActivityType)
        {
            context.WfActivityType.Attach(wfActivityType);
            context.WfActivityType.Remove(wfActivityType);
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
