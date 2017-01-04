using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using ERPDomain.Abstract;
using ERPDomain.Entities;
using Dapper;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace ERPDomain.Concrete
{
    public class EFInvoiceLineRepository : IInvoiceLineRepository
    {
        private EFDbContextAR context = new EFDbContextAR();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextAR"].ConnectionString);

        public IEnumerable<InvoiceLine> InvoiceLine
        {
            get
            {
                return cnn.Query<InvoiceLine>("SELECT * FROM InvoiceLine");
                //return context.InvoiceLine;
            }
        }

        public IEnumerable<InvoiceLine> GetInvoiceLine(String fieldName, Int64 fieldValue)
        {
            //improve performance
            return cnn.Query<InvoiceLine>("SELECT * FROM InvoiceLine WHERE " + fieldName + " = '" + fieldValue + "'");

        }

        public IEnumerable<InvoiceLine> InvoiceLineWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<InvoiceLine>("SELECT * FROM InvoiceLine WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<InvoiceLine> GetInvoiceLineByID(Int64 id)
        {
            //improve performance
            return cnn.Query<InvoiceLine>("SELECT * FROM InvoiceLine WHERE ID = " + id);

        }

        public void SaveInvoiceLine(InvoiceLine invoiceline)
        {
            if (invoiceline.ID == 0)
            {
                context.InvoiceLine.Add(invoiceline);
            }
            else
            {
                context.Entry(invoiceline).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteInvoiceLine(InvoiceLine invoiceline)
        {
            context.InvoiceLine.Attach(invoiceline);
            context.InvoiceLine.Remove(invoiceline);
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
