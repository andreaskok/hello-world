using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IDebugLogRepository
    {
        IEnumerable<DebugLog> DebugLog { get; }
        void SaveDebugLog(DebugLog debugLog);
        void DeleteDebugLog(DebugLog debugLog);
    }
}
