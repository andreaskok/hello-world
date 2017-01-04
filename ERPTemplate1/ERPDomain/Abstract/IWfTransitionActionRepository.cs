﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfTransitionActionRepository
    {
        IEnumerable<WfTransitionAction> WfTransitionAction { get; }
        void SaveWfTransitionAction(WfTransitionAction wfTransitionAction);
        void DeleteWfTransitionAction(WfTransitionAction wfTransitionAction);
    }
}
