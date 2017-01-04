using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IDfItemTypeRepository
    {
        IEnumerable<DfItemType> DfItemType { get; }
        void SaveDfItemType(DfItemType dfItemType);
        void DeleteDfItemType(DfItemType dfItemType);
    }
}
