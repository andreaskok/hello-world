using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IActivityGroupRepository
    {
        IEnumerable<ActivityGroup> ActivityGroup { get; }
        void SaveActivityGroup(ActivityGroup activitygroup);
        void DeleteActivityGroup(ActivityGroup activitygroup);
    }
}
