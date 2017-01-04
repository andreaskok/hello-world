using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface ISH_USERRepository
    {
        IEnumerable<SH_USER> SH_USER { get; }

        void SaveSH_USER(SH_USER sh_user);
        void DeleteSH_USER(SH_USER sh_user);
    }
}
