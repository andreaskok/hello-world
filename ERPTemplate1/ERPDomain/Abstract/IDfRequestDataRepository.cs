using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IDfRequestDataRepository
    {
        IEnumerable<DfRequestData> DfRequestData { get; }
        void SaveDfRequestData(DfRequestData dfRequestData);
        void DeleteDfRequestData(DfRequestData dfRequestData);
    }
}
