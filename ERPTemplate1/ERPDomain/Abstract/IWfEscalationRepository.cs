using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfEscalationRepository
    {
        IEnumerable<WfEscalation> WfEscalation { get; }
        void SaveWfEscalation(WfEscalation wfEscalation);
        void DeleteWfEscalation(WfEscalation wfEscalation);
    }
}
