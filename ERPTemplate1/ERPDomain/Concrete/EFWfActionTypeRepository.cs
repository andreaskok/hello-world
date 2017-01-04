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
    public class EFWfActionTypeRepository : IWfActionTypeRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfActionType> WfActionType
        {
            get
            {
                //improve performance
                return cnn.Query<WfActionType>("SELECT * FROM WfActionType");
            }
        }

        public void SaveWfActionType(WfActionType wfActionType)
        {
            if (wfActionType.ID == 0)
            {
                context.WfActionType.Add(wfActionType);
            }
            else
            {
                context.Entry(wfActionType).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfActionType(WfActionType wfActionType)
        {
            context.WfActionType.Attach(wfActionType);
            context.WfActionType.Remove(wfActionType);
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
