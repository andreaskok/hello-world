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
    public class EFDfMasterDataRepository : IDfMasterDataRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<DfMasterData> DfMasterData
        {
            get
            {
                //improve performance
                return cnn.Query<DfMasterData>("SELECT * FROM DfMasterData");
            }
        }

        public void SaveDfMasterData(DfMasterData dfMasterData)
        {
            if (dfMasterData.ID == 0)
            {
                context.DfMasterData.Add(dfMasterData);
            }
            else
            {
                context.Entry(dfMasterData).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteDfMasterData(DfMasterData dfMasterData)
        {
            context.DfMasterData.Attach(dfMasterData);
            context.DfMasterData.Remove(dfMasterData);
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
