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
    public class EFDebitNoteLineRepository : IDebitNoteLineRepository
    {
        private EFDbContextAR context = new EFDbContextAR();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextAR"].ConnectionString);
        public IEnumerable<DebitNoteLine> DebitNoteLine
        {
            get
            {
                //improve performance
                return cnn.Query<DebitNoteLine>("SELECT * FROM DebitNoteLine");
            }
        }

        public IEnumerable<DebitNoteLine> GetDebitNoteLinePaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<DebitNoteLine>("SELECT * FROM DebitNoteLine WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<DebitNoteLine> GetDebitNoteLine(String fieldName, Int64 fieldValue)
        {
            //improve performance
            return cnn.Query<DebitNoteLine>("SELECT * FROM DebitNoteLine WHERE " + fieldName + " = '" + fieldValue + "'");

        }

        public IEnumerable<DebitNoteLine> DebitNoteLineWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<DebitNoteLine>("SELECT * FROM DebitNoteLine WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<DebitNoteLine> GetDebitNoteLineByID(Int64 id)
        {
            //improve performance
            return cnn.Query<DebitNoteLine>("SELECT * FROM DebitNoteLine WHERE ID = " + id);

        }

        public void SaveDebitNoteLine(DebitNoteLine debitnoteline)
        {
            if (debitnoteline.ID == 0)
            {
                context.DebitNoteLine.Add(debitnoteline);
            }
            else
            {
                context.Entry(debitnoteline).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteDebitNoteLine(DebitNoteLine debitnoteline)
        {
            context.DebitNoteLine.Attach(debitnoteline);
            context.DebitNoteLine.Remove(debitnoteline);
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

        public Int64 DebitNoteLineCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM DebitNoteLine";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64 GetMaxID()
        {
            Int64 iMax = 0;
            String sSql = "SELECT Max(ID) FROM DebitNoteLine";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

        public void DeleteDebitNoteLineExcel(DebitNoteLine debitnoteline)
        {
            using (EFDbContextAR contextExcel = new EFDbContextAR())
            {
                contextExcel.DebitNoteLine.Attach(debitnoteline);
                contextExcel.DebitNoteLine.Remove(debitnoteline);

                bool saveFailed;
                do
                {
                    saveFailed = false;

                    try
                    {
                        contextExcel.SaveChanges();
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
}
