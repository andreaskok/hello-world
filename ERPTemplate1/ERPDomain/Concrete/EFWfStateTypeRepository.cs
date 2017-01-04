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
    public class EFWfStateTypeRepository : IWfStateTypeRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfStateType> WfStateType
        {
            get
            {
                //improve performance
                return cnn.Query<WfStateType>("SELECT * FROM WfStateType");
            }
        }

        public void SaveWfStateType(WfStateType wfStateType)
        {
            if (wfStateType.ID == 0)
            {
                context.WfStateType.Add(wfStateType);
            }
            else
            {
                context.Entry(wfStateType).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfStateType(WfStateType wfStateType)
        {
            context.WfStateType.Attach(wfStateType);
            context.WfStateType.Remove(wfStateType);
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
