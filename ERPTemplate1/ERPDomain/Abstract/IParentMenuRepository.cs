using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IParentMenuRepository
    {
        IEnumerable<ParentMenu> ParentMenu { get; }
        void SaveParentMenu(ParentMenu parentMenu);
        void DeleteParentMenu(ParentMenu parentMenu);
    }

}
