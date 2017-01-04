using System;
using System.Collections.Generic;
using System.Data.Entity;
using ERPDomain.Abstract;
using ERPDomain.Entities;

namespace ERPDomain.Concrete
{
    public class EFSH_ROLEACCESSRepository : ISH_ROLEACCESSRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<SH_ROLEACCESS> SH_ROLEACCESS
        {

            get { return context.SH_ROLEACCESS; }

        }

        public void SaveSH_ROLEACCESS(SH_ROLEACCESS sh_roleaccess)
        {
            if (sh_roleaccess.ID == 0)
            {
                context.SH_ROLEACCESS.Add(sh_roleaccess);
            }
            else
            {

                context.Entry(sh_roleaccess).State = EntityState.Modified;
                context.Entry(sh_roleaccess).Property(m => m.CreateDate).IsModified = false;
            }
            context.SaveChanges();
        }

        public void DeleteSH_ROLEACCESS(SH_ROLEACCESS sh_roleaccess)
        {
            context.SH_ROLEACCESS.Remove(sh_roleaccess);
            context.SaveChanges();
        }
    }
}
