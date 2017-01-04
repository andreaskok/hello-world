using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfRequestFileRepository
    {
        IEnumerable<WfRequestFile> WfRequestFile { get; }
        void SaveWfRequestFile(WfRequestFile wfRequestFile);
        void DeleteWfRequestFile(WfRequestFile wfRequestFile);
    }
}
