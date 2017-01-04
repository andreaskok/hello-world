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
    public class EFDfRequestDataRepository : IDfRequestDataRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<DfRequestData> DfRequestData
        {
            get
            {
                //improve performance
                return cnn.Query<DfRequestData>("SELECT * FROM DfRequestData");
            }
        }

        public void SaveDfRequestData(DfRequestData dfRequestData)
        {
            if (dfRequestData.ID == 0)
            {
                context.DfRequestData.Add(dfRequestData);
            }
            else
            {
                context.Entry(dfRequestData).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteDfRequestData(DfRequestData dfRequestData)
        {
            context.DfRequestData.Attach(dfRequestData);
            context.DfRequestData.Remove(dfRequestData);
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
