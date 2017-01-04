using System;
using System.Collections.Generic;

using ERPDomain.Abstract;
using ERPDomain.Entities;
using System.Data.Entity;

namespace ERPDomain.Concrete
{
    public class EFParentMenuRepository : IParentMenuRepository
    {
        private EFDbContext context = new EFDbContext();
        public IEnumerable<ParentMenu> ParentMenu
        {
            get { return context.ParentMenu; }
        }

        public void SaveParentMenu(ParentMenu parentMenu)
        {
            if (parentMenu.ID == 0)
            {
                context.ParentMenu.Add(parentMenu);
            }
            else
            {
                context.Entry(parentMenu).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteParentMenu(ParentMenu parentMenu)
        {
            context.ParentMenu.Remove(parentMenu);
            context.SaveChanges();
        }

    }
}
