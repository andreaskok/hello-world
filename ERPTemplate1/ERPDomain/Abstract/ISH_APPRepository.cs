using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface ISH_APPRepository
    {
        IEnumerable<SH_APP> SH_APP { get; }
        void SaveSH_APP(SH_APP sh_app);
        void DeleteSH_APP(SH_APP sh_app);
    }
}
