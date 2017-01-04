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
    public class EFWfGroupMemberRepository : IWfGroupMemberRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfGroupMember> WfGroupMember
        {
            get
            {
                //improve performance
                return cnn.Query<WfGroupMember>("SELECT * FROM WfGroupMember");
            }
        }

        public void SaveWfGroupMember(WfGroupMember wfGroupMember)
        {
            if (wfGroupMember.ID == 0)
            {
                context.WfGroupMember.Add(wfGroupMember);
            }
            else
            {
                context.Entry(wfGroupMember).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfGroupMember(WfGroupMember wfGroupMember)
        {
            context.WfGroupMember.Attach(wfGroupMember);
            context.WfGroupMember.Remove(wfGroupMember);
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
