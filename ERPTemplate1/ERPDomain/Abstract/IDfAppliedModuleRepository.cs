using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IDfAppliedModuleRepository
    {
        IEnumerable<DfAppliedModule> DfAppliedModule { get; }
        void SaveDfAppliedModule(DfAppliedModule dfAppliedModule);
        void DeleteDfAppliedModule(DfAppliedModule dfAppliedModule);
    }
}
