using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfRequestDataRepository
    {
        IEnumerable<WfRequestData> WfRequestData { get; }
        void SaveWfRequestData(WfRequestData wfRequestData);
        void DeleteWfRequestData(WfRequestData wfRequestData);
    }
}
