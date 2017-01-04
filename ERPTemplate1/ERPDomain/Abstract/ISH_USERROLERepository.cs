using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface ISH_USERROLERepository
    {
        IEnumerable<SH_USERROLE> SH_USERROLE { get; }

        void SaveSH_USERROLE(SH_USERROLE sh_userrole);
        void DeleteSH_USERROLE(SH_USERROLE sh_userrole);
    }
}
