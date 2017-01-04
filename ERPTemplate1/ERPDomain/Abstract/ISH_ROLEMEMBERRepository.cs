using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface ISH_ROLEMEMBERRepository
    {
        IEnumerable<SH_ROLEMEMBER> SH_ROLEMEMBER { get; }

        void SaveSH_ROLEMEMBER(SH_ROLEMEMBER sh_rolemember);
        void DeleteSH_ROLEMEMBER(SH_ROLEMEMBER sh_rolemember);
    }
}
