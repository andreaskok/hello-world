using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ERPDomain.Abstract;
using ERPDomain.Entities;

namespace ERPDomain.Concrete
{
    public class EFWfRequestNoteRepository : IWfRequestNoteRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfRequestNote> WfRequestNote
        {
            get
            {
                //improve performance
                return cnn.Query<WfRequestNote>("SELECT * FROM WfRequestNote");
            }
        }

        public void SaveWfRequestNote(WfRequestNote wfRequestNote)
        {
            if (wfRequestNote.ID == 0)
            {
                context.WfRequestNote.Add(wfRequestNote);
            }
            else
            {
                context.Entry(wfRequestNote).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfRequestNote(WfRequestNote wfRequestNote)
        {
            context.WfRequestNote.Attach(wfRequestNote);
            context.WfRequestNote.Remove(wfRequestNote);
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
