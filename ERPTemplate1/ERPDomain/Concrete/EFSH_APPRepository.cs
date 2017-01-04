using System.Collections.Generic;
using System.Data.Entity;
using ERPDomain.Abstract;
using ERPDomain.Entities;

namespace ERPDomain.Concrete
{
    public class EFSH_APPRepository : ISH_APPRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<SH_APP> SH_APP
        {

            get { return context.SH_APP; }

        }

        public void SaveSH_APP(SH_APP sh_app)
        {
            if (sh_app.ID == 0)
            {
                context.SH_APP.Add(sh_app);
            }
            else
            {

                context.Entry(sh_app).State = EntityState.Modified;
                //context.Entry(sh_app).Property(m => m.CreateDate).IsModified = false;
            }
            context.SaveChanges();
        }

        public void DeleteSH_APP(SH_APP sh_app)
        {
            context.SH_APP.Remove(sh_app);
            context.SaveChanges();
        }
    }
}
