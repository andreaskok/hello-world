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
    public class EFDfExemptItemRepository : IDfExemptItemRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<DfExemptItem> DfExemptItem
        {
            get
            {
                //improve performance
                return cnn.Query<DfExemptItem>("SELECT * FROM DfExemptItem");
            }
        }

        public void SaveDfExemptItem(DfExemptItem dfExemptItem)
        {
            if (dfExemptItem.ID == 0)
            {
                context.DfExemptItem.Add(dfExemptItem);
            }
            else
            {
                context.Entry(dfExemptItem).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteDfExemptItem(DfExemptItem dfExemptItem)
        {
            context.DfExemptItem.Attach(dfExemptItem);
            context.DfExemptItem.Remove(dfExemptItem);
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
