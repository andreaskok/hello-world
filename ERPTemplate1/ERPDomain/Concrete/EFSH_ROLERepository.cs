using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using ERPDomain.Abstract;
using ERPDomain.Entities;

namespace ERPDomain.Concrete
{
    public class EFSH_ROLERepository : ISH_ROLERepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<SH_ROLE> SH_ROLE
        {

            get { return context.SH_ROLE; }

        }

        public void SaveSH_ROLE(SH_ROLE sh_role)
        {
            if (sh_role.ID == 0)
            {
                context.SH_ROLE.Add(sh_role);
            }
            else
            {

                context.Entry(sh_role).State = EntityState.Modified;
                context.Entry(sh_role).Property(m => m.CreateDate).IsModified = false;
            }
            context.SaveChanges();
        }

        public void DeleteSH_ROLE(SH_ROLE sh_role)
        {
            context.SH_ROLE.Remove(sh_role);
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
