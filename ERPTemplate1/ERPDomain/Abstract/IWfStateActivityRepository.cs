using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfStateActivityRepository
    {
        IEnumerable<WfStateActivity> WfStateActivity { get; }
        void SaveWfStateActivity(WfStateActivity wfStateActivity);
        void DeleteWfStateActivity(WfStateActivity wfStateActivity);
    }
}
