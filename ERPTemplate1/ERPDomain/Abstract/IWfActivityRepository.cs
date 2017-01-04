using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfActivityRepository
    {
        IEnumerable<WfActivity> WfActivity { get; }
        void SaveWfActivity(WfActivity wfActivity);
        void DeleteWfActivity(WfActivity wfActivity);
    }
}
