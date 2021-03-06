﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IDfMasterRepository
    {
        IEnumerable<DfMaster> DfMaster { get; }
        void SaveDfMaster(DfMaster dfMaster);
        void DeleteDfMaster(DfMaster dfMaster);
    }
}
