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
    public class EFJournalLineRepository : IJournalLineRepository
    {
        private EFDbContextGL context = new EFDbContextGL();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextGL"].ConnectionString);

        public IEnumerable<JournalLine> JournalLine
        {
            get
            {
                return cnn.Query<JournalLine>("SELECT * FROM JournalLine");
                //return context.JournalLine;
            }
        }

        public IEnumerable<JournalLine> GetJournalLine(String fieldName, Int64 fieldValue)
        {
            //improve performance
            return cnn.Query<JournalLine>("SELECT * FROM JournalLine WHERE " + fieldName + " = '" + fieldValue + "'");

        }

        public IEnumerable<JournalLine> JournalLineWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<JournalLine>("SELECT * FROM JournalLine WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<JournalLine> GetJournalLineByID(Int64 id)
        {
            //improve performance
            return cnn.Query<JournalLine>("SELECT * FROM JournalLine WHERE ID = " + id);

        }

        public void SaveJournalLine(JournalLine journalline)
        {
            if (journalline.ID == 0)
            {
                context.JournalLine.Add(journalline);
            }
            else
            {
                context.Entry(journalline).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteJournalLine(JournalLine journalline)
        {
            context.JournalLine.Attach(journalline);
            context.JournalLine.Remove(journalline);
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
