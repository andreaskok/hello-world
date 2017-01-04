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
    public class EFCreditNoteLineRepository : ICreditNoteLineRepository
    {
        private EFDbContextAR context = new EFDbContextAR();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextAR"].ConnectionString);
        public IEnumerable<CreditNoteLine> CreditNoteLine
        {
            get
            {
                //improve performance
                return cnn.Query<CreditNoteLine>("SELECT * FROM CreditNoteLine WHERE id = 0");
            }
        }

        public IEnumerable<CreditNoteLine> GetCreditNoteLinePaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<CreditNoteLine>("SELECT * FROM CreditNoteLine WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<CreditNoteLine> GetCreditNoteLine(String fieldName, Int64 fieldValue)
        {
            //improve performance
            return cnn.Query<CreditNoteLine>("SELECT * FROM CreditNoteLine WHERE " + fieldName + " = '" + fieldValue + "'");

        }

        public IEnumerable<CreditNoteLine> CreditNoteLineWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<CreditNoteLine>("SELECT * FROM CreditNoteLine WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<CreditNoteLine> GetCreditNoteLineByID(Int64 id)
        {
            //improve performance
            return cnn.Query<CreditNoteLine>("SELECT * FROM CreditNoteLine WHERE ID = " + id);

        }

        public void SaveCreditNoteLine(CreditNoteLine creditnoteline)
        {
            if (creditnoteline.ID == 0)
            {
                context.CreditNoteLine.Add(creditnoteline);
            }
            else
            {
                context.Entry(creditnoteline).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteCreditNoteLine(CreditNoteLine creditnoteline)
        {
            context.CreditNoteLine.Attach(creditnoteline);
            context.CreditNoteLine.Remove(creditnoteline);
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

        public Int64 CreditNoteLineCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM CreditNoteLine";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64 GetMaxID()
        {
            Int64 iMax = 0;
            String sSql = "SELECT Max(ID) FROM CreditNoteLine";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
