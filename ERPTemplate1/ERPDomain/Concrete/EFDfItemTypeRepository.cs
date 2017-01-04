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
    public class EFDfItemTypeRepository : IDfItemTypeRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<DfItemType> DfItemType
        {
            get
            {
                //improve performance
                return cnn.Query<DfItemType>("SELECT * FROM DfItemType");
            }
        }

        public void SaveDfItemType(DfItemType dfItemType)
        {
            if (dfItemType.ID == 0)
            {
                context.DfItemType.Add(dfItemType);
            }
            else
            {
                context.Entry(dfItemType).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteDfItemType(DfItemType dfItemType)
        {
            context.DfItemType.Attach(dfItemType);
            context.DfItemType.Remove(dfItemType);
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
