using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Abstract;
using ERPDomain.Entities;

namespace ERPDomain.Concrete
{
    public class EFSH_USERROLERepository : ISH_USERROLERepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<SH_USERROLE> SH_USERROLE
        {

            get { return context.SH_USERROLE; }

        }

        public void SaveSH_USERROLE(SH_USERROLE sh_userrole)
        {
            if (sh_userrole.ID == 0)
            {
                context.SH_USERROLE.Add(sh_userrole);
            }
            else
            {

                context.Entry(sh_userrole).State = EntityState.Modified;
                context.Entry(sh_userrole).Property(m => m.CreateDate).IsModified = false;
            }
            context.SaveChanges();
        }

        public void DeleteSH_USERROLE(SH_USERROLE sh_userrole)
        {
            context.SH_USERROLE.Remove(sh_userrole);
            context.SaveChanges();
        }
    }
}
