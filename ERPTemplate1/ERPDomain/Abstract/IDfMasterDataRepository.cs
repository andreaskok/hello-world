using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IDfMasterDataRepository
    {
        IEnumerable<DfMasterData> DfMasterData { get; }
        void SaveDfMasterData(DfMasterData dfMasterData);
        void DeleteDfMasterData(DfMasterData dfMasterData);
    }
}
