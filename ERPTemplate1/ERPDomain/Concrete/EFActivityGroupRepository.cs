using System;
using System.Collections.Generic;
using System.Data.Entity;
using ERPDomain.Abstract;
using ERPDomain.Entities;

namespace ERPDomain.Concrete
{
    public class EFActivityGroupRepository : IActivityGroupRepository
    {
        private EFDbContextGL context = new EFDbContextGL();
        public IEnumerable<ActivityGroup> ActivityGroup
        {
            get { return context.ActivityGroup; }
        }

        public void SaveActivityGroup(ActivityGroup activitygroup)
        {
            if (activitygroup.ID == 0)
            {
                context.ActivityGroup.Add(activitygroup);
            }
            else
            {
                context.Entry(activitygroup).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteActivityGroup(ActivityGroup activitygroup)
        {
            context.ActivityGroup.Remove(activitygroup);
            context.SaveChanges();
        }
    }
}
