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
    public class EFWfGroupRepository : IWfGroupRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfGroup> WfGroup
        {
            get
            {
                //improve performance
                return cnn.Query<WfGroup>("SELECT * FROM WfGroup");
            }
        }

        public void SaveWfGroup(WfGroup wfGroup)
        {
            if (wfGroup.ID == 0)
            {
                context.WfGroup.Add(wfGroup);
            }
            else
            {
                context.Entry(wfGroup).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfGroup(WfGroup wfGroup)
        {
            context.WfGroup.Attach(wfGroup);
            context.WfGroup.Remove(wfGroup);
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
