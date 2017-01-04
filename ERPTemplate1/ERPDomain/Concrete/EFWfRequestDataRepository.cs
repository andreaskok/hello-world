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
    public class EFWfRequestDataRepository : IWfRequestDataRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfRequestData> WfRequestData
        {
            get
            {
                //improve performance
                return cnn.Query<WfRequestData>("SELECT * FROM WfRequestData");
            }
        }

        public void SaveWfRequestData(WfRequestData wfRequestData)
        {
            if (wfRequestData.ID == 0)
            {
                context.WfRequestData.Add(wfRequestData);
            }
            else
            {
                context.Entry(wfRequestData).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfRequestData(WfRequestData wfRequestData)
        {
            context.WfRequestData.Attach(wfRequestData);
            context.WfRequestData.Remove(wfRequestData);
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
