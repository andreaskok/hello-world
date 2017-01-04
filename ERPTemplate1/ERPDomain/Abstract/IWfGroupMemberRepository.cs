using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfGroupMemberRepository
    {
        IEnumerable<WfGroupMember> WfGroupMember { get; }
        void SaveWfGroupMember(WfGroupMember wfGroupMember);
        void DeleteWfGroupMember(WfGroupMember wfGroupMember);
    }
}
