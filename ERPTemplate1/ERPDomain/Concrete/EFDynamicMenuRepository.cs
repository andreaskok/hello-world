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
    public class EFDynamicMenuRepository : IDynamicMenuRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<DynamicMenu> DynamicMenu
        {

            get { return context.DynamicMenu; }

        }

        public void SaveDynamicMenu(DynamicMenu dynamicMenu)
        {
            if (dynamicMenu.ID == 0)
            {
                context.DynamicMenu.Add(dynamicMenu);
            }
            else
            {

                context.Entry(dynamicMenu).State = EntityState.Modified;
                context.Entry(dynamicMenu).Property(m => m.CreateDate).IsModified = false;
            }
            context.SaveChanges();
        }

        public void DeleteDynamicMenu(DynamicMenu dynamicMenu)
        {
            context.DynamicMenu.Remove(dynamicMenu);
            context.SaveChanges();
        }
    }
}
