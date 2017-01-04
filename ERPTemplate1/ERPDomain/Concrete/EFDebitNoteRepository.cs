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
    public class EFDebitNoteRepository : IDebitNoteRepository
    {
        private EFDbContextAR context = new EFDbContextAR();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextAR"].ConnectionString);
        public IEnumerable<DebitNote> DebitNote
        {
            get
            {
                //improve performance
                return cnn.Query<DebitNote>("SELECT * FROM DebitNote");
            }
        }

        public IEnumerable<DebitNote> GetDebitNotePaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<DebitNote>("SELECT * FROM DebitNote WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<DebitNote> DebitNoteWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<DebitNote>("SELECT * FROM DebitNote WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<DebitNote> GetDebitNoteByID(Int64 id)
        {
            //improve performance
            return cnn.Query<DebitNote>("SELECT * FROM DebitNote WHERE ID = " + id);

        }

        public void SaveDebitNote(DebitNote debitnote)
        {
            if (debitnote.ID == 0)
            {
                context.DebitNote.Add(debitnote);
            }
            else
            {
                context.Entry(debitnote).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteDebitNote(DebitNote debitnote)
        {
            context.DebitNote.Attach(debitnote);
            context.DebitNote.Remove(debitnote);
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

        public Int64 DebitNoteCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM DebitNote";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64? GetMaxID()
        {
            Int64? iMax = 0;
            String sSql = "SELECT Max(ID) FROM DebitNote";
            iMax = context.Database.SqlQuery<Int64?>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
