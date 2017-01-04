using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface ISH_ROLERepository
    {
        IEnumerable<SH_ROLE> SH_ROLE { get; }

        void SaveSH_ROLE(SH_ROLE sh_role);
        void DeleteSH_ROLE(SH_ROLE sh_role);
    }
}
