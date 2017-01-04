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
    public class EFPaymentLineRepository : IPaymentLineRepository
    {
        private EFDbContextAP context = new EFDbContextAP();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextAP"].ConnectionString);

        public IEnumerable<PaymentLine> PaymentLine
        {
            get
            {
                return cnn.Query<PaymentLine>("SELECT * FROM PaymentLine");
                //return context.InvoiceLine;
            }
        }

        public IEnumerable<PaymentLine> GetPaymentLine(String fieldName, Int64 fieldValue)
        {
            //improve performance
            return cnn.Query<PaymentLine>("SELECT * FROM PaymentLine WHERE " + fieldName + " = '" + fieldValue + "'");

        }

        public IEnumerable<PaymentLine> PaymentLineWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<PaymentLine>("SELECT * FROM PaymentLine WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<PaymentLine> GetPaymentLineByID(Int64 id)
        {
            //improve performance
            return cnn.Query<PaymentLine>("SELECT * FROM PaymentLine WHERE ID = " + id);

        }

        public void SavePaymentLine(PaymentLine paymentline)
        {
            if (paymentline.ID == 0)
            {
                context.PaymentLine.Add(paymentline);
            }
            else
            {
                context.Entry(paymentline).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeletePaymentLine(PaymentLine paymentline)
        {
            context.PaymentLine.Attach(paymentline);
            context.PaymentLine.Remove(paymentline);
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
