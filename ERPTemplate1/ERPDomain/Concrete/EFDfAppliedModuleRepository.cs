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
    public class EFDfAppliedModuleRepository : IDfAppliedModuleRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<DfAppliedModule> DfAppliedModule
        {
            get
            {
                //improve performance
                return cnn.Query<DfAppliedModule>("SELECT * FROM DfAppliedModule");
            }
        }

        public void SaveDfAppliedModule(DfAppliedModule dfAppliedModule)
        {
            if (dfAppliedModule.ID == 0)
            {
                context.DfAppliedModule.Add(dfAppliedModule);
            }
            else
            {
                context.Entry(dfAppliedModule).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteDfAppliedModule(DfAppliedModule dfAppliedModule)
        {
            context.DfAppliedModule.Attach(dfAppliedModule);
            context.DfAppliedModule.Remove(dfAppliedModule);
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
