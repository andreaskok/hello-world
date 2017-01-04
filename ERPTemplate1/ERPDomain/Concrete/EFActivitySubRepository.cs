using System;
using System.Collections.Generic;
using System.Data.Entity;
using ERPDomain.Abstract;
using ERPDomain.Entities;

namespace ERPDomain.Concrete
{
    public class EFActivitySubRepository : IActivitySubRepository
    {
        private EFDbContextGL context = new EFDbContextGL();
        public IEnumerable<ActivitySub> ActivitySub
        {
            get { return context.ActivitySub; }
        }

        public void SaveActivitySub(ActivitySub activitysub)
        {
            if (activitysub.ID == 0)
            {
                context.ActivitySub.Add(activitysub);
            }
            else
            {
                context.Entry(activitysub).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteActivitySub(ActivitySub activitysub)
        {
            context.ActivitySub.Remove(activitysub);
            context.SaveChanges();
        }

    }

}
