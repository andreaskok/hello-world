using System;
using System.Collections.Generic;
using System.Data.Entity;
using ERPDomain.Abstract;
using ERPDomain.Entities;

namespace ERPDomain.Concrete
{
    public class EFSH_ROLEMEMBERRepository : ISH_ROLEMEMBERRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<SH_ROLEMEMBER> SH_ROLEMEMBER
        {

            get { return context.SH_ROLEMEMBER; }

        }

        public void SaveSH_ROLEMEMBER(SH_ROLEMEMBER sh_rolemember)
        {
            if (sh_rolemember.ID == 0)
            {
                context.SH_ROLEMEMBER.Add(sh_rolemember);
            }
            else
            {

                context.Entry(sh_rolemember).State = EntityState.Modified;
                //context.Entry(sh_role).Property(m => m.CreateDate).IsModified = false;
            }
            context.SaveChanges();
        }

        public void DeleteSH_ROLEMEMBER(SH_ROLEMEMBER sh_rolemember)
        {
            context.SH_ROLEMEMBER.Remove(sh_rolemember);
            context.SaveChanges();
        }
    }
}
