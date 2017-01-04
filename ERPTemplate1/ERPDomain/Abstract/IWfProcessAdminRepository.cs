using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfProcessAdminRepository
    {
        IEnumerable<WfProcessAdmin> WfProcessAdmin { get; }
        void SaveWfProcessAdmin(WfProcessAdmin wfProcessAdmin);
        void DeleteWfProcessAdmin(WfProcessAdmin wfProcessAdmin);
    }
}
