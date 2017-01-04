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
    public class EFPaymentRepository : IPaymentRepository
    {
        private EFDbContextAP context = new EFDbContextAP();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextAP"].ConnectionString);
        public IEnumerable<Payment> Payment
        {
            get
            {
                //improve performance
                return cnn.Query<Payment>("SELECT * FROM Payment WHERE id = 0");
            }
        }

        public IEnumerable<Payment> GetPaymentPaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<Payment>("SELECT * FROM Payment WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<Payment> PaymentWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<Payment>("SELECT * FROM Payment WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<Payment> GetPaymentByID(Int64 id)
        {
            //improve performance
            return cnn.Query<Payment>("SELECT * FROM Payment WHERE ID = " + id);

        }

        public void SavePayment(Payment payment)
        {
            if (payment.ID == 0)
            {
                context.Payment.Add(payment);
            }
            else
            {
                context.Entry(payment).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeletePayment(Payment payment)
        {
            context.Payment.Attach(payment);
            context.Payment.Remove(payment);
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

        public Int64 PaymentCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM Payment";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64? GetMaxID()
        {
            Int64? iMax = 0;
            String sSql = "SELECT Max(ID) FROM Payment";
            iMax = context.Database.SqlQuery<Int64?>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
