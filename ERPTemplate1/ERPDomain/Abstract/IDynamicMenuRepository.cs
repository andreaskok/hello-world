using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IDynamicMenuRepository
    {
        IEnumerable<DynamicMenu> DynamicMenu { get; }
        void SaveDynamicMenu(DynamicMenu dynamicMenu);
        void DeleteDynamicMenu(DynamicMenu dynamicMenu);
    }
}
