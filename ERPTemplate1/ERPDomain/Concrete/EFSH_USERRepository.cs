using System;
using System.Collections.Generic;
using System.Data.Entity;
using ERPDomain.Abstract;
using ERPDomain.Entities;

namespace ERPDomain.Concrete
{
    public class EFSH_USERRepository : ISH_USERRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<SH_USER> SH_USER
        {

            get { return context.SH_USER; }

        }

        public void SaveSH_USER(SH_USER sh_user)
        {
            if (sh_user.ID == 0)
            {
                context.SH_USER.Add(sh_user);
            }
            else
            {
                context.Entry(sh_user).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteSH_USER(SH_USER sh_user)
        {
            context.SH_USER.Remove(sh_user);
            context.SaveChanges();
        }
    }
}
