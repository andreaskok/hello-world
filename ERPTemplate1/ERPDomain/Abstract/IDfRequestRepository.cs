using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IDfRequestRepository
    {
        IEnumerable<DfRequest> DfRequest { get; }
        void SaveDfRequest(DfRequest dfRequest);
        void DeleteDfRequest(DfRequest dfRequest);
    }
}
