using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfTransitionActivityRepository
    {
        IEnumerable<WfTransitionActivity> WfTransitionActivity { get; }
        void SaveWfTransitionActivity(WfTransitionActivity wfTransitionActivity);
        void DeleteWfTransitionActivity(WfTransitionActivity wfTransitionActivity);
    }
}
