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
    public class EFCreditNoteRepository : ICreditNoteRepository
    {
        private EFDbContextAR context = new EFDbContextAR();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextAR"].ConnectionString);
        public IEnumerable<CreditNote> CreditNote
        {
            get
            {
                //improve performance
                return cnn.Query<CreditNote>("SELECT * FROM CreditNote WHERE id = 0");
            }
        }

        public IEnumerable<CreditNote> GetCreditNotePaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<CreditNote>("SELECT * FROM CreditNote WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<CreditNote> CreditNoteWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<CreditNote>("SELECT * FROM CreditNote WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<CreditNote> GetCreditNoteByID(Int64 id)
        {
            //improve performance
            return cnn.Query<CreditNote>("SELECT * FROM CreditNote WHERE ID = " + id);

        }

        public void SaveCreditNote(CreditNote creditnote)
        {
            if (creditnote.ID == 0)
            {
                context.CreditNote.Add(creditnote);
            }
            else
            {
                context.Entry(creditnote).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteCreditNote(CreditNote creditnote)
        {
            context.CreditNote.Attach(creditnote);
            context.CreditNote.Remove(creditnote);
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

        public Int64 CreditNoteCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM CreditNote";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64? GetMaxID()
        {
            Int64? iMax = 0;
            String sSql = "SELECT Max(ID) FROM CreditNote";
            iMax = context.Database.SqlQuery<Int64?>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
