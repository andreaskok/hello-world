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
    public class EFSubMenuRepository : ISubMenuRepository
    {
        private EFDbContext context = new EFDbContext();
        public IEnumerable<SubMenu> SubMenu
        {
            get { return context.SubMenu; }
        }

        public void SaveSubMenu(SubMenu subMenu)
        {
            if (subMenu.ID == 0)
            {
                context.SubMenu.Add(subMenu);
            }
            else
            {
                context.Entry(subMenu).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteSubMenu(SubMenu subMenu)
        {
            context.SubMenu.Remove(subMenu);
            context.SaveChanges();
        }

    }
}
