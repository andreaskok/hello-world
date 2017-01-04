using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IErrorLogRepository
    {
        IEnumerable<ErrorLog> ErrorLog { get; }
        void SaveErrorLog(ErrorLog errorLog);
        void DeleteErrorLog(ErrorLog errorLog);
    }
}
