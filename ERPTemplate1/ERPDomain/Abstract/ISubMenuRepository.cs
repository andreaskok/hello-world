using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface ISubMenuRepository
    {
        IEnumerable<SubMenu> SubMenu { get; }
        void SaveSubMenu(SubMenu subMenu);
        void DeleteSubMenu(SubMenu subMenu);
    }
}
