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
    public class EFDfMasterRepository : IDfMasterRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<DfMaster> DfMaster
        {
            get
            {
                //improve performance
                return cnn.Query<DfMaster>("SELECT * FROM DfMaster");
            }
        }

        public void SaveDfMaster(DfMaster dfMaster)
        {
            if (dfMaster.ID == 0)
            {
                context.DfMaster.Add(dfMaster);
            }
            else
            {
                context.Entry(dfMaster).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteDfMaster(DfMaster dfMaster)
        {
            context.DfMaster.Attach(dfMaster);
            context.DfMaster.Remove(dfMaster);
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
