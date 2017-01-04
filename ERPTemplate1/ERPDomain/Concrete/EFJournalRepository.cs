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
using ERPDomain.Models;

namespace ERPDomain.Concrete
{
    public class EFJournalRepository : IJournalRepository
    {
        private EFDbContextGL context = new EFDbContextGL();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextGL"].ConnectionString);
        public IEnumerable<Journal> Journal
        {
            get
            {

                //context.Configuration.ProxyCreationEnabled = false;
                //return context.Journal;
                //improve performance
                return cnn.Query<Journal>("SELECT * FROM Journal where ID = 0");
            }
        }

        public IEnumerable<Journal> GetJournalPaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<Journal>("SELECT * FROM Journal WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<Journal> JournalWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<Journal>("SELECT * FROM Journal WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<Journal> GetJournalByID(Int64 id)
        {
            //improve performance
            return cnn.Query<Journal>("SELECT * FROM Journal WHERE ID = " + id);

        }

        public IEnumerable<MonthEndPostingModel> GetJournalMonthEndPostingModel(Int64 journalID)
        {
            return cnn.Query<MonthEndPostingModel>("SELECT coa.OrganizationID,jh.ID AS VoucherID,jl.ID AS VoucherLineID,jh.JournalCode AS VoucherCode, jl.JournalLineCode As VoucherLineCode, jl.Description, jh.AccYear, jh.AccMonth,jl.AccCode, jh.TransactType, jh.DocDate, jh.DocAmt, jl.Quantity,jl.UnitPrice, jl.Total, jl.DebitCreditIndicator, 'Journal' AS PostingTable, SYSDATETIME() as PostingDate FROM JournalLine jl LEFT JOIN Journal jh ON jl.JournalID=jh.ID LEFT JOIN ChartOfAccount coa ON jl.ChartOfAccountID=coa.ID WHERE jh.ID = '" + journalID + "'");
        }

        public void SaveJournal(Journal journal)
        {
            if (journal.ID == 0)
            {
                context.Journal.Add(journal);
            }
            else
            {
                context.Entry(journal).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteJournal(Journal journal)
        {
            context.Journal.Attach(journal);
            context.Journal.Remove(journal);
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

        public Int64 JournalCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM Journal";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64? GetMaxID()
        {
            Int64? iMax = 0;
            String sSql = "SELECT Max(ID) FROM Journal";
            iMax = context.Database.SqlQuery<Int64?>(sSql).Single();
            //context.Database.Query
            return iMax;
        }
    }
}
