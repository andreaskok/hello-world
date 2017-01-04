using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfActivityTypeRepository
    {
        IEnumerable<WfActivityType> WfActivityType { get; }
        void SaveWfActivityType(WfActivityType wfActivityType);
        void DeleteWfActivityType(WfActivityType wfActivityType);
    }
}
