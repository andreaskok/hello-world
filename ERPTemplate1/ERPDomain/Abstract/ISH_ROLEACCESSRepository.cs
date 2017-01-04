using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface ISH_ROLEACCESSRepository
    {
        IEnumerable<SH_ROLEACCESS> SH_ROLEACCESS { get; }

        void SaveSH_ROLEACCESS(SH_ROLEACCESS sh_roleaccess);
        void DeleteSH_ROLEACCESS(SH_ROLEACCESS sh_roleaccess);
    }
}
