using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using ERPDomain.Abstract;
using ERPDomain.Entities;
using Dapper;

namespace ERPDomain.Concrete
{
    public class EFInvoiceRepository : IInvoiceRepository
    {
        private EFDbContextAR context = new EFDbContextAR();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextAR"].ConnectionString);
        public IEnumerable<Invoice> Invoice
        {
            get
            {
                //improve performance
                return cnn.Query<Invoice>("SELECT * FROM Invoice WHERE id = 0");
            }
        }

        public IEnumerable<Invoice> GetInvoicePaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<Invoice>("SELECT * FROM Invoice WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<Invoice> InvoiceWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<Invoice>("SELECT * FROM Invoice WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<Invoice> GetInvoiceByID(Int64 id)
        {
            //improve performance
            return cnn.Query<Invoice>("SELECT * FROM Invoice WHERE ID = " + id);

        }

        public void SaveInvoice(Invoice invoice)
        {
            if (invoice.ID == 0)
            {
                context.Invoice.Add(invoice);
            }
            else
            {
                context.Entry(invoice).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteInvoice(Invoice invoice)
        {
            context.Invoice.Attach(invoice);
            context.Invoice.Remove(invoice);
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

        public Int64 InvoiceCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM Invoice";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64? GetMaxID()
        {
            Int64? iMax = 0;
            String sSql = "SELECT Max(ID) FROM Invoice";
            iMax = context.Database.SqlQuery<Int64?>(sSql).Single();
            //context.Database.Query
            return iMax;
        }


    }
}
