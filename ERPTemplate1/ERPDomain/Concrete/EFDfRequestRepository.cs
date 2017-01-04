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
    public class EFDfRequestRepository : IDfRequestRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<DfRequest> DfRequest
        {
            get
            {
                //improve performance
                return cnn.Query<DfRequest>("SELECT * FROM DfRequest");
            }
        }

        public void SaveDfRequest(DfRequest dfRequest)
        {
            if (dfRequest.ID == 0)
            {
                context.DfRequest.Add(dfRequest);
            }
            else
            {
                context.Entry(dfRequest).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteDfRequest(DfRequest dfRequest)
        {
            context.DfRequest.Attach(dfRequest);
            context.DfRequest.Remove(dfRequest);
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
