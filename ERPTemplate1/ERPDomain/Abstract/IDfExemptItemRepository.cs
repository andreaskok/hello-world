using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IDfExemptItemRepository
    {
        IEnumerable<DfExemptItem> DfExemptItem { get; }
        void SaveDfExemptItem(DfExemptItem dfExemptItem);
        void DeleteDfExemptItem(DfExemptItem dfExemptItem);
    }
}
