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
    public class EFWfRequestFileRepository : IWfRequestFileRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfRequestFile> WfRequestFile
        {
            get
            {
                //improve performance
                return cnn.Query<WfRequestFile>("SELECT * FROM WfRequestFile");
            }
        }

        public void SaveWfRequestFile(WfRequestFile wfRequestFile)
        {
            if (wfRequestFile.ID == 0)
            {
                context.WfRequestFile.Add(wfRequestFile);
            }
            else
            {
                context.Entry(wfRequestFile).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfRequestFile(WfRequestFile wfRequestFile)
        {
            context.WfRequestFile.Attach(wfRequestFile);
            context.WfRequestFile.Remove(wfRequestFile);
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
