using System.Collections.Generic;
using ERPDomain.Entities;
using System;

namespace ERPDomain.Abstract
{
    public interface IActivitySubRepository
    {
        IEnumerable<ActivitySub> ActivitySub { get; }
        void SaveActivitySub(ActivitySub activitysub);
        void DeleteActivitySub(ActivitySub activitysub);
    }
}
